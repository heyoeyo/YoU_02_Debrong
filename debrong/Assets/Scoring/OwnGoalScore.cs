using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class OwnGoalScore : MonoBehaviour
{
    ParticleSystem psys;
    bool is_triggering;

    // ----------------------------------------------------------------------------------------------------------------
    // Builtins

    private void Awake() {
        this.psys = this.GetComponent<ParticleSystem>();
        this.is_triggering = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        STATIC_LostBallsCounter.Increment();

        GameStateEventManager.TriggerScoreEvent();
        TriggerScoringEffects();
    }


    // ----------------------------------------------------------------------------------------------------------------
    // Behavior specific

    void TriggerScoringEffects() {
        if (!this.is_triggering) {
            StartCoroutine(HandleParticleEffects());
        }
    }

    IEnumerator HandleParticleEffects() {

        this.is_triggering = true;

        // Play the particles and wait for them to finish
        this.psys.Play();
        while(this.psys.IsAlive()) {
            yield return new WaitForSeconds(0.25f);
        }
        this.psys.Stop();

        // Signal a game start event to reset the ball
        GameStateEventManager.TriggerStartEvent();

        this.is_triggering = false;
    }
}