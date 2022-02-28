using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PlayerChargingEventManager
{
    public static Action<float> ChargingEvent;
    public static Action<bool> StateChangeEvent;

    // .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .

    public static void TriggerChargingEvent(float charge_amount) {
        if(PlayerChargingEventManager.ChargingEvent != null) {
            PlayerChargingEventManager.ChargingEvent(charge_amount);
        }
    }

    public static void TriggerStateChange(bool is_charged) {
        if (PlayerChargingEventManager.StateChangeEvent != null) {
            PlayerChargingEventManager.StateChangeEvent(is_charged);
        }
    }

    // .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .

    public static void SubscribeChargingEvents(Action<float> charging_func) {
        PlayerChargingEventManager.ChargingEvent += charging_func;
    }

    public static void UnsubscribeChargingEvents(Action<float> charging_func) {
        PlayerChargingEventManager.ChargingEvent -= charging_func;
    }

    // .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .

    public static void SubscribeChangedEvents(Action<bool> state_change_func) {
        PlayerChargingEventManager.StateChangeEvent += state_change_func;
    }

    public static void UnsubscribeChangedEvents(Action<bool> state_change_func) {
        PlayerChargingEventManager.StateChangeEvent -= state_change_func;
    }
}
