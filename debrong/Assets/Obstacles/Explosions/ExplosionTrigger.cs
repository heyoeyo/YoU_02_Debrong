using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{

    [SerializeField] ExplosionFX explode_fx_prefab;
    [SerializeField] private float trigger_collision_speed = 15f;
    [SerializeField] private float explosive_strength = 1f;
    [SerializeField] private float impact_delay_time = 2f;
    [SerializeField] private float chain_delay_time = 0.4f;
    [SerializeField] private float chain_radius = 10f;
    [SerializeField] bool _debug_explode_on_start;

    ParticleSystem countdown_psys;
    bool has_been_triggered = false;


    private void Awake() {

        countdown_psys = this.GetComponent<ParticleSystem>();

        if (_debug_explode_on_start) {
            SetExplosionCountdown(impact_delay_time);
        }
    }

    private void OnEnable() {
        ExplosionEventManager.ExplosionEvent += TriggerChainExplosion;
    }

    private void OnDisable() {
        ExplosionEventManager.ExplosionEvent -= TriggerChainExplosion;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        bool is_high_speed_collision = (collision.relativeVelocity.magnitude > trigger_collision_speed);
        if (is_high_speed_collision) {
            SetExplosionCountdown(impact_delay_time);
        }
    }

    void TriggerChainExplosion(ExplosionParameters ex_params) {

        // Trigger chain explosion if we're 'close enough' to the explosion
        Vector2 away_vec = ((Vector2)this.transform.position) - ex_params.position;
        Vector2 away_direction = away_vec.normalized;
        if (away_vec.magnitude < chain_radius) {
            SetExplosionCountdown(chain_delay_time * Random.Range(0.75f, 1.25f));
        }
    }

    void SetExplosionCountdown(float delay_time) {

        // Don't re-set the countdown if we've already triggered it
        if (has_been_triggered) {
            return;
        }

        has_been_triggered = true;
        countdown_psys.Play();
        StartCoroutine(DelayToExplosion(delay_time));
    }

    IEnumerator DelayToExplosion(float delay_time) {

        // Delay, then create explosion animation object & remove parent object
        yield return new WaitForSeconds(delay_time);

        // Trigger event for other objects to react to
        Vector2 explosion_pos = this.transform.position;
        ExplosionParameters.TriggerExplosionEvent(explosion_pos, explosive_strength);

        // Set off explosion effects
        Instantiate(explode_fx_prefab, this.transform.position, Quaternion.identity);

        // Finally, remove this object for good
        Destroy(this.gameObject);

    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, chain_radius);
    }
}


        // sound effect
        // screen shake?