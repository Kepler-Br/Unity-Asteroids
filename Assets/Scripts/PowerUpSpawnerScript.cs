using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnerScript : MonoBehaviour
{
    [SerializeField]
    float spawnPowerUpTime = 50.0f;
    float spawnPowerUpTimer = 0.0f;
    [SerializeField]
    bool spawnPowerups = true;

    [SerializeField]
    GameObject[] powerUps = null;

    [SerializeField]
    Vector2 powerupSpawnBoundBox;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.powerupSpawnBoundBox.x * 2.0f,
                                                      this.powerupSpawnBoundBox.y * 2.0f,
                                                      1.0f));
    }

    void Awake()
    {
        this.SetPosition();
        spawnPowerUpTimer = spawnPowerUpTime;
        GameEvents.GameOver += StopSpawn;
        GameEvents.PlayerDeath += StopSpawn;
        GameEvents.PlayerRespawn += StartSpawn;
        GameEvents.GameRestart += StartSpawn;
        GameEvents.GameStateChanged += OnGameStateChanged;
    }

    void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.StartState)
            spawnPowerups = false;
        if (gameState == GameState.GameOverState)
            spawnPowerups = false;
        if (gameState == GameState.PlayingState)
            spawnPowerups = true;
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
                SpawnPowerup();
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

    // void SpawnPowerUp()
    // {
    //     GameObject powerUp = Instantiate(this.GetRandomPowerUp(), this.transform.position, Quaternion.identity);
    //     const float destroyPowerUpTime = 18.0f;
    //     Destroy(powerUp, destroyPowerUpTime);
    // }

    void SpawnPowerup()
    {
        float x = Random.Range(-this.powerupSpawnBoundBox.x, this.powerupSpawnBoundBox.x);
        float y = Random.Range(-this.powerupSpawnBoundBox.y, this.powerupSpawnBoundBox.y);

        Instantiate(this.GetRandomPowerUp(), new Vector3(x, y, 0.0f), Quaternion.identity);
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
