using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum GameState { StartState = 0, PlayingState, GameOverState };
    public GameState gameState;
    public int firstSpawnCount = 10;
    public float spawnNewAsteroidEvery = 3.0f;
    public GameObject asteroidPrefab;
    public GameObject powerUpSpawner;
    public DigitDisplayScript digitDisplayScript;

    private float spawnNewAsteroidTimer;
    private float screenHeight;
    private float screenWidth;


    void Start()
    {
        digitDisplayScript = GameObject.FindObjectOfType<DigitDisplayScript>();
        spawnNewAsteroidTimer = spawnNewAsteroidEvery;
        for (int i = 0; i < firstSpawnCount; i++)
        {
            CreateAsteroid();
        }
    }

    void ClearPlayField()
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
        gameState = GameState.GameOverState;
        powerUpSpawner.SendMessage("OnGameOver");
    }

    void OnPlayerRestart()
    {
        gameState = GameState.PlayingState;
        spawnNewAsteroidTimer = spawnNewAsteroidEvery;
        powerUpSpawner.SendMessage("OnPlayerReplay");
        SpawnStartAsteroids();
    }

    void OnPlayerRespawn()
    {
        powerUpSpawner.SendMessage("OnPlayerRespawn");
        spawnNewAsteroidTimer = spawnNewAsteroidEvery;
        SpawnStartAsteroids();
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
        float degree = Random.Range(0.0f, Mathf.PI * 2.0f);
        float radius = 80.0f;
        Vector3 spawnPoint = new Vector3();
        spawnPoint.x = radius * Mathf.Cos(degree);
        spawnPoint.y = radius * Mathf.Sin(degree);

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPoint, Quaternion.identity);
        AsteroidScript asteroidScript = asteroid.GetComponent<AsteroidScript>();
        const float maxRadius = 9.0f;
        const float minRadius = 3.0f;
        float asteroidRadius = Random.Range(minRadius, maxRadius);
        asteroidScript.Initialize(asteroidRadius);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.PlayingState)
            spawnNewAsteroidTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (gameState == GameState.PlayingState)
        {
            if (spawnNewAsteroidTimer < 0.0f)
            {
                CreateAsteroid();
                spawnNewAsteroidTimer = 3.0f;
            }
        }
    }
}
