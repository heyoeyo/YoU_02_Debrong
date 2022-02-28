using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftSpawnRegion : MonoBehaviour
{

    [SerializeField] private GameObject drift_obstacle_prefab;

    [Header("Region Properties")]
    [SerializeField, Range(0, 100f)] private float region_width = 10f;
    [SerializeField, Range(0, 100f)] private const float region_height = 1f;
    [SerializeField] private float initial_spawn_delay = 4f;
    [SerializeField, Min(1f)] private float spawn_period = 3f;

    [Header("Obstacle Sizing")]
    [SerializeField, Range(0f, 1f)] private float obstacle_jaggedness = 0.1f;
    [SerializeField, Range(0.1f, 2f)] private float min_obstacle_scale = 0.75f;
    [SerializeField, Range(0.1f, 2f)] private float max_obstacle_scale = 1f;

    [Header("Drift Properties")]
    [SerializeField] private Vector2 drift_direction;
    [SerializeField] private float max_drift_speed = 3f;
    [SerializeField] private float drift_angle_variation = 30f;

    BoxBounds spawn_region;
    Spawner spawner;


    // ----------------------------------------------------------------------------------------------------------------
    // Built-in functions

    private void Awake() {

        this.spawn_region = new BoxBounds(this.transform.position, region_width, region_height);
        spawner = new Spawner(this.spawn_region, this.transform);

        StartCoroutine(PeriodicSpawns());

    }

    // ----------------------------------------------------------------------------------------------------------------

    IEnumerator PeriodicSpawns() {

        yield return new WaitForSeconds(initial_spawn_delay);

        while (true) {

            // Random wait
            float random_period = Random.Range(0.1f, 1f) * spawn_period;
            yield return new WaitForSeconds(random_period);

            // Random spawn count
            int num_to_spawn = Random.Range(0, 3);
            if (num_to_spawn > 0) {
                HandleSpawns(num_to_spawn);
            }
        }


    }

    void HandleSpawns(int num_to_spawn) {

        GameObject[] new_spawns = spawner.SpawnManyObstacles(drift_obstacle_prefab, num_to_spawn);
        foreach (GameObject each_spawn in new_spawns) {
            SetupShape(each_spawn);
            SetupMotion(each_spawn, drift_direction);
        }

    }

    void SetupShape(GameObject new_spawn) {
        BoxyShapeGenerator new_shape = new_spawn.GetComponent<BoxyShapeGenerator>();

        MinMaxFloat scale_range = new MinMaxFloat(min_obstacle_scale, max_obstacle_scale);
        MinMaxFloat jagged_range = new MinMaxFloat(obstacle_jaggedness, obstacle_jaggedness);
        new_shape.GenerateShape(scale_range, jagged_range);
    }

    void SetupMotion(GameObject new_spawn, Vector2 drift_direction) {
        DriftMotion new_motion = new_spawn.GetComponent<DriftMotion>();
        new_motion.SetRandomDrift(drift_direction, this.drift_angle_variation, this.max_drift_speed);
    }

    // ----------------------------------------------------------------------------------------------------------------
    // Debugging

    private void OnDrawGizmos() {
        BoxBounds spawn_bounds = new BoxBounds(this.transform.position, region_width, region_height);
        spawn_bounds.DrawAsGizmo();
    }

    
}

