using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerashake : MonoBehaviour
{
    Vector3 originalPosition;

    public float shakeTimer = 0.0f;

    const float maxShakeTimer = 2.0f;

    void OnAsteroidDestroyed(int score)
    {
        shakeTimer += score / 5;
        shakeTimer = shakeTimer > maxShakeTimer ? maxShakeTimer : shakeTimer;
    }

    void OnPlayerDeath()
    {
        shakeTimer = maxShakeTimer;
    }

    // Start is called before the first frame update
    void Awake()
    {
        GameEvents.AsteroidDestroyed += OnAsteroidDestroyed;
        GameEvents.PlayerDeath += OnPlayerDeath;
        

    }

    float NormalizedShakeTime()
    {
        return shakeTimer / maxShakeTimer;
    }

    void Shake()
    {
        originalPosition = this.transform.position;
        Vector2 t = Random.insideUnitCircle * NormalizedShakeTime()/10.0f;
        float x = t.x + originalPosition.x;
        float y = t.y + originalPosition.y;
        float z = originalPosition.z;
        this.transform.position = new Vector3(x, y, z);
        shakeTimer -= Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0.0f)
            Shake();


    }
}
