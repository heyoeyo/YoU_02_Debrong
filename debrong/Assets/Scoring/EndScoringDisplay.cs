using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScoringDisplay : MonoBehaviour
{
    [SerializeField] private Text lost_balls;
    [SerializeField] private Text bounces_per_min;
    [SerializeField] private Text total_time;

    void Start()
    {
        float total_time_sec = STATIC_Timekeeper.GetTotalSeconds();

        lost_balls.text = STATIC_LostBallsCounter.GetCount();
        bounces_per_min.text = STATIC_BounceCounter.GetBPM(total_time_sec);
        total_time.text = Mathf.RoundToInt(total_time_sec).ToString();
    }
}
