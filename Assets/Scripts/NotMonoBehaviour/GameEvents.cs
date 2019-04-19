using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents
{

    public static Action GameOver;
    public static Action PlayerDeath;
    public static Action PlayerRespawn;
    public static Action GameRestart;
    public static Action HealthPickup;
    public static Action<int> AsteroidDestroyed;
    

    public static void OnHealthPickup() => HealthPickup?.Invoke();
    public static void OnGameOver() => GameOver?.Invoke();
    public static void OnPlayerDeath() => PlayerDeath?.Invoke();
    public static void OnPlayerRespawn() => PlayerRespawn?.Invoke();
    public static void OnGameRestart() => GameRestart?.Invoke();
    public static void OnAsteroidDestroyed(int score) => AsteroidDestroyed?.Invoke(score);
}
