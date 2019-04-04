using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float fireTimer = 1.0f;
    public float thrustSpeed = 50.0f;
    public float rotationSpeed = 3.0f;
    public float bulletSpeed = 1000.0f;
    public float bulletLifeTime = 10.0f;
    public GameObject bulletPrefab;

    void Update()
    {
        if (fireTimer > 0.0f)
            fireTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0.0f, 0.0f, rotationSpeed);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0.0f, 0.0f, -rotationSpeed);

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(this.transform.up * this.thrustSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-this.transform.up * this.thrustSpeed);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (fireTimer < 0.0f)
            {
                fireTimer = 1.0f;
                GameObject bullet = Instantiate(bulletPrefab, this.transform.position + this.transform.up * 1, this.transform.rotation);
                Destroy(bullet, bulletLifeTime);
                Rigidbody2D rb2d = bullet.GetComponent<Rigidbody2D>();
                rb2d.AddForce(this.transform.up * bulletSpeed);
            }
        }
    }
}
