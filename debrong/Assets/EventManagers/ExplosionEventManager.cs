using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ExplosionEventManager {

    public static Action<ExplosionParameters> ExplosionEvent;

    public static Vector2 CalculateExplosionForce(ExplosionParameters explosion_params, Vector2 recv_position) {

        // Get direction heading away from explosion center
        Vector2 away_vec = recv_position - explosion_params.position;
        Vector2 away_direction = away_vec.normalized;

        // Use 1/r^2 distance scaling (but don't allow r < 1)
        float distance = Mathf.Max(1f, away_vec.magnitude);
        float ex_scale = 15000f * (explosion_params.strength / (distance * distance));

        return away_direction * ex_scale;
    }

    public static Vector2 LimitForceFromExplosion(Vector2 explosion_force, float max_allowable_speed, float object_mass) {
        // Function used to alter explosive force so that the resulting object velocity is capped
        // calculated from: v = (F / m) * dt    | velocity, Force, mass, fixedDeltaTime
        // -> so: F(limited) = vmax * m / dt
        float max_allowable_force = max_allowable_speed * object_mass * (1 / Time.fixedDeltaTime);
        float force_mag = Mathf.Min(max_allowable_force, explosion_force.magnitude);
        return force_mag * explosion_force.normalized;
    }

    public static float SpeedFromForce(Vector2 explosion_force, float object_mass) {
        // From: F = ma, V = a * t  ->  So: V = (F/m) * (deltaT)
        return (explosion_force.magnitude / object_mass) * Time.fixedDeltaTime;
    }

}

public struct ExplosionParameters {

    public Vector2 position;
    public float strength;
    public float start_time;

    public ExplosionParameters(Vector2 position, float strength) {
        this.position = position;
        this.strength = strength;
        this.start_time = Time.time;
    }

    public void TriggerExplosionEvent() {
        if (ExplosionEventManager.ExplosionEvent != null) {
            ExplosionEventManager.ExplosionEvent(this);
        }
    }

    public static void TriggerExplosionEvent(Vector2 position, float strength) {
        ExplosionParameters new_params = new ExplosionParameters(position, strength);
        new_params.TriggerExplosionEvent();
    }

    public float DistanceFromExplosion(Vector2 position) {
        return (position - this.position).magnitude;
    }

}