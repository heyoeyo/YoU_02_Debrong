using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGen {

    public PolygonCollider2D source_collider;
    Mesh mesh;
    public Vector3[] vertices;
    public int[] triangles;

    // ----------------------------------------------------------------------------------------------------------------
    // Constructors

    public ShapeGen(GameObject source_obj) {
        this.mesh = new Mesh();
        source_obj.GetComponent<MeshFilter>().mesh = mesh;

        this.source_collider = source_obj.GetComponent<PolygonCollider2D>();
    }

    // ----------------------------------------------------------------------------------------------------------------
    // Public

    public void UpdateShape(Vector2[] collider_points) {
        Vector2[] clockwise_collider_points = ForceClockwiseColliderPoints(collider_points);
        Vector2 center_point = GetCenterFromColliderPoints(clockwise_collider_points);
        UpdatePolygonCollider(clockwise_collider_points);
        GenerateRadialVertexData(clockwise_collider_points, center_point);
        UpdateMeshData();
    }

    // . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .

    public void UpdateShape(Vector2[] collider_points, Vector2 center_vertex_position) {
        Vector2[] clockwise_collider_points = ForceClockwiseColliderPoints(collider_points);
        UpdatePolygonCollider(clockwise_collider_points);
        GenerateRadialVertexData(clockwise_collider_points, center_vertex_position);
        UpdateMeshData();
    }

    // . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .

    public void UpdateShape(ShatterPiece shatter_piece) {
        UpdatePolygonCollider(shatter_piece.collider_points);
        GenerateRadialVertexData(shatter_piece.collider_points, shatter_piece.center_point);
        UpdateMeshData();
    }

    // . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .

    public void UpdateShape(Vector2[] collider_points, Vector3[] vertices, int[] triangles) {
        UpdatePolygonCollider(collider_points);
        SetVertexData(vertices, triangles);
        UpdateMeshData();
    }

    public Vector2[] GetColliderPoints() {
        return this.source_collider.GetPath(0);
    }

    public BoxBounds GetBounds() {
        Vector2 min_xy = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max_xy = new Vector2(float.MinValue, float.MinValue);
        foreach(Vector2 vertex in GetColliderPoints()) {
            min_xy = Vector2.Min(min_xy, vertex);
            max_xy = Vector2.Max(max_xy, vertex);
        }
        return new BoxBounds(min_xy, max_xy);
    }

    // . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .

    public ShatterPiece[] ShatterToTriangles() {

        // Shape data is organized as follows:
        // verts = [(x,y), (x,y), (x,y), ...]
        // tris = [0,1,2, 0,3,4, 0,5,6, 0,7,8, ...]
        // -> Want to grab [vert0, vert1, ver2] + [vert0, vert3, vert4] + [vert0, vert5, ver6] + ... etc

        // Create a new 'shatter piece' from each triangle we have defined
        int num_tris = triangles.Length / 3;
        List<ShatterPiece> pieces = new List<ShatterPiece>();
        for (int i = 0; i < num_tris; i++) {
            int tri_idx_offset = 3 * i;
            Vector2[] new_collider_pts = {
                this.vertices[triangles[0 + tri_idx_offset]],
                this.vertices[triangles[1 + tri_idx_offset]],
                this.vertices[triangles[2 + tri_idx_offset]]
            };

            pieces.Add(new ShatterPiece(new_collider_pts));
        }

        return pieces.ToArray();
    }


    // ----------------------------------------------------------------------------------------------------------------
    // Private

    Vector2 GetCenterFromColliderPoints(Vector2[] collider_points) {
        Vector2 center = Vector2.zero;
        foreach(Vector2 point in collider_points) {
            center += point;
        }
        center = center * (1f / (float)collider_points.Length);

        return center;
    }

    void GenerateRadialVertexData(Vector2[] clockwise_collider_points, Vector2 center_vertex_position) {

        // For clarity
        int num_outer_points = clockwise_collider_points.Length;
        int num_vertices = num_outer_points + 1;
        int last_vert_idx = num_vertices - 1;
        int num_triangles = 3 * num_outer_points;

        // Setup vertices to use center point + collider points
        vertices = new Vector3[num_outer_points + 1];
        for (int i = 0; i < num_outer_points; i++) {
            vertices[i] = clockwise_collider_points[i];
        }

        // Store 'center point' as last entry
        vertices[last_vert_idx] = center_vertex_position;

        // Create triangles by 'fanning' out along collider path from center point
        // NOTE: points have to be connected clockwise to render properly (i.e. facing the camera)
        triangles = new int[num_triangles];
        for (int i = 0; i < num_outer_points; i++) {
            int tri_idx_offset = 3 * i;
            int tri_idx0 = 0 + tri_idx_offset;
            int tri_idx1 = 1 + tri_idx_offset;
            int tri_idx2 = 2 + tri_idx_offset;

            // Store triangle vertex sequence
            triangles[tri_idx0] = last_vert_idx;
            triangles[tri_idx1] = i;
            triangles[tri_idx2] = ((i + 1) % num_outer_points);
        }
    }

    Vector2[] ForceClockwiseColliderPoints(Vector2[] collider_points) {

        // Find center of collider points, assuming 'radial' arrangment of points
        Vector2 center = GetCenterFromColliderPoints(collider_points);

        // Check radial angle between 'point 0' and 'point 1' to see if it is clockwise or not
        Vector2 pt0 = collider_points[0] - center;
        Vector2 pt1 = collider_points[1] - center;

        // Since angles increase in CCW direction, points are clockwise if angle difference between 0 -> 1 is negative!
        bool is_clockwise = (Vector2.SignedAngle(pt0, pt1) < 0);

        // Reverse collider point order if they were found to be counter-clockwise
        Vector2[] clockwise_collider_points = new Vector2[collider_points.Length];
        if (is_clockwise) {
            clockwise_collider_points = collider_points;
        } else {
            for (int i = 0; i < collider_points.Length; i++) {
                clockwise_collider_points[i] = collider_points[collider_points.Length - i - 1];
            }
        }

        return clockwise_collider_points;
    }

    void SetVertexData(Vector3[] vertices, int[] triangles) {
        this.vertices = vertices;
        this.triangles = triangles;
    }

    void UpdatePolygonCollider(Vector2[] collider_points) {
        this.source_collider.pathCount = 1;
        this.source_collider.SetPath(0, collider_points);
    }

    void UpdateMeshData() {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}