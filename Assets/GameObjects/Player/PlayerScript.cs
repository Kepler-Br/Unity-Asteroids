using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBulletType
{
    void Fire();
    void Update();
}

public class SquareBullet : IBulletType
{
    private Transform playerPosition;
    private Rigidbody2D playerRigidBody;
    private GameObject bulletPrefab;

    private const float reloadTime = 1.0f;
    private const string bulletPrefabName = "Bullet";
    private const float bulletLifeTime = 5.0f;
    private const float bulletForce = 10.0f;

    private float reloadTimer = 0.0f;


    SquareBullet(GameObject player)
    {
        this.playerPosition = player.transform;
        this.playerRigidBody = player.GetComponent<Rigidbody2D>();
        this.bulletPrefab = UnityEngine.Resources.Load(bulletPrefabName) as GameObject;
        if (this.bulletPrefab == null)
            Debug.LogError("Cannot load bullet prefab by name(null reference): " + bulletPrefabName);
    }

    public void Fire()
    {
        if (reloadTimer < 0.0f)
        {
            reloadTimer = reloadTime;
            GameObject bullet = GameObject.Instantiate(bulletPrefab, playerPosition.position + playerPosition.up * 1, playerPosition.rotation);
            GameObject.Destroy(bullet, bulletLifeTime);
            Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidBody.AddForce(playerPosition.up * bulletForce);
            playerRigidBody.AddForce(-playerPosition.up * bulletForce);
        }
    }

    public void Update()
    {
        if (reloadTimer >= 0.0f)
            reloadTimer -= Time.deltaTime;
    }
}

public class PlayerScript : MonoBehaviour
{
    public float reloadTime = 1.0f;
    public float thrustForce = 50.0f;
    public float rotationSpeed = 3.0f;
    public float bulletForce = 1000.0f;
    public float bulletLifeTime = 10.0f;
    public GameObject bulletPrefab;
    private float reloadTimer = 0.0f;
    private GameManager gameManager;
    private Rigidbody2D rigidbody2D;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "Asteroid")
        {

            gameManager.SendMessage("DeleteAsteroids");
        }
    }

    void Update()
    {
        if (reloadTimer >= 0.0f)
            reloadTimer -= Time.deltaTime;
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
            rb.AddForce(this.transform.up * this.thrustForce);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-this.transform.up * this.thrustForce);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (reloadTimer < 0.0f)
            {
                reloadTimer = reloadTime;
                GameObject bullet = Instantiate(bulletPrefab, this.transform.position + this.transform.up * 1, this.transform.rotation);
                Destroy(bullet, bulletLifeTime);
                Rigidbody2D rb2d = bullet.GetComponent<Rigidbody2D>();
                rb2d.AddForce(this.transform.up * bulletForce);
                rigidbody2D.AddForce(-this.transform.up * bulletForce);
            }
        }
    }
}
