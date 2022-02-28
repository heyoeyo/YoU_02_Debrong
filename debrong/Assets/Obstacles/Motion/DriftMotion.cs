using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DriftMotion : MonoBehaviour
{
    // Anchor point properties
    [Header("Drift Properties")]
    [SerializeField] private float max_drift_speed = 7f;
    [SerializeField] private Vector2 drift_direction;
    [SerializeField] private float drift_force = 10f;
    [SerializeField, Range(0f, 1f)] private float drift_force_variation = 0.1f;

    Rigidbody2D rb;

    Vector2 static_force_vector;
    float _min_force_multiplier;
    float _max_force_multiplier;


    // ----------------------------------------------------------------------------------------------------------------
    // Built-ins

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();

        // Make sure drift vector is set (otherwise force downward)
        if (drift_direction.magnitude < 0.01f) {
            drift_direction = Vector2.down;
        }

        // Pre-calculate some values to avoid doing it on every update
        static_force_vector = drift_direction.normalized * drift_force;
        _min_force_multiplier = 1f - drift_force_variation;
        _max_force_multiplier = 1f + drift_force_variation;
    }

    private void FixedUpdate() {

        if (this.rb.velocity.magnitude < this.max_drift_speed) {
            float force_variation = Random.Range(_min_force_multiplier, _max_force_multiplier);
            this.rb.AddForce(static_force_vector * force_variation);
        }
    }

    // ----------------------------------------------------------------------------------------------------------------

    public void SetRandomVelocity(float max_speed) {

        float random_speed = Random.Range(0.1f, 1f) * max_speed;
        float random_angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float start_x_velo = random_speed * Mathf.Cos(random_angle);
        float start_y_velo = random_speed * Mathf.Sin(random_angle);

        this.rb.velocity = new Vector2(start_x_velo, start_y_velo);
    }

    public void SetRandomDrift(Vector2 base_direction, float angle_range_deg, float max_drift_speed) {

        // Randomize the base direction given (by +/- the angle range)
        Vector2 random_direction = Quaternion.Euler(0, 0, Random.Range(-1f, 1f) * angle_range_deg) * base_direction;
        this.drift_direction = random_direction.normalized;

        this.max_drift_speed = max_drift_speed;
    }


}
