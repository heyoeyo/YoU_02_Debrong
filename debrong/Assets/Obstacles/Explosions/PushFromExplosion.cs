using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PushFromExplosion : MonoBehaviour
{

    [Header("Speed Limiting")]
    [SerializeField] private bool limit_max_speed = true;
    [SerializeField] private float max_speed_from_explosion = 35f;

    Rigidbody2D rb;

    private void Awake() {
        this.rb = this.GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        ExplosionEventManager.ExplosionEvent += RespondToExplosion;
    }

    private void OnDisable() {
        ExplosionEventManager.ExplosionEvent -= RespondToExplosion;
    }

    void RespondToExplosion(ExplosionParameters ex_params) {        

        Vector2 explosive_force = ExplosionEventManager.CalculateExplosionForce(ex_params, this.transform.position);
        if (limit_max_speed) {
            explosive_force = ExplosionEventManager.LimitForceFromExplosion(explosive_force, max_speed_from_explosion, this.rb.mass);
        }

        this.rb.AddForce(explosive_force);
    }
}
