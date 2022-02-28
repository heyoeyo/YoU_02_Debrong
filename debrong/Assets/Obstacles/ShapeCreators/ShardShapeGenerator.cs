using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(PolygonCollider2D))]
public class ShardShapeGenerator : MonoBehaviour, IShatterShapeGenerator {

    ShapeGen shape;


    // ----------------------------------------------------------------------------------------------------------------
    // Built-ins

    private void Awake() {
        shape = new ShapeGen(this.gameObject);
    }


    // ----------------------------------------------------------------------------------------------------------------
    // Public

    public void GenerateShatterShape(ShatterPiece shatter_piece) {
        shape.UpdateShape(shatter_piece);
    }

}
