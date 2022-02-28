using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Camera camera_ref;
    [SerializeField, Range(0f, 1f)] private float follow_strength = 0.7f;
    [SerializeField] private bool use_rigidbody_for_movement = false;

    Rigidbody2D rb;
    float target_y;
    Vector2 move_pos;
    float _follow_t;

    private void Awake() {

        // Init position data on startup
        this.target_y = this.transform.position.y;
        this.move_pos = this.transform.position;

        // Pre-calculate follow coefficient, so we're not doing power-calcs on every update
        this._follow_t = Mathf.Pow(follow_strength, 3);

        // Get reference to rigidbody for physics updates
        this.rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        Vector2 mouse_pos = camera_ref.ScreenToWorldPoint(Input.mousePosition);
        target_y = mouse_pos.y;
    }

    private void FixedUpdate() {
        move_pos.y = Mathf.Lerp(this.transform.position.y, target_y, _follow_t);
        if (use_rigidbody_for_movement) {
            this.rb.MovePosition(move_pos);
        } else {
            this.transform.position = move_pos;
        }
    }
}
