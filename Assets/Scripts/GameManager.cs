using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState { StartState = 0, PlayingState, GameOverState };

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameState gameState;
    bool isDestroyingAsteroids = false;

    void Awake()
    {
        // spawnNewAsteroidTimer = spawnNewAsteroidEvery;

        GameEvents.GameOver += OnGameOver;
        GameEvents.PlayerDeath += OnPlayerDeath;
        GameEvents.GameRestart += OnGameRestart;
        GameEvents.ClearScreen += ClearScreen;
        GameEvents.PowerupPickup += OnPowerupPickup;
    }

    void OnPowerupPickup()
    {
        // var sound = Instantiate(powerupPickupSound);
        // Destroy(sound, 3);
    }


    void OnPlayerDeath()
    {
        // spawnNewAsteroidTimer = spawnNewAsteroidEvery;
        // spawningAsteroids = false;
        ClearScreen();

    }

    void ClearScreen()
    {
        // GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        // for (int i = 0; i < asteroids.Length; i++)
            // Destroy(asteroids[i]);
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        for (int i = 0; i < bullets.Length; i++)
            Destroy(bullets[i]);
        GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
        for (int i = 0; i < powerups.Length; i++)
            Destroy(powerups[i]);
    }

    void OnGameOver()
    {
        ChangeGameState(GameState.GameOverState);
    }

    void OnGameRestart()
    {
        ChangeGameState(GameState.PlayingState);
    }

    void FixedUpdate()
    {
        ProcessInput();
    }

    void ChangeGameState(GameState gameState)
    {
        GameEvents.OnGameStateChanged(gameState);
        this.gameState = gameState;
    }

    void ProcessInput()
    {
        if (gameState == GameState.StartState && Input.GetKey(KeyCode.Return))
        {
            ChangeGameState(GameState.PlayingState);
        }
        if (gameState == GameState.GameOverState && Input.GetKey(KeyCode.Return))
        {
            ChangeGameState(GameState.PlayingState);
            GameEvents.OnGameRestart();
        }
    }
}
