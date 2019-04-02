using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AsteroidSpawner : MonoBehaviour
{
    public int firstSpawnCount = 10;
    public float spawnNewTimer = 3.0f;
    public GameObject asteroidPrefab;
    // Start is called before the first frame update

    void CreateAsteroidBig()
    {
        float degree = Random.Range(0.0f, Mathf.PI * 2.0f);
        float radius = 10.0f;
        Vector3 velocity = new Vector2();
        velocity.x = radius * Mathf.Cos(degree);
        velocity.y = radius * Mathf.Sin(degree);

        GameObject asteroid = Instantiate(asteroidPrefab, velocity, Quaternion.identity);
        AsteroidScript asteroidScript = asteroid.GetComponent<AsteroidScript>();
        asteroidScript.Initialize(1.0f, 0.5f, 1);
    }

    void CreateAsteroidSmall()
    {
        float degree = Random.Range(0.0f, Mathf.PI * 2.0f);
        float radius = 10.0f;
        Vector3 velocity = new Vector2();
        velocity.x = radius * Mathf.Cos(degree);
        velocity.y = radius * Mathf.Sin(degree);

        GameObject asteroid = Instantiate(asteroidPrefab, velocity, Quaternion.identity);
        AsteroidScript asteroidScript = asteroid.GetComponent<AsteroidScript>();
        asteroidScript.Initialize(0.5f, 0.3f, 1);
    }

    void Start()
    {
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
        spawnNewTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (spawnNewTimer < 0.0f)
        {
            int choice = Random.Range(0, 2);
            if (choice == 0)
                CreateAsteroidBig();
            else
                CreateAsteroidSmall();
            spawnNewTimer = 3.0f;
        }
    }
}
