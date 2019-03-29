using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AsteroidSpawner : MonoBehaviour
{
    public int spawnTotalAsteroids = 10;
    public GameObject asteroidPrefab;
    // Start is called before the first frame update

    void CreateAsteroidBig()
    {
        GameObject asteroid = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
        AsteroidScript asteroidScript = asteroid.GetComponent<AsteroidScript>();
        asteroidScript.Initialize(1.0f, 0.5f, 1);
    }

    void CreateAsteroidSmall()
    {
        GameObject asteroid = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);
        AsteroidScript asteroidScript = asteroid.GetComponent<AsteroidScript>();
        asteroidScript.Initialize(0.5f, 0.3f, 1);
    }

    void Start()
    {
        for (int i = 0; i < spawnTotalAsteroids; i++)
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

    }
}
