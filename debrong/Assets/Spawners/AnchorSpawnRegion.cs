using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorSpawnRegion : MonoBehaviour
{

    [SerializeField] private GameObject anchor_obstacle_prefab;

    [Header("Region Properties")]
    [SerializeField, Range(0, 100f)] private float region_width = 10f;
    [SerializeField, Range(0, 100f)] private float region_height = 10f;
    [SerializeField, Min(1)] private int num_obstacles = 1;

    [Header("Obstacle Shape")]
    [SerializeField, Range(0f, 1f)] private float obstacle_jaggedness = 0.1f;
    [SerializeField, Range(0.1f, 2f)] private float min_obstacle_scale = 0.25f;
    [SerializeField, Range(0.1f, 2f)] private float max_obstacle_scale = 1f;

    [Header("Anchor Properties")]
    [SerializeField] private float max_spawn_speed = 10f;
    [SerializeField] private float anchor_force = 500f;
    [SerializeField] private float anchor_distance_threshold = 7f;

    Spawner spawner;


    // ----------------------------------------------------------------------------------------------------------------
    // Built-in functions

    private void Awake() {
        BoxBounds spawn_region = new BoxBounds(this.transform.position, region_width, region_height);
        HandleInitialSpawn(spawn_region);
    }

    // ----------------------------------------------------------------------------------------------------------------

    void HandleInitialSpawn(BoxBounds spawn_region) {
        spawner = new Spawner(spawn_region, this.transform);
        GameObject[] new_spawns = spawner.SpawnManyObstacles(anchor_obstacle_prefab, num_obstacles);
        foreach (GameObject new_spawn in new_spawns) {
            SetupShape(new_spawn);
            SetupMotion(new_spawn);
        }
    }

    void SetupShape(GameObject new_spawn) {
        RadialShapeGenerator new_shape = new_spawn.GetComponent<RadialShapeGenerator>();

        MinMaxFloat scale_range = new MinMaxFloat(min_obstacle_scale, max_obstacle_scale);
        MinMaxFloat jagged_range = new MinMaxFloat(obstacle_jaggedness, obstacle_jaggedness);
        new_shape.GenerateShape(scale_range, jagged_range);
    }

    void SetupMotion(GameObject new_spawn) {
        AnchoredMotion new_motion = new_spawn.GetComponent<AnchoredMotion>();
        Vector2 anchor_point = new_spawn.transform.position;
        new_motion.SetAnchorPoint(anchor_point, this.anchor_force, this.anchor_distance_threshold);
        new_motion.SetRandomVelocity(this.max_spawn_speed);
    }


    // ----------------------------------------------------------------------------------------------------------------
    // Debugging

    private void OnDrawGizmos() {

        BoxBounds bounds = new BoxBounds(this.transform.position, region_width, region_height);
        bounds.DrawAsGizmo();
    }

    
}

