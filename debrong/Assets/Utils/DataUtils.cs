using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataUtils
{
    public static Vector3[] V2ToV3Array(Vector2[] v2_input) {

        Vector3[] output = new Vector3[v2_input.Length];
        for (int i = 0; i < v2_input.Length; i++) {
            output[i] = new Vector3(v2_input[i].x, v2_input[i].y, 0);
        }

        return output;
    }

    public static Vector2[] V3ToV2Array(Vector3[] v3_input) {

        Vector2[] output = new Vector2[v3_input.Length];
        for (int i = 0; i < v3_input.Length; i++) {
            output[i] = new Vector2(v3_input[i].x, v3_input[i].y);
        }

        return output;
    }

    public static Vector2 PolarToV2(float radius, float angle_deg) {

        float angle_rad = Mathf.Deg2Rad * angle_deg;
        float x_term = Mathf.Cos(angle_rad);
        float y_term = Mathf.Sin(angle_rad);

        return new Vector2(x_term, y_term) * radius;
    }

    public static Vector2 Rotate2D(Vector2 vec, float angle) {

        float rot_x = vec.x * Mathf.Cos(angle) - vec.y * Mathf.Sin(angle);
        float rot_y = vec.x * Mathf.Sin(angle) + vec.y * Mathf.Cos(angle);

        return new Vector2(rot_x, rot_y);
    }
}

public struct MinMaxFloat {

    public float min;
    public float max;

    public MinMaxFloat(float min, float max) {
        this.min = min;
        this.max = max;
    }

    public float Random() {
        return UnityEngine.Random.Range(this.min, this.max);
    }
}

public struct MinMaxInt {

    public int min;
    public int max;

    public MinMaxInt(int min, int max) {
        this.min = min;
        this.max = max;
    }

    public int RandomInclusive() {
        return UnityEngine.Random.Range(this.min, this.max + 1);
    }
}
