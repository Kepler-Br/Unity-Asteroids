using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPlayerWeapon
{
    void Fire();
    void Update();
}

public class RapidFireWeapon : IPlayerWeapon
{
    private Transform playerPosition;
    private Rigidbody2D playerRigidBody;
    private GameObject bulletPrefab;

    private const float reloadTime = 0.04f;
    private const string bulletPrefabName = "RapidFireBullet";
    private const float bulletLifeTime = 1.0f;
    private const float bulletForce = 50.0f;

    private float reloadTimer = 0.0f;


    public RapidFireWeapon(GameObject player)
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
            GameObject bullet = GameObject.Instantiate(bulletPrefab, playerPosition.position + playerPosition.up * 1.0f, playerPosition.rotation);
            GameObject.Destroy(bullet, bulletLifeTime);
            Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidBody.AddForce(playerPosition.up + (Vector3)playerRigidBody.velocity * 4.0f + GetRandomBulletDirection());
            playerRigidBody.AddForce(-playerPosition.up * bulletForce);


        }
    }

    Vector3 GetRandomBulletDirection()
    {
        const float bias = Mathf.PI / 2;
        const float dispersion = Mathf.PI / 8.0f;
        const float radius = bulletForce;
        float maxDegree = Mathf.Deg2Rad * playerPosition.rotation.eulerAngles.z + dispersion + bias;
        float minDegree = Mathf.Deg2Rad * playerPosition.rotation.eulerAngles.z - dispersion + bias;

        Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
        float degree = Random.Range(minDegree, maxDegree);
        direction.x = radius * Mathf.Cos(degree);
        direction.y = radius * Mathf.Sin(degree);
        return direction;
    }

    public void Update()
    {
        if (reloadTimer >= 0.0f)
            reloadTimer -= Time.deltaTime;
    }
}

public class SquareWeapon : IPlayerWeapon
{
    private Transform playerPosition;
    private Rigidbody2D playerRigidBody;
    private GameObject bulletPrefab;

    private const float reloadTime = 1.0f;
    private const string bulletPrefabName = "SquareBullet";
    private const float bulletLifeTime = 5.0f;
    private const float bulletForce = 1000.0f;

    private float reloadTimer = 0.0f;


    public SquareWeapon(GameObject player)
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

public class RocketWeapon : IPlayerWeapon
{
    private Transform playerPosition;
    private Rigidbody2D playerRigidBody;
    private GameObject bulletPrefab;

    private const float reloadTime = 1.3f;
    private const string bulletPrefabName = "RocketBullet";
    private const float bulletLifeTime = 5.0f;
    private const float bulletForce = 1500.0f;

    private float reloadTimer = 0.0f;


    public RocketWeapon(GameObject player)
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
    public float thrustForce = 50.0f;
    public float rotationSpeed = 3.0f;

    private GameManager gameManager;
    private Rigidbody2D playerRigidBody;
    private IPlayerWeapon currentWeapon;

    void Start()
    {
        currentWeapon = new RocketWeapon(this.gameObject);

        gameManager = GameObject.FindObjectOfType<GameManager>();
        playerRigidBody = this.GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Asteroid")
            gameManager.SendMessage("DeleteAsteroids");
    }

    void Update()
    {
        currentWeapon.Update();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0.0f, 0.0f, rotationSpeed);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0.0f, 0.0f, -rotationSpeed);

        if (Input.GetKey(KeyCode.W))
        {
            playerRigidBody.AddForce(this.transform.up * this.thrustForce);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerRigidBody.AddForce(-this.transform.up * this.thrustForce);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            currentWeapon.Fire();
        }
    }
}
