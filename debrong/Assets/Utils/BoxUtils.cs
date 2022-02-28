using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoxUtils
{
}
public struct BoxBounds {
    public Vector2 center;
    public float left_x, right_x;
    public float top_y, bottom_y;

    public float width { get => (tr.x - tl.x); }
    public float height { get => (tr.y - br.y); }

    Vector2 tl { get => new Vector2(left_x, top_y); }
    Vector2 tr { get => new Vector2(right_x, top_y); }
    Vector2 bl { get => new Vector2(left_x, bottom_y); }
    Vector2 br { get => new Vector2(right_x, bottom_y); }


    public BoxBounds(Vector2 center, float width, float height) {

        this.center = center;

        float w_halfed = width / 2f;
        float h_halfed = height / 2f;
        this.left_x = center.x - w_halfed;
        this.right_x = center.x + w_halfed;
        this.top_y = center.y + h_halfed;
        this.bottom_y = center.y - h_halfed;
    }

    public BoxBounds(Vector2 top_left, Vector2 bottom_right) {

        this.center = (top_left + bottom_right) / 2f;

        this.left_x = top_left.x;
        this.right_x = bottom_right.x;
        this.top_y = top_left.y;
        this.bottom_y = bottom_right.y;
    }

    public void DrawAsGizmo() {
        Gizmos.DrawLine(tl, tr);
        Gizmos.DrawLine(tr, br);
        Gizmos.DrawLine(br, bl);
        Gizmos.DrawLine(bl, tl);
    }

    public float RandomX() {
        return Mathf.Lerp(left_x, right_x, Random.Range(0f, 1f));
    }
    public float RandomY() {
        return Mathf.Lerp(bottom_y, top_y, Random.Range(0f, 1f));
    }

    public BoxBounds[] SubdivideX(int num_regions) {

        BoxBounds[] output = new BoxBounds[num_regions];

        float sub_width = this.width / num_regions;
        float sub_height = this.height;
        float center_y = this.center.y;
        Vector2 center = new Vector2(0, center_y);

        for (int i = 0; i < output.Length; i++) {
            float t = (0.5f + i) / num_regions;
            center.x = Mathf.Lerp(this.left_x, this.right_x, t);
            output[i] = new BoxBounds(center, sub_width, sub_height);
        }

        return output;
    }

    public BoxBounds[] SubdivideY(int num_regions) {

        BoxBounds[] output = new BoxBounds[num_regions];

        float sub_width = this.width;
        float sub_height = this.height / num_regions;
        float center_x = this.center.x;
        Vector2 center = new Vector2(center_x, 0);

        for (int i = 0; i < output.Length; i++) {
            float t = (0.5f + i) / num_regions;
            center.y = Mathf.Lerp(this.bottom_y, this.top_y, t);
            output[i] = new BoxBounds(center, sub_width, sub_height);
        }

        return output;
    }
}
