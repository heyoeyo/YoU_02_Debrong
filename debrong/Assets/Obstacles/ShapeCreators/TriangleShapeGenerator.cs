using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(PolygonCollider2D))]
public class TriangleShapeGenerator : MonoBehaviour, IShapeGenerator, IShatterable {

    [SerializeField] private float scale = 1f;

    private const float area_factor = 3f;

    ShapeGen shape;


    // ----------------------------------------------------------------------------------------------------------------
    // Built-ins

    private void Awake() {
        shape = new ShapeGen(this.gameObject);
        GenerateShape(scale);
    }


    // ----------------------------------------------------------------------------------------------------------------

    public void GenerateShape(MinMaxFloat scale_range, MinMaxFloat jaggedness) {
        GenerateShape(scale_range.Random());
    }

    void GenerateShape(float scale) {

        // Don't generate collider points if we already have a triangular collider
        Vector2[] collider_points = this.shape.GetColliderPoints();
        bool has_3_points = (collider_points.Length == 3);
        if (!has_3_points) {
            collider_points = GenerateTriangularCollider();
        }

        // Create 'radial' triangulation
        this.shape.UpdateShape(collider_points);
    }

    Vector2[] GenerateTriangularCollider() {

        // Generate a simple triangular shape
        float base_fraction = 0.5f + Random.Range(-0.15f, 0.15f);
        float height_fraction = 1f - base_fraction;
        float area_scale = scale * area_factor;

        // For clarity, specify x/y components for 3 triangle points
        // Note these points are defined in a counter-clockwise order!
        Vector2 pt0 = new Vector2(0f, 0f);
        Vector2 pt1 = new Vector2(base_fraction * area_scale, 0f);
        Vector2 pt2 = new Vector2(pt1.x * Random.Range(0, 1f), height_fraction * area_scale);

        // Calculate triangle center offset, so shape is properly centered
        Vector2 center = (pt0 + pt1 + pt2) * (1f / 3f);
        pt0 -= center;
        pt1 -= center;
        pt2 -= center;

        // Rotate outer points to give random orientation of triangle
        float random_angle = Random.Range(0, 360f) * Mathf.Deg2Rad;
        pt0 = DataUtils.Rotate2D(pt0, random_angle);
        pt1 = DataUtils.Rotate2D(pt1, random_angle);
        pt2 = DataUtils.Rotate2D(pt2, random_angle);

        return new Vector2[] { pt0, pt1, pt2 };
    }

    public ShatterPiece[] Shatter() {
        return this.shape.ShatterToTriangles();
    }

}
