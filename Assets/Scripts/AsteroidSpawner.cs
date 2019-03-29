using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AsteroidSpawner : MonoBehaviour
{
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
        for (int i = 0; i < 10; i++)
            CreateAsteroidBig();

        for (int i = 0; i < 10; i++)
            CreateAsteroidSmall();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
