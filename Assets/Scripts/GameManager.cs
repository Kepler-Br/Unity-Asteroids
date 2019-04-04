using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public 
public class GameManager : MonoBehaviour
{
    public enum GameState {StartState = 0, PlayingState, GameOverState};
    public GameState gameState;
    public int firstSpawnCount = 10;
    public float spawnNewAsteroidEvery = 3.0f;
    private float spawnNewAsteroidTimer;
    public GameObject asteroidPrefab;
    // Start is called before the first frame update

    void CreateAsteroidBig()
    {
        float degree = Random.Range(0.0f, Mathf.PI * 2.0f);
        float radius = 80.0f;
        Vector3 spawnPoint = new Vector3();
        spawnPoint.x = radius * Mathf.Cos(degree);
        spawnPoint.y = radius * Mathf.Sin(degree);

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPoint, Quaternion.identity);
        AsteroidScript asteroidScript = asteroid.GetComponent<AsteroidScript>();
        int timesToBeDestroyed = Random.Range(3, 4);
        const float maxRadius = 8.0f;
        const float minRadius = 5.0f;
        asteroidScript.Initialize(maxRadius, minRadius, 1);
    }

    void CreateAsteroidSmall()
    {
        float degree = Random.Range(0.0f, Mathf.PI * 2.0f);
        float radius = 80.0f;
        Vector3 spawnPoint = new Vector3();
        spawnPoint.x = radius * Mathf.Cos(degree);
        spawnPoint.y = radius * Mathf.Sin(degree);

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPoint, Quaternion.identity);
        AsteroidScript asteroidScript = asteroid.GetComponent<AsteroidScript>();
        int timesToBeDestroyed = Random.Range(1, 3);
        const float maxRadius = 3.0f;
        const float minRadius = 1.0f;
        asteroidScript.Initialize(maxRadius, minRadius, 1);
    }

    void Start()
    {
        spawnNewAsteroidTimer = spawnNewAsteroidEvery;
        for (int i = 0; i < firstSpawnCount; i++)
        {
            int choice = Random.Range(0, 2);
            if (choice == 0)
                CreateAsteroidBig();
            else
                CreateAsteroidSmall();
        }
    }

    // Update is called once per frame
    void Update()
    {
        spawnNewAsteroidTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (spawnNewAsteroidTimer < 0.0f)
        {
            int choice = Random.Range(0, 2);
            if (choice == 0)
                CreateAsteroidBig();
            else
                CreateAsteroidSmall();
            spawnNewAsteroidTimer = 3.0f;
        }
    }
}
