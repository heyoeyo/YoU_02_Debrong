using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterFromBallImpact : MonoBehaviour
{
    [Header("Shatter Behavior")]
    [SerializeField] private ShardInitializer shatter_prefab;
    [SerializeField] private float shatter_speed_threshold = 24f;

    PolygonCollider2D polycollider;
    IShatterable shapegen;

    private void Awake() {
        this.polycollider = this.GetComponent<PolygonCollider2D>();
        this.shapegen = this.GetComponent<IShatterable>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        // Don't react to anything other than ball collisions
        bool is_ball_collision = collision.collider.name == "Ball";
        if (!is_ball_collision) {
            return;
        }

        // Ignore if the impact isn't 'fast enough'
        bool is_fast_collision = (collision.relativeVelocity.magnitude > shatter_speed_threshold);
        if (!is_fast_collision) {
            return;
        }

        // Shatter if the ball is charged
        BallChargeState ball = collision.collider.GetComponent<BallChargeState>();
        bool ball_is_charged = ball.CheckIsCharged();
        if (ball_is_charged) {
            float ball_mass = collision.collider.attachedRigidbody.mass;
            Vector2 ball_momentum = ball_mass * collision.relativeVelocity;
            SpawnShatterPieces(ball_momentum);
            Destroy(this.gameObject);
        }
    }

    void SpawnShatterPieces(Vector2 momentum) {

        // For clarity
        Vector3 root_spawn_pos = this.transform.position;
        Quaternion root_spawn_rot = this.transform.rotation;
        Transform parent = this.transform.parent;
        float density = this.polycollider.density;
        Material material = this.GetComponent<MeshRenderer>().sharedMaterial;

        // Split momentum into separate amounts, to share among pieces
        ShatterPiece[] shatter_pieces = this.shapegen.Shatter();
        Vector2 shatter_force = momentum * (1f / shatter_pieces.Length) * (1f / Time.fixedDeltaTime);

        // Have shape generate it's own shatter shapes
        foreach (ShatterPiece piece in shatter_pieces) {
            Vector3 spawn_pos = root_spawn_pos + (Vector3)piece.center_point;
            Quaternion spawn_rot = root_spawn_rot * Quaternion.Euler(0, 0, Random.Range(-1f, 1f) * 45f);
            ShardInitializer new_piece = Instantiate(shatter_prefab, spawn_pos, spawn_rot, parent);
            ShardSpec shard_spec = new ShardSpec(piece, density, material);
            new_piece.InitOnShatter(shard_spec, shatter_force);
        }
    }
}
