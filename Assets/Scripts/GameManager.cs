using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState { StartState = 0, PlayingState, GameOverState };

public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public int firstSpawnCount = 10;
    public float spawnNewAsteroidEvery = 0.1f;
    public GameObject asteroidPrefab;
    public GameObject powerupPickupSound;

    private float spawnNewAsteroidTimer;
    private float screenHeight;
    private float screenWidth;
    private bool spawningAsteroids = true;


    void Awake()
    {
        spawnNewAsteroidTimer = spawnNewAsteroidEvery;

        GameEvents.GameOver += OnGameOver;
        GameEvents.PlayerDeath += OnPlayerDeath;
        GameEvents.PlayerRespawn += OnPlayerRespawn;
        GameEvents.GameRestart += OnGameRestart;
        GameEvents.ClearScreen += ClearScreen;
        GameEvents.PowerupPickup += OnPowerupPickup;
    }

    void OnPowerupPickup()
    {
        var sound = Instantiate(powerupPickupSound);
        Destroy(sound, 3);
    }


    void OnPlayerDeath()
    {
        spawnNewAsteroidTimer = spawnNewAsteroidEvery;
        spawningAsteroids = false;
        ClearScreen();

    }

    void ClearScreen()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        for (int i = 0; i < asteroids.Length; i++)
            Destroy(asteroids[i]);
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        for (int i = 0; i < bullets.Length; i++)
            Destroy(bullets[i]);
        GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
        for (int i = 0; i < powerups.Length; i++)
            Destroy(powerups[i]);
    }

    void OnGameOver()
    {
        spawnNewAsteroidTimer = spawnNewAsteroidEvery;
        ChangeGameState(GameState.GameOverState);
    }

    void OnGameRestart()
    {
        spawningAsteroids = true;
        gameState = GameState.PlayingState;
        spawnNewAsteroidTimer = spawnNewAsteroidEvery;
        // SpawnStartAsteroids();
    }

    void OnPlayerRespawn()
    {
        spawningAsteroids = true;
        spawnNewAsteroidTimer = spawnNewAsteroidEvery;
        // SpawnStartAsteroids();
    }

    void SpawnStartAsteroids()
    {
        for (int i = 0; i < firstSpawnCount; i++)
        {
            CreateAsteroid();
        }
    }

    void CreateAsteroid()
    {
        float degree = UnityEngine.Random.Range(0.0f, Mathf.PI * 2.0f);
        float radius = 80.0f;
        Vector3 spawnPoint = new Vector3();
        spawnPoint.x = radius * Mathf.Cos(degree);
        spawnPoint.y = radius * Mathf.Sin(degree);

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPoint, Quaternion.identity);
        AsteroidScript asteroidScript = asteroid.GetComponent<AsteroidScript>();
        const float maxRadius = 5.0f;
        const float minRadius = 2.0f;
        float asteroidRadius = UnityEngine.Random.Range(minRadius, maxRadius);
        asteroidScript.Initialize(asteroidRadius);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.PlayingState && spawningAsteroids)
            spawnNewAsteroidTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (gameState == GameState.PlayingState)
        {
            if (spawnNewAsteroidTimer < 0.0f)
            {
                CreateAsteroid();
                spawnNewAsteroidTimer = spawnNewAsteroidEvery;
            }
        }
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
