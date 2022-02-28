using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(PolygonCollider2D))]
public class RadialShapeGenerator : MonoBehaviour, IShapeGenerator, IShatterable {

    [SerializeField] private int num_min_points = 6;
    [SerializeField] private int num_max_points = 12;
    [SerializeField] private bool generate_shape_on_awake = false;

    ShapeGen shape;


    // ----------------------------------------------------------------------------------------------------------------
    // Built-ins

    private void Awake() {
        shape = new ShapeGen(this.gameObject);
        if (generate_shape_on_awake) {
            GenerateShape(new MinMaxFloat(0.7f, 1f), new MinMaxFloat(0, 1f));
        }
    }


    // ----------------------------------------------------------------------------------------------------------------
    // Public

    public void GenerateShape(MinMaxFloat scale_range, MinMaxFloat jaggedness_range) {

        float spawned_scale = scale_range.Random();
        float spawned_jaggedness = jaggedness_range.Random();
        Vector2[] collider_points = GenerateColliderPoints(spawned_scale, spawned_jaggedness);

        shape.UpdateShape(collider_points);
    }

    public ShatterPiece[] Shatter() {
        return this.shape.ShatterToTriangles();
    }

    // ----------------------------------------------------------------------------------------------------------------

    Vector2[] GenerateColliderPoints(float scale, float jaggedness) {

        // Adjust generation parameters based on inputs
        int num_points = Mathf.RoundToInt(Mathf.Lerp(num_min_points, num_max_points, jaggedness));
        float min_radius = Mathf.Lerp(1f, 0.25f, jaggedness);

        // Create shape by generating points along a set of circular segments
        // Note: angle step is negative, to ensure 'clockwise' sequence of points!
        float angle_step = -1 * 360f / num_points;
        Vector2[] collider_points = new Vector2[num_points];
        for (int i = 0; i < num_points; i++) {

            float angle_min = i * angle_step;
            float angle_max = (i + 1) * angle_step;
            float random_angle = Random.Range(angle_min, angle_max);
            float random_radius = scale * Random.Range(min_radius, 1);

            float point_x = random_radius * Mathf.Cos(Mathf.Deg2Rad * random_angle);
            float point_y = random_radius * Mathf.Sin(Mathf.Deg2Rad * random_angle);

            collider_points[i] = new Vector2(point_x, point_y);
        }

        return collider_points;
    }

}
