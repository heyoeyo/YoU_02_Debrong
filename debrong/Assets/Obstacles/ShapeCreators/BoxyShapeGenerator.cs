using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(PolygonCollider2D))]
public class BoxyShapeGenerator : MonoBehaviour, IShapeGenerator {

    ShapeGen shape;


    // ----------------------------------------------------------------------------------------------------------------
    // Built-ins

    private void Awake() {
        shape = new ShapeGen(this.gameObject);

        GenerateShape(new MinMaxFloat(0.7f, 1f), new MinMaxFloat(0.5f, 1f));
    }


    // ----------------------------------------------------------------------------------------------------------------
    // Public

    public void GenerateShape(MinMaxFloat scale_range, MinMaxFloat jaggedness_range) {

        float spawned_scale = scale_range.Random();
        float spawned_jaggedness = jaggedness_range.Random();
        Vector2[] collider_points = GenerateColliderPoints(spawned_scale, spawned_jaggedness);

        shape.UpdateShape(collider_points);
    }


    // ----------------------------------------------------------------------------------------------------------------

    Vector2[] GenerateColliderPoints(float scale, float jaggedness) {

        // Hard-code jagged square shape
        Vector2 pt0 = new Vector2(1, 0) * scale;
        Vector2 pt1 = new Vector2(0, -1) * scale;
        Vector2 pt2 = new Vector2(-1, 0) * scale;
        Vector2 pt3 = new Vector2(0, 1) * scale;
        Vector2[] diamond_pts = {pt0, pt1, pt2, pt3};

        // Controls which adjust beveling behavior
        MinMaxFloat corner_offset = new MinMaxFloat(0.05f, 0.12f);
        float bevel_threshold = Mathf.Lerp(0.9f, 0.1f, jaggedness);

        // Controls which adjust denting behavior
        MinMaxFloat mid_lerp_t = new MinMaxFloat(0.35f, 0.65f);
        MinMaxFloat dent_shrink = new MinMaxFloat(Mathf.Lerp(0.9f, 0.7f, jaggedness), 0.95f);

        // Loop over all corner points and generate following dent point along with potential beveling
        List<Vector2> collider_pts_list = new List<Vector2>();
        for (int i = 0; i < diamond_pts.Length; i++) {

            // For convenience
            Vector2 prev_corner = diamond_pts[(i - 1 + diamond_pts.Length) % diamond_pts.Length];
            Vector2 curr_corner = diamond_pts[i];
            Vector2 next_corner = diamond_pts[(i + 1) % diamond_pts.Length];

            // Random chance of generating a 'beveled' corner
            bool create_bevel = Random.Range(0, 1f) > bevel_threshold;
            if (create_bevel) {
                // Bevel by offsetting towards prev/next corner points
                Vector2 left_pt = Vector2.Lerp(curr_corner, prev_corner, corner_offset.Random());
                Vector2 right_pt = Vector2.Lerp(curr_corner, next_corner, corner_offset.Random());
                collider_pts_list.Add(left_pt);
                collider_pts_list.Add(right_pt);
            } else {
                collider_pts_list.Add(curr_corner);
            }

            // Generate 'dent' point by interpolating point between corners and scaling towards origin
            Vector2 dent_pt = Vector2.Lerp(curr_corner, next_corner, mid_lerp_t.Random()) * dent_shrink.Random();
            collider_pts_list.Add(dent_pt);

        }

        return collider_pts_list.ToArray();
    }
}
