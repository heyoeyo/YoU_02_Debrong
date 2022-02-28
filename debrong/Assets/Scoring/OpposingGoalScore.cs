using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class OpposingGoalScore : MonoBehaviour
{
    [SerializeField] private bool _debug_win_on_score = false;

    ParticleSystem psys;
    bool is_triggering;


    // ----------------------------------------------------------------------------------------------------------------
    // Builtins

    private void Awake() {
        this.psys = this.GetComponent<ParticleSystem>();
        this.is_triggering = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GameStateEventManager.TriggerScoreEvent();
        TriggerParticlesAndNextScene();

    }


    // ----------------------------------------------------------------------------------------------------------------
    // Behavior specific

    void TriggerParticlesAndNextScene() {
        if (!this.is_triggering) {
            StartCoroutine(LoadSceneAfterParticles());
        }
    }

    IEnumerator LoadSceneAfterParticles() {

        this.is_triggering = true;

        // Start the particle FX and wait for them to finish
        this.psys.Play();
        while (this.psys.IsAlive()) {
            yield return new WaitForSeconds(0.25f);
        }

        LoadNextScene();

        this.is_triggering = false;
    }

    void LoadNextScene() {

        // Figure out the next scene using the current scene index
        int curr_scene_idx = SceneManager.GetActiveScene().buildIndex;
        int next_scene_idx = curr_scene_idx + 1;

        // For debugging, we can skip to show the end screen for testing
        if(_debug_win_on_score) {
            next_scene_idx = SceneManager.sceneCountInBuildSettings - 1;
        }

        SceneManager.LoadScene(next_scene_idx);
    }
}