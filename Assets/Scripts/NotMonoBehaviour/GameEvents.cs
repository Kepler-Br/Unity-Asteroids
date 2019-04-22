using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents
{

    public static Action AsteroidCreated;
    public static Action<WeaponType> WeaponChanged;
    public static Action ClearScreen;
    public static Action GameOver;
    public static Action PlayerDeath;
    public static Action PlayerRespawn;
    public static Action GameRestart;
    public static Action HealthPickup;
    public static Action<int> AsteroidDestroyed;
    public static Action<GameState> GameStateChanged;
    public static Action PowerupPickup;

    public static void OnAsteroidCreated() => AsteroidCreated?.Invoke();
    public static void OnPowerupPickup() => PowerupPickup?.Invoke();
    public static void OnGameStateChanged(GameState gameState) => GameStateChanged?.Invoke(gameState);
    public static void OnClearScreen() => ClearScreen?.Invoke();
    public static void OnWeaponChanged(WeaponType weapon) => WeaponChanged?.Invoke(weapon);
    public static void OnHealthPickup() => HealthPickup?.Invoke();
    public static void OnGameOver() => GameOver?.Invoke();
    public static void OnPlayerDeath() => PlayerDeath?.Invoke();
    public static void OnPlayerRespawn() => PlayerRespawn?.Invoke();
    public static void OnGameRestart() => GameRestart?.Invoke();
    public static void OnAsteroidDestroyed(int score) => AsteroidDestroyed?.Invoke(score);
}
