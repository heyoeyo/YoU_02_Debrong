using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IShatterShapeGenerator)), RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(MeshRenderer))]
public class ShardInitializer : MonoBehaviour
{
    // Other components
    PolygonCollider2D polycollider;
    Rigidbody2D rb;
    MeshRenderer render;
    IShatterShapeGenerator shatter_shape;

    // For convenience, used to partially randomize explosive effects
    MinMaxFloat force_randomizer = new MinMaxFloat(0.5f, 1.15f);

    void Awake()
    {
        this.polycollider = this.GetComponent<PolygonCollider2D>();
        this.rb = this.GetComponent<Rigidbody2D>();
        this.render = this.GetComponent<MeshRenderer>();
        this.shatter_shape = this.GetComponent<IShatterShapeGenerator>();
    }

    public void InitOnShatter(ShardSpec shard_spec, Vector2 shatter_force) {

        this.shatter_shape.GenerateShatterShape(shard_spec.piece);
        this.polycollider.density = shard_spec.density;
        this.render.sharedMaterial = shard_spec.material;

        this.rb.AddForce(shatter_force * force_randomizer.Random());
        this.rb.AddTorque(Random.Range(-1f, 1f) * this.rb.mass * shatter_force.magnitude * 0.5f);
    }
}

public struct ShardSpec {
    public ShatterPiece piece;
    public float density;
    public Material material;

    public ShardSpec(ShatterPiece piece, float density, Material material) {
        this.piece = piece;
        this.density = density;
        this.material = material;
    }
}
