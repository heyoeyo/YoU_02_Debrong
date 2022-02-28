using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

    [Header("Initial Properties")]
    [SerializeField] private float max_regular_move_speed = 20f;
    [SerializeField] private float max_charged_move_speed = 35f;
    [SerializeField] private float reacceleration_rate = 10f;
    [SerializeField, Range(0f, 1f)] private float min_horizontal_speed_component = 0.3f;
    [SerializeField] private Vector2 restart_direction = new Vector2(1f, 0f);

    TrailRenderer trail;
    Rigidbody2D rb;
    Vector2 start_position;
    float _curr_max_speed;


    // ----------------------------------------------------------------------------------------------------------------
    // Public

    public void UpdateFromChargeState(bool is_charged) {
        this.rb.mass = is_charged ? 5f : 1f;
        this._curr_max_speed = is_charged ? max_charged_move_speed : max_regular_move_speed;
    }


    // ----------------------------------------------------------------------------------------------------------------
    // Builtins

    private void OnEnable() {
        GameStateEventManager.SubscribePlayStart(StartBall);
        GameStateEventManager.SubscribeScore(HideBall);
    }
    private void OnDisable() {
        GameStateEventManager.UnsubscribePlayStart(StartBall);
        GameStateEventManager.UnsubscribeScore(HideBall);
    }

    private void Awake() {
        this.start_position = this.transform.position;
        this.rb = GetComponent<Rigidbody2D>();
        this.trail = this.GetComponent<TrailRenderer>();

        this.ResetBall();
    }

    private void Update() {

        // Allow for manually resetting the ball in editor-mode
        #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("SPACE PRESSED");
                StartBall();
            }
        #endif
    }

    private void FixedUpdate() {

        // Pre-calculate speed, since we need it in multiple places
        Vector2 curr_velo = this.rb.velocity;
        float curr_speed = curr_velo.magnitude;

        // Speed up or slow down ball if it isn't at target speed
        ForceTargetSpeed(curr_velo, curr_speed);

        // Make sure the ball is generally travelling sideways
        ForceSidewaysMovement(curr_velo, curr_speed);

        // Special case to handle funny physics results
        ResetIfOutOfBounds();
    }


    // ----------------------------------------------------------------------------------------------------------------
    // Behavior specific

    void ResetIfOutOfBounds() {

        bool bad_x = Mathf.Abs(this.transform.position.x) > 80f;
        bool bad_y = Mathf.Abs(this.transform.position.y) > 80f;
        if (bad_x || bad_y) {
            StartBall();
        }
    }

    private void HideBall() {
        this.transform.position = Vector2.left * 40f;
        this.rb.velocity = Vector2.zero;
        this.trail.Clear();
    }

    private void ResetBall() {
        this.transform.position = this.start_position;
        this.rb.velocity = Vector2.zero;
        this.trail.Clear();
    }

    private void StartBall() {
        ResetBall();
        StartCoroutine(DelayToMove());
    }

    IEnumerator DelayToMove() {
        yield return new WaitForSeconds(0.5f);
        this.rb.velocity = restart_direction.normalized * this._curr_max_speed;
    }

    void ForceTargetSpeed(Vector2 curr_velo, float curr_speed) {
        float speed_delta = (this._curr_max_speed - curr_speed);
        bool not_at_target_speed = (Mathf.Abs(speed_delta) > 1f);
        if (not_at_target_speed) {
            Vector2 correction_direction = Mathf.Sign(speed_delta) * curr_velo.normalized;
            float reaccel_strength = reacceleration_rate * this.rb.mass;
            this.rb.AddForce(reaccel_strength * correction_direction * Time.fixedDeltaTime);
            //Debug.Log("CURR SPEED: " + curr_speed);
        }
    }

    void ForceSidewaysMovement(Vector2 curr_velo, float curr_speed) {
        float curr_sideways_speed_fraction = curr_velo.x / curr_speed;
        bool not_moving_sideways_fast_enough = (curr_sideways_speed_fraction < min_horizontal_speed_component);
        if (not_moving_sideways_fast_enough) {
            Vector2 sideways_direction = Vector2.right * Mathf.Sign(curr_velo.x);
            float reaccel_strength = reacceleration_rate * this.rb.mass * 2f;
            this.rb.AddForce(reaccel_strength * sideways_direction * Time.fixedDeltaTime);
        }
    }
}