using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class STATIC_LostBallsCounter
{
    static int lost_balls_count = 0;

    public static void Increment() {
        lost_balls_count += 1;
        Debug.Log("LOSTBALLS: " + lost_balls_count);
    }

    public static void ResetCount() {
        lost_balls_count = 0;
    }

    public static string GetCount() {
        return lost_balls_count.ToString();
    }
}
