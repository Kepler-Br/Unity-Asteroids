using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBulletScript : MonoBehaviour
{
    public float damage = 100.0f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
            OnDeath(collision);
    }

    void OnDeath(Collision2D collision)
    {
        collision.gameObject.SendMessage("Damage", damage);
        SpawnSplinters(18);
        Destroy(this.gameObject);
    }

    void SpawnSplinters(int count)
    {
        GameObject splinterPrefab = UnityEngine.Resources.Load("SquareBullet") as GameObject;
        float degreeStep = Mathf.PI / count;
        const float splinterSpeed = 2000.0f;
        const float splinterLifeTime = 1.0f;
        for (float degree = 0.0f; degree < Mathf.PI * 2.0f; degree += degreeStep)
        {
            Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
            direction.x = splinterSpeed * Mathf.Cos(degree);
            direction.y = splinterSpeed * Mathf.Sin(degree);
            degree += degreeStep;
            GameObject splinter = Instantiate(splinterPrefab, this.transform.position, Quaternion.identity);
            Rigidbody2D splinterRigidBody = splinter.GetComponent<Rigidbody2D>();
            splinterRigidBody.AddForce(direction);
            Destroy(splinter, splinterLifeTime);
        }

    }
}
