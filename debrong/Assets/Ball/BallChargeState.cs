using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallChargeState : MonoBehaviour {
    BallMovement movement;
    TrailRenderer trail;
    ParticleSystem psys;

    bool player_is_charged;
    bool is_charged;


    // ----------------------------------------------------------------------------------------------------------------
    // Public 

    public bool CheckIsCharged() {
        return this.is_charged;
    }


    // ----------------------------------------------------------------------------------------------------------------
    // Builtins 

    private void OnEnable() => PlayerChargingEventManager.SubscribeChangedEvents(HandleChangeEvent);
    private void OnDisable() => PlayerChargingEventManager.UnsubscribeChangedEvents(HandleChangeEvent);

    private void Awake() {
        this.player_is_charged = false;
        this.is_charged = false;
        this.movement = this.GetComponent<BallMovement>();
        this.trail = this.GetComponent<TrailRenderer>();
        this.psys = this.GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        // Only react to player collisions (the only way to 'charge' this object)
        bool is_player_collision = collision.collider.CompareTag("Player");
        if (is_player_collision) {

            // Only disable charging if we're currently charged & the player is no longer charged
            bool disable_charge = this.is_charged && !this.player_is_charged;
            if (disable_charge) {
                this.is_charged = false;
                UpdateTrailEffects();
            }
        }
    }


    // ----------------------------------------------------------------------------------------------------------------
    // Behavior specific 

    void HandleChangeEvent(bool player_is_charged) {

        // As soon as the player is charged, so are we
        // -> not true in reverse, we only lose charge after *hitting* uncharged player
        this.player_is_charged = player_is_charged;
        if (player_is_charged) {
            this.is_charged = true;
        }
        UpdateTrailEffects();
    }

    void UpdateTrailEffects() {

        // HACKY AF. Directly telling movement component to change behavior... yuck
        // - would be better to have separate 'ball is charged' state to deal with this
        // - no time to fix this though
        movement.UpdateFromChargeState(this.is_charged);

        // Switch trail effects according to (ball) charge state
        if (this.is_charged) {
            this.trail.enabled = false;
            this.psys.Play();
        } else {
            this.trail.enabled = true;
            this.psys.Stop();
        }
    }

}
