using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(PolygonCollider2D))]
public class PlayerShape : MonoBehaviour
{

    [SerializeField] private Vector2 scale = new Vector2(0.5f, 4f);


    ShapeGen shape;
    private enum ColliderShape { Flat, Angled, Curved }


    private void Awake() {
        shape = new ShapeGen(this.gameObject);
        SelectMesh();
    }

    void SelectMesh() {

        // Pick random shape
        int num_enums = System.Enum.GetNames(typeof(ColliderShape)).Length;
        ColliderShape shape_select = (ColliderShape) Random.Range(1, num_enums);

        if (shape_select == ColliderShape.Flat) {
            GenerateFlatMesh();
        } else if (shape_select == ColliderShape.Angled) {
            GenerateAngledMesh();
        } else if (shape_select == ColliderShape.Curved) {
            GenerateCurvedMesh();
        } else {
            GenerateFlatMesh();
            Debug.LogErrorFormat("Couldn't figure out player shape! Got: {0}", shape_select);
        }

    }

    public BoxBounds GetShapeBounds() {
        return this.shape.GetBounds();
    }

    void GenerateFlatMesh() {

        // For clarity
        float top = scale.y, bot = -scale.y;
        float left = -scale.x / 2f, right = 0;

        // Hard-code vertices for flat (e.g. rectangular) player shape
        Vector2[] collider_points = new Vector2[] {
            new Vector2(right, top),
            new Vector2(right, bot),
            new Vector2(left, bot),
            new Vector2(left, top)
        };

        // Hard-code 2 triangles to make flat shape
        Vector3[] vertices = DataUtils.V2ToV3Array(collider_points);
        int[] triangles = new int[] {0, 1, 2, 0, 2, 3};

        shape.UpdateShape(collider_points, vertices, triangles);
    }

    void GenerateAngledMesh() {

        // For clarity
        float thick = scale.x / 1.5f;
        float left_near = -scale.x * 0.5f, left_far = (left_near - thick), right = 0;
        float top = scale.y, middle = 0, bot = -scale.y;

        // Hard-code vertices for flat (e.g. rectangular) player shape
        Vector2[] collider_points = new Vector2[] {
            new Vector2(right, middle),
            new Vector2(left_near, bot),
            new Vector2(left_far, bot),
            new Vector2(left_far, top),
            new Vector2(left_near, top),
        };

        // Hard-code single triangle + box shape to make angled shape
        Vector3[] vertices = DataUtils.V2ToV3Array(collider_points);
        int[] triangles = new int[] {0, 1, 2, 0, 2, 3, 0, 3, 4};

        shape.UpdateShape(collider_points, vertices, triangles);
    }

    void GenerateCurvedMesh() {

        // For clarity
        int num_collider_points = 10;
        float h_radius = scale.y * 0.6f;
        float v_radius = scale.y;
        Vector2 center = new Vector2(-h_radius, 0);

        // Calculate circular co-ords. in clockwise direction (for easier triangulation)
        Vector2[] collider_points = new Vector2[num_collider_points];
        float angle_start = 90;
        float angle_end = -angle_start;
        float angle_step = (angle_start - angle_end) / Mathf.Max(1f, (num_collider_points - 1));
        for (int i = 0; i < num_collider_points; i++) {
            float angle_rad = (angle_start - (i * angle_step)) * Mathf.Deg2Rad;
            float x_pt = center.x + h_radius * Mathf.Cos(angle_rad);
            float y_pt = center.y + v_radius * Mathf.Sin(angle_rad);
            collider_points[i] = new Vector2(x_pt, y_pt);
        }

        // Use built-in radial mesh creation
        shape.UpdateShape(collider_points, center);
    }
}
