using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameStateEventManager {

    public static Action StartEvent;
    public static Action ScoreEvent;

    // .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .

    public static void TriggerStartEvent() {
        Debug.Log("STARTING");
        if (GameStateEventManager.StartEvent != null) {
            GameStateEventManager.StartEvent();
        }
    }

    public static void TriggerScoreEvent() {
        if (GameStateEventManager.ScoreEvent != null) {
            GameStateEventManager.ScoreEvent();
        }
    }

    // .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .

    public static void SubscribePlayStart(Action playstart_func) {
        GameStateEventManager.StartEvent += playstart_func;
    }

    public static void UnsubscribePlayStart(Action playstart_func) {
        GameStateEventManager.StartEvent -= playstart_func;
    }

    // .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .

    public static void SubscribeScore(Action score_func) {
        GameStateEventManager.ScoreEvent += score_func;
    }

    public static void UnsubscribeScore(Action score_func) {
        GameStateEventManager.ScoreEvent -= score_func;
    }
}