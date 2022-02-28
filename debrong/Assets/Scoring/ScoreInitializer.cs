using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreInitializer : MonoBehaviour
{
    void Start()
    {
        STATIC_Timekeeper.StartTiming();
        STATIC_LostBallsCounter.ResetCount();
        STATIC_BounceCounter.ResetCount();
    }

}
