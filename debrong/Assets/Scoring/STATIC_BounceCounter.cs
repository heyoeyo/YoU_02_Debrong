using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class STATIC_BounceCounter
{
    static int bounce_count = 0;

    public static void Increment() {
        bounce_count += 1;
    }

    public static void ResetCount() {
        bounce_count = 0;
    }

    public static string GetBPM(float total_seconds) {
        float total_minutes = total_seconds / 60f;
        float bounces_per_minute = bounce_count / total_minutes;
        return Mathf.RoundToInt(bounces_per_minute).ToString();
    }
}
