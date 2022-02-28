using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(IShatterable))]
public class ShatterFromExplosions : MonoBehaviour
{

    [Header("Shatter Behavior")]
    [SerializeField] private ShardInitializer shatter_prefab;
    [SerializeField] private float shatter_speed_threshold = 10f;

    PolygonCollider2D polycollider;
    Rigidbody2D rb;
    IShatterable shapegen;

    private void Awake() {
        this.polycollider = this.GetComponent<PolygonCollider2D>();
        this.rb = this.GetComponent<Rigidbody2D>();
        this.shapegen = this.GetComponent<IShatterable>();

    }

    private void OnEnable() {
        ExplosionEventManager.ExplosionEvent += RespondToExplosion;
    }

    private void OnDisable() {
        ExplosionEventManager.ExplosionEvent -= RespondToExplosion;
    }

    void RespondToExplosion(ExplosionParameters ex_params) {
        Vector2 explosive_force = ExplosionEventManager.CalculateExplosionForce(ex_params, this.transform.position);
        float speed_from_explosion = ExplosionEventManager.SpeedFromForce(explosive_force, this.rb.mass);
        if (speed_from_explosion > shatter_speed_threshold) {
            SpawnShatterPieces(ex_params, explosive_force);
            Destroy(this.gameObject);
        }
    }
    
    void SpawnShatterPieces(ExplosionParameters ex_params, Vector2 explosive_force) {

        // For clarity
        Vector3 root_spawn_pos = this.transform.position;
        Quaternion root_spawn_rot = this.transform.rotation;
        Transform parent = this.transform.parent;
        float density = this.polycollider.density;
        Material material = this.GetComponent<MeshRenderer>().sharedMaterial;

        // Have shape generate it's own shatter shapes
        ShatterPiece[] shatter_pieces = this.shapegen.Shatter();
        foreach (ShatterPiece piece in shatter_pieces) {
            Vector3 spawn_pos = root_spawn_pos + (Vector3) piece.center_point;
            Quaternion spawn_rot = root_spawn_rot * Quaternion.Euler(0, 0, Random.Range(-1f, 1f) * 45f);
            ShardInitializer new_piece = Instantiate(shatter_prefab, spawn_pos, spawn_rot, parent);
            ShardSpec shard_spec = new ShardSpec(piece, density, material);
            new_piece.InitOnShatter(shard_spec, explosive_force);
        }
    }
}
