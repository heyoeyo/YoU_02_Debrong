using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFX : MonoBehaviour
{

    [SerializeField] private float explosion_duration = 1f;
    [SerializeField] private float explosion_decay_rate = 0.9f;

    ParticleSystem psys;
    PointEffector2D pt_effector;


    float explosive_force;
    float end_time;

    private void Awake() {
        pt_effector = this.GetComponent<PointEffector2D>();
        psys = this.GetComponent<ParticleSystem>();
        explosive_force = pt_effector.forceMagnitude;
    }

    private void Start() {
        end_time = Time.time + explosion_duration;
    }

    private void Update() {
        if(!psys.IsAlive()) {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate() {
        if (Time.time > end_time) {
            explosive_force = explosive_force * explosion_decay_rate;
        }
    }
}
