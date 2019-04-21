using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    RapidFire = 0,
    Rockets,
    SquareWeapon,
    Chaingun,
    Stinger,
    Shotgun,
    Lazer
}

public abstract class PlayerWeapon
{
    public GameObject fireParticle;
    public float reloadTime = 1.0f;
    public string bulletsFolderName = "Bullets/";
    public string soundFolderName = "Sound/";
    public string soundPrefabName = "";
    public string bulletPrefabName = "";
    public float bulletLifeTime = 2.0f;
    public float bulletForce = 1500.0f;

    public Transform firePlacePosition;
    public Transform playerPosition;
    public Rigidbody2D playerRigidBody;
    public GameObject bulletPrefab;
    public GameObject soundPrefab;


    public float reloadTimer = 0.0f;

    public abstract void Fire();

    public void Update()
    {
        if (reloadTimer >= 0.0f)
            reloadTimer -= Time.deltaTime;
    }

    public PlayerWeapon(GameObject player, GameObject firePlace, string bulletName, string soundName = "")
    {
        this.bulletPrefabName = bulletName;
        this.soundPrefabName = soundName.Length == 0 ? bulletName : soundName;
        this.firePlacePosition = firePlace.transform;
        this.fireParticle = UnityEngine.Resources.Load("FireParticles") as GameObject;
        this.soundPrefab = UnityEngine.Resources.Load(bulletsFolderName + soundFolderName + soundPrefabName) as GameObject;
        this.playerPosition = player.transform;
        this.playerRigidBody = player.GetComponent<Rigidbody2D>();

        this.bulletPrefab = UnityEngine.Resources.Load(bulletsFolderName + bulletPrefabName) as GameObject;

        if (this.bulletPrefab == null)
            Debug.LogError("Cannot load bullet prefab by name(null reference): " + bulletPrefabName);
    }

    public void PlaySound()
    {
        GameObject sound = GameObject.Instantiate(this.soundPrefab);
        GameObject.Destroy(sound, 5.0f);
    }

    public float GetNormalizedReloadTime()
    {
        return reloadTimer / reloadTime;
    }

    public void SpawnParticle(Vector3 position)
    {
        Quaternion particleRotation = Quaternion.Euler(playerPosition.rotation.eulerAngles.x,
                                                       playerPosition.rotation.eulerAngles.y,
                                                       playerPosition.rotation.eulerAngles.z);
        var particle = GameObject.Instantiate(fireParticle, position, particleRotation);
        GameObject.Destroy(particle, 5.0f);
    }
}

public class ShotgunWeapon : PlayerWeapon
{
    public ShotgunWeapon(GameObject player, GameObject firePlace) : base(player, firePlace, "SquareBullet", "ShotgunWeapon")
    {
        reloadTime = 1.0f;
        bulletLifeTime = 2.0f;
        bulletForce = 1500.0f;
        reloadTimer = 0.0f;
    }

    public override void Fire()
    {
        if (reloadTimer < 0.0f)
        {
            PlaySound();
            SpawnParticle(firePlacePosition.position);
            reloadTimer = reloadTime;
            const int bulletCount = 10;
            for (int i = 1; i < bulletCount; i++)
            {
                GameObject bullet = GameObject.Instantiate(bulletPrefab, firePlacePosition.position, playerPosition.rotation);
                GameObject.Destroy(bullet, bulletLifeTime);
                Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidBody.AddForce(GetBulletForce());
                playerRigidBody.AddForce(-playerPosition.up * bulletForce / 10.0f);
            }
        }
    }

    Vector3 GetBulletForce()
    {
        float bulletSlowdown = Random.Range(0.8f, 1.0f);
        Vector3 bulletDirection = GetRandomBulletDirection(Mathf.PI / 6.0f);
        return playerPosition.up + (Vector3)playerRigidBody.velocity +
               bulletDirection * bulletSlowdown;
    }

    Vector3 GetRandomBulletDirection(float dispersion)
    {
        const float bias = Mathf.PI / 2;
        float radius = bulletForce;
        float maxDegree = Mathf.Deg2Rad * playerPosition.rotation.eulerAngles.z + dispersion + bias;
        float minDegree = Mathf.Deg2Rad * playerPosition.rotation.eulerAngles.z - dispersion + bias;

        Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
        float degree = Random.Range(minDegree, maxDegree);
        direction.x = radius * Mathf.Cos(degree);
        direction.y = radius * Mathf.Sin(degree);
        return direction;
    }

    Vector3 GetBulletDirection(float dispersion, float currentCount, float totalCount)
    {
        const float bias = Mathf.PI / 2;
        float radius = bulletForce;
        float maxDegree = Mathf.Deg2Rad * playerPosition.rotation.eulerAngles.z + dispersion + bias;
        float minDegree = Mathf.Deg2Rad * playerPosition.rotation.eulerAngles.z - dispersion + bias;

        Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
        float degree = (maxDegree - minDegree) / totalCount * currentCount + minDegree;
        direction.x = radius * Mathf.Cos(degree);
        direction.y = radius * Mathf.Sin(degree);
        return direction;
    }
}

public class RapidFireWeapon : PlayerWeapon
{
    public RapidFireWeapon(GameObject player, GameObject firePlace) : base(player, firePlace, "RapidFireBullet")
    {
        reloadTime = 0.1f;
        bulletLifeTime = 2.0f;
        bulletForce = 100.0f;
    }

    public override void Fire()
    {
        if (reloadTimer < 0.0f)
        {
            PlaySound();
            reloadTimer = reloadTime;
            // SpawnParticle(bulletPosition);
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePlacePosition.position, playerPosition.rotation);
            GameObject.Destroy(bullet, bulletLifeTime);
            Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidBody.AddForce(playerPosition.up + (Vector3)playerRigidBody.velocity * 4.0f + GetRandomBulletDirection());
            playerRigidBody.AddForce(-playerPosition.up * bulletForce);
        }
    }

    Vector3 GetRandomBulletDirection()
    {
        const float bias = Mathf.PI / 2;
        const float dispersion = Mathf.PI / 32.0f;
        float radius = bulletForce;
        float maxDegree = Mathf.Deg2Rad * playerPosition.rotation.eulerAngles.z + dispersion + bias;
        float minDegree = Mathf.Deg2Rad * playerPosition.rotation.eulerAngles.z - dispersion + bias;

        Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
        float degree = Random.Range(minDegree, maxDegree);
        direction.x = radius * Mathf.Cos(degree);
        direction.y = radius * Mathf.Sin(degree);
        return direction;
    }
}

public class ChaingunWeapon : PlayerWeapon
{
    // If true - left weapon will be fired.
    //Otherwise - right.
    private bool isFiringFromLeftMuzzle = true;

    public ChaingunWeapon(GameObject player, GameObject firePlace) : base(player, firePlace, "SquareBullet", "ChaingunWeapon")
    {
        reloadTime = 0.1f;
        bulletLifeTime = 4.5f;
        bulletForce = 1000.0f;
    }

    public override void Fire()
    {
        if (reloadTimer < 0.0f)
        {
            PlaySound();
            reloadTimer = reloadTime;
            Vector3 muzzlePlace = playerPosition.right / 4.0f;
            if (isFiringFromLeftMuzzle)
                muzzlePlace = -muzzlePlace;
            isFiringFromLeftMuzzle = !isFiringFromLeftMuzzle;
            Vector3 bulletPosition = firePlacePosition.position + muzzlePlace;
            SpawnParticle(bulletPosition);
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletPosition, playerPosition.rotation);
            GameObject.Destroy(bullet, bulletLifeTime);
            Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidBody.AddForce(playerPosition.up * bulletForce + (Vector3)playerRigidBody.velocity);
            playerRigidBody.AddForce(-playerPosition.up * bulletForce);
        }
    }
}

public class SquareWeapon : PlayerWeapon
{
    public SquareWeapon(GameObject player, GameObject firePlace) : base(player, firePlace, "SquareBullet")
    {
        reloadTime = 0.3f;
        bulletLifeTime = 5.0f;
        bulletForce = 1000.0f;
    }

    public override void Fire()
    {
        if (reloadTimer < 0.0f)
        {
            reloadTimer = reloadTime;
            SpawnParticle(firePlacePosition.position);
            PlaySound();
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePlacePosition.position, playerPosition.rotation);
            GameObject.Destroy(bullet, bulletLifeTime);
            Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidBody.AddForce(playerPosition.up * bulletForce);
            playerRigidBody.AddForce(-playerPosition.up * bulletForce);
        }
    }
}

public class RocketWeapon : PlayerWeapon
{

    private bool isFiringFromLeftMuzzle = true;

    public RocketWeapon(GameObject player, GameObject firePlace) : base(player, firePlace, "RocketBullet")
    {
        reloadTime = 1.3f;
        bulletLifeTime = 5.0f;
        bulletForce = 1500.0f;
    }

    public override void Fire()
    {
        if (reloadTimer < 0.0f)
        {
            PlaySound();
            Vector3 muzzlePlace = playerPosition.right / 2.0f;
            if (isFiringFromLeftMuzzle)
                muzzlePlace = -muzzlePlace;
            isFiringFromLeftMuzzle = !isFiringFromLeftMuzzle;
            reloadTimer = reloadTime;

            Vector3 firePosition = playerPosition.position + muzzlePlace - playerPosition.up;
            SpawnParticle(firePosition);
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePosition, playerPosition.rotation);
            GameObject.Destroy(bullet, bulletLifeTime);
            Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidBody.AddForce(playerPosition.up * bulletForce);
            playerRigidBody.AddForce(-playerPosition.up * bulletForce);
        }
    }
}

public class StingerWeapon : PlayerWeapon
{
    public StingerWeapon(GameObject player, GameObject firePlace) : base(player, firePlace, "StingerBullet")
    {
        reloadTime = 1.0f;
        bulletLifeTime = 2.0f;
        bulletForce = 2000.0f;
    }

    public override void Fire()
    {
        if (reloadTimer < 0.0f)
        {
            PlaySound();
            reloadTimer = reloadTime;
            SpawnParticle(firePlacePosition.position);
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePlacePosition.position, playerPosition.rotation);
            GameObject.Destroy(bullet, bulletLifeTime);
            Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidBody.AddForce(playerPosition.up * bulletForce + (Vector3)playerRigidBody.velocity * 4.0f);
            playerRigidBody.AddForce(-playerPosition.up * bulletForce);


        }
    }
}

public class LazerWeapon : PlayerWeapon
{
    public LazerWeapon(GameObject player, GameObject firePlace) : base(player, firePlace, "LazerBullet", "LazerWeapon")
    {
        reloadTime = 3.5f;
        // bulletLifeTime = 20.0f;
        bulletForce = 0.0f;
    }

    public override void Fire()
    {
        if (reloadTimer < 0.0f)
        {
            PlaySound();
            reloadTimer = reloadTime;
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePlacePosition.position, playerPosition.rotation);
            bullet.transform.parent = this.firePlacePosition;
            // GameObject.Destroy(bullet, bulletLifeTime);
        }
    }
}