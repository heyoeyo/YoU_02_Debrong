using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner {

    bool spawn_on_row;
    public BoxBounds spawn_region;
    Transform parent;


    // ----------------------------------------------------------------------------------------------------------------

    public Spawner(BoxBounds spawn_region, Transform parent) {
        this.spawn_region = spawn_region;
        this.parent = parent;

        // Spawn objects along the longer dimension, by default
        this.spawn_on_row = spawn_region.width > spawn_region.height;
    }


    // ----------------------------------------------------------------------------------------------------------------

    public void SetSpawnVertically() {
        this.spawn_on_row = false;
    }

    public void SetSpawnHorizontally() {
        this.spawn_on_row = true;
    }

    public GameObject[] SpawnManyObstacles(GameObject prefab_to_spawn, int num_spawn) {

        // Spawn obstacles in columns
        GameObject[] output = new GameObject[num_spawn];
        int store_idx = 0;
        BoxBounds[] spawn_boxes = spawn_on_row ? spawn_region.SubdivideX(num_spawn) : spawn_region.SubdivideY(num_spawn);
        foreach (BoxBounds spawn_box in spawn_boxes) {
            output[store_idx] = SpawnOneObstacle(prefab_to_spawn, spawn_box);
            store_idx++;
        }

        return output;
    }

    private GameObject SpawnOneObstacle(GameObject spawn_prefab, BoxBounds spawn_region) {

        // Set initial position based on bounds
        float spawn_x = spawn_region.RandomX();
        float spawn_y = spawn_region.RandomY();
        Vector2 spawn_pos = new Vector2(spawn_x, spawn_y);

        // Set initial random angle
        float spawn_angle = Random.Range(-180f, 180f);
        Quaternion spawn_ori = Quaternion.Euler(0, 0, spawn_angle);

        GameObject new_spawn = GameObject.Instantiate(spawn_prefab, spawn_pos, spawn_ori, this.parent);

        return new_spawn;
    }

}

