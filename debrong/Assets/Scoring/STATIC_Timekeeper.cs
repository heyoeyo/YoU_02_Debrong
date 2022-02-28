using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class STATIC_Timekeeper
{

    static float start_time = 0;

    public static void StartTiming() {
        start_time = Time.time;
    }

    public static float GetTotalSeconds() {
        return Mathf.Max(Time.time - start_time, 1f);
    }
}
