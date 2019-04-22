using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject asteroidPrefab;

    [SerializeField]
    bool isSpawningAsteroids;

    int totalAsteroids = 0;
    int asteroidDestroyed = 0;
    const int defaultAsteroidsToDestroyPerLevel = 10;

    void Awake()
    {
        GameEvents.AsteroidDestroyed += OnAsteroidDestroyed;
        GameEvents.AsteroidCreated += OnNewAsteroidCreated;
        GameEvents.GameStateChanged += OnGameStateChange;
    }

    void Update()
    {
        // if (asteroidDestroyed >= defaultAsteroidsToDestroyPerLevel)
            // isSpawningAsteroids = false;
        if(isSpawningAsteroids && totalAsteroids <= 20)
            SpawnAsteroid();
    }

    void OnGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GameOverState:
                totalAsteroids = 0;
                asteroidDestroyed = 0;
                isSpawningAsteroids = false;
                break;
            case GameState.PlayingState:
                asteroidDestroyed = 0;
                totalAsteroids = 0;
                isSpawningAsteroids = true;
                break;
            case GameState.StartState:
                totalAsteroids = 0;
                asteroidDestroyed = 0;
                isSpawningAsteroids = false;
                break;
        }
    }

    void SpawnAsteroid()
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

    void OnNewAsteroidCreated()
    {
        totalAsteroids++;
    }

    void OnAsteroidDestroyed(int score)
    {
        totalAsteroids--;
        asteroidDestroyed++;
    }
}
