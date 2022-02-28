using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelIndicator : MonoBehaviour
{
    [SerializeField] private Text level_text;
    [SerializeField] private float on_screen_time = 2.5f;

    private void Start() {
        StartCoroutine(DisplayLevelText());
    }

    IEnumerator DisplayLevelText() {

        // Figure out what level # to display
        int level_num = SceneManager.GetActiveScene().buildIndex;
        level_text.text = string.Format("Level {0}", level_num);

        // Fade text in for effect
        level_text.canvasRenderer.SetAlpha(0f);
        level_text.CrossFadeAlpha(1f, 0.5f, false);

        // Wait a bit before we trigger the game start
        yield return new WaitForSeconds(on_screen_time);
        GameStateEventManager.TriggerStartEvent();

        // Hacky way to avoid proper state management...
        StartCoroutine(DisappearAndDelete());        
    }

    IEnumerator DisappearAndDelete() {

        // Fade out the text
        float fade_out_time = 0.5f;
        level_text.CrossFadeAlpha(0, fade_out_time, false);
        yield return new WaitForSeconds(fade_out_time + 0.1f);

        // Remove the canvas itself once we're done
        level_text.canvasRenderer.gameObject.SetActive(false);
    }
}
