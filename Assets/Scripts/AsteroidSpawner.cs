using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject asteroidPrefab = null;

    [SerializeField]
    Vector2 asteroidSpawnBoundBox;

    [SerializeField]
    bool isSpawningAsteroids = false;
    [SerializeField]
    int totalAsteroids = 0;
    [SerializeField]
    int asteroidDestroyed = 0;
    [SerializeField]
    int minAsteroidsOnScreen = 20;
    const int defaultAsteroidsToDestroyPerLevel = 100;

    [SerializeField]
    float spawnTimer = 0.0f;
    [SerializeField]
    float spawnTimerDefault = 1.0f;

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.asteroidSpawnBoundBox.x*2.0f, this.asteroidSpawnBoundBox.y*2.0f, 1.0f));
    }

    void Awake()
    {
        GameEvents.AsteroidDestroyed += OnAsteroidDestroyed;
        GameEvents.AsteroidCreated += OnNewAsteroidCreated;
        GameEvents.GameStateChanged += OnGameStateChange;
        GameEvents.ClearScreen += OnClearScreen;
        GameEvents.PlayerDeath += OnPlayerDeath;
    }

    void Update()
    {
        
        if (asteroidDestroyed >= defaultAsteroidsToDestroyPerLevel)
            isSpawningAsteroids = false;

        if (isSpawningAsteroids && totalAsteroids <= minAsteroidsOnScreen)
        {
            if (spawnTimer < 0.0f)
            {
                SpawnAsteroid();
                spawnTimer = spawnTimerDefault;
            }
            spawnTimer -= Time.deltaTime;
        }
    }

    void OnPlayerDeath()
    {
        // asteroidDestroyed += totalAsteroids;
        totalAsteroids = 0;
    }

    void OnClearScreen()
    {
        asteroidDestroyed += totalAsteroids;
        totalAsteroids = 0;
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

        float x = Random.Range(-this.asteroidSpawnBoundBox.x, this.asteroidSpawnBoundBox.x);
        float y = Random.Range(-this.asteroidSpawnBoundBox.y, this.asteroidSpawnBoundBox.y);

        GameObject asteroid = Instantiate(asteroidPrefab, new Vector3(x, y, 0.0f), Quaternion.identity);
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
