using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AnchoredMotion : MonoBehaviour
{
    // Anchor point properties
    [Header("Anchor Properties")]
    [SerializeField] private Vector2 anchor_point = Vector2.zero;
    [SerializeField] private float anchor_force = 500;
    [SerializeField] private float anchor_distance_threshold = 7;

    [Header("Initial Speed")]
    [SerializeField, Range(0f, 1f)] private float min_speed_scale = 0.05f;
    [SerializeField, Range(0f, 1f)] private float max_speed_scale = 1f;

    Rigidbody2D rb;
    float sqrtmass;

    bool too_far_from_anchor = false;
    Vector2 force_vector;
    Vector2 curr_pos_2d;


    // ----------------------------------------------------------------------------------------------------------------

    public void SetRandomVelocity(float max_speed) {

        float random_speed = Random.Range(min_speed_scale, max_speed_scale) * max_speed;
        float random_angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float start_x_velo = random_speed * Mathf.Cos(random_angle);
        float start_y_velo = random_speed * Mathf.Sin(random_angle);

        this.rb.velocity = new Vector2(start_x_velo, start_y_velo);
    }

    public void SetAnchorPoint(Vector2 point, float force, float distance_threshold) {
        this.anchor_point = point;
        this.anchor_force = force;
        this.anchor_distance_threshold = distance_threshold;
    }

    // ----------------------------------------------------------------------------------------------------------------
    // Built-ins

    private void Awake() {
        this.rb = GetComponent<Rigidbody2D>();
        SetAnchorPoint(this.transform.position, anchor_force, anchor_distance_threshold);
    }

    void Start() {
        this.sqrtmass = Mathf.Sqrt(this.rb.mass);
    }

    void Update() {
        too_far_from_anchor = Vector2.Distance(anchor_point, this.transform.position) > anchor_distance_threshold;
    }

    private void FixedUpdate() {
        
        // Only apply tethering/anchor effect when 'far away' from anchor point
        if (too_far_from_anchor) {
            curr_pos_2d = this.transform.position;
            force_vector = anchor_force * (anchor_point - curr_pos_2d).normalized;
            this.rb.AddForce(force_vector * sqrtmass * Time.fixedDeltaTime);
        }
    }

    // ----------------------------------------------------------------------------------------------------------------
    // Debugging

    private void OnDrawGizmos() {
        if (too_far_from_anchor) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(anchor_point, this.transform.position);
        }
    }
}
