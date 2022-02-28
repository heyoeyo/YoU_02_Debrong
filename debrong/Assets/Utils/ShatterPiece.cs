using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShatterPiece {

    public Vector2 center_point;
    public Vector2[] collider_points;

    public ShatterPiece(Vector2[] collider_points) {

        // Set basic defaults
        this.center_point = Vector2.zero;
        this.collider_points = collider_points;

        // Calculate center point for set of all points
        foreach (Vector2 point in collider_points) { this.center_point += point; }
        this.center_point = this.center_point / collider_points.Length;

        // Offset all the collider points
        for (int i = 0; i < collider_points.Length; i++) {
            this.collider_points[i] -= this.center_point;
        }
    }
}