using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharging : MonoBehaviour {

    [SerializeField] int num_bounces_to_charge = 5;
    [SerializeField] float charge_lifetime = 5f;
    [SerializeField] bool charge_on_startup = false;

    int bounce_count;
    bool is_charged;

    void Awake() {
        // Hard-coded 'default' initial state
        this.bounce_count = 0;
        this.is_charged = false;
    }

    private void Start() {

        // For debugging mainly
        if (charge_on_startup) {
            this.bounce_count = num_bounces_to_charge;
            this.is_charged = true;
        }

        // Make sure initial state is broadcast
        SignalChangeEvent();
        SignalChargingEvent();
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        bool is_ball_collision = collision.collider.CompareTag("ball");
        if (is_ball_collision) {
            HandleBallCollision();
        }
    }

    void HandleBallCollision() {

        // Do nothing while in charged state
        if (this.is_charged) {
            return;
        }

        // Update bounce count and signal current charging amount
        this.bounce_count += 1;
        SignalChargingEvent();

        // Signal 'is charged' event if needed
        this.is_charged = (this.bounce_count >= this.num_bounces_to_charge);
        if (this.is_charged) {
            StartCoroutine(ChargeCountDown());
            SignalChangeEvent();
        }

        //Debug.Log("BOUNCE COUNT: " + bounce_count);

    }

    float GetChargeAmount() {
        return (float)this.bounce_count / (float)this.num_bounces_to_charge;
    }

    void SignalChangeEvent() {
        PlayerChargingEventManager.TriggerStateChange(this.is_charged);
    }

    void SignalChargingEvent(float charge_amount) {
        PlayerChargingEventManager.TriggerChargingEvent(charge_amount);
    }
    
    void SignalChargingEvent() {
        float charge_amount = GetChargeAmount();
        PlayerChargingEventManager.TriggerChargingEvent(charge_amount);
    }

    IEnumerator ChargeCountDown() {

        // Set up looping countdown, used to signal charge amount/decay
        float charge_amount = 1f;
        float curr_time = Time.time;
        float end_time = curr_time + charge_lifetime;
        while (true) {
            yield return new WaitForSeconds(0.3f);

            // Countdown timer + signal charge state
            curr_time = Time.time;
            charge_amount = Mathf.Clamp01((end_time - curr_time) / charge_lifetime);
            SignalChargingEvent(charge_amount);

            // Bail once we've passed timer
            if (Time.time > end_time) {
                break;
            }
        }

        // Make sure to disable charge state on completion
        ResetCharging();
    }

    void ResetCharging() {
        this.is_charged = false;
        this.bounce_count = 0;
        SignalChangeEvent();
    }

}
