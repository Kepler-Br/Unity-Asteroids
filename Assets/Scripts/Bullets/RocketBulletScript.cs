using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBulletScript : MonoBehaviour
{
    [SerializeField]
    GameObject sound = null;
    [SerializeField]
    GameObject splinterPrefab = null;
    public float damage = 100.0f;

    void Awake()
    {
        PlaySound();
    }

    void PlaySound()
    {
        var soundGameObject = Instantiate(sound);
        Destroy(soundGameObject, 2.0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Asteroid" || col.gameObject.tag == "Wall")
            OnDeath(col.gameObject);
    }

    void OnDeath(GameObject asteroid)
    {
        var damageReciever = asteroid.GetComponent<DamageReceiver>();
        damageReciever?.Damage(damage);

        SpawnSplinters(18);
        Destroy(this.gameObject);
    }

    void SpawnSplinters(int count)
    {
        float degreeStep = Mathf.PI / count;
        const float splinterSpeed = 2000.0f;
        const float splinterLifeTime = 3.0f;
        for (float degree = 0.0f; degree < Mathf.PI * 2.0f; degree += degreeStep)
        {
            Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
            direction.x = splinterSpeed * Mathf.Cos(degree);
            direction.y = splinterSpeed * Mathf.Sin(degree);
            degree += degreeStep;
            GameObject splinter = Instantiate(splinterPrefab, this.transform.position - this.transform.up, Quaternion.identity);
            Rigidbody2D splinterRigidBody = splinter.GetComponent<Rigidbody2D>();
            splinterRigidBody.AddForce(direction);
            Destroy(splinter, splinterLifeTime);
        }

    }
}
