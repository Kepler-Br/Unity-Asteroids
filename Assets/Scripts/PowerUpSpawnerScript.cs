using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnerScript : MonoBehaviour
{
    public float spawnPowerUpTime = 50.0f;
    public float spawnPowerUpTimer = 0.0f;
    public bool spawnPowerups = true;

    private GameObject[] powerUps;
    // Start is called before the first frame update
    void Awake()
    {
        this.SetPosition();
        spawnPowerUpTimer = spawnPowerUpTime;
        powerUps = new GameObject[] { UnityEngine.Resources.Load("Powerups/RandomWeapon") as GameObject,
                                      UnityEngine.Resources.Load("Powerups/RocketWeapon") as GameObject,
                                    //   UnityEngine.Resources.Load("Powerups/SquareWeapon") as GameObject,
                                      UnityEngine.Resources.Load("Powerups/RapidWeapon") as GameObject,
                                      UnityEngine.Resources.Load("Powerups/ChaingunWeapon") as GameObject,
                                      UnityEngine.Resources.Load("Powerups/ClearScreenPowerUp") as GameObject,
                                      UnityEngine.Resources.Load("Powerups/ShotgunPowerUp") as GameObject, };
        GameEvents.GameOver += StopSpawn;
        GameEvents.PlayerDeath += StopSpawn;
        GameEvents.PlayerRespawn += StartSpawn;
        GameEvents.GameRestart += StartSpawn;

    }
    void SetPosition()
    {
        var cam = Camera.main;

        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        float screenHeight = screenTopRight.y - screenBottomLeft.y;
        const float powerUpSize = 4.0f;
        this.transform.position = new Vector3(-screenHeight - powerUpSize, 0.0f, 0.0f);
    }

    void Update()
    {
        if (spawnPowerups)
        {
            spawnPowerUpTimer -= Time.deltaTime;
            if (spawnPowerUpTimer < 0.0f)
            {
                SpawnPowerUp();
                spawnPowerUpTimer = spawnPowerUpTime;
            }
        }
    }

    GameObject GetRandomPowerUp()
    {
        int powerUpsCount = powerUps.Length;
        int powerUpIndex = Random.Range(0, powerUpsCount);
        return this.powerUps[powerUpIndex];
    }

    void SpawnPowerUp()
    {
        GameObject powerUp = Instantiate(this.GetRandomPowerUp(), this.transform.position, Quaternion.identity);
        const float destroyPowerUpTime = 18.0f;
        Destroy(powerUp, destroyPowerUpTime);
    }

    void StopSpawn()
    {
        spawnPowerUpTimer = spawnPowerUpTime;
        spawnPowerups = false;
    }

    void StartSpawn()
    {
        spawnPowerUpTimer = spawnPowerUpTime;
        spawnPowerups = true;
    }
}
