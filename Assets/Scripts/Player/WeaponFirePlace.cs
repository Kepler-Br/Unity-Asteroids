using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFirePlace : MonoBehaviour
{
    // [SerializeField]
    // GameObject playerControllet;
    [SerializeField]
    GameObject player = null;
    PlayerController controllerScript = null;

    [SerializeField]
    public float weaponTimeout = 15.0f;
    private float weaponTimeoutTimer = 0.0f;
    private bool isCustomWeapon = false;
    private PlayerWeapon currentWeapon = null;


    void Awake()
    {
        currentWeapon = new LazerWeapon(this.player, this.gameObject);
        weaponTimeoutTimer = weaponTimeout;

        this.controllerScript = player.GetComponent<PlayerController>();
        this.controllerScript.Fire += OnFire;
        GameEvents.WeaponChanged += OnWeaponChange;
        GameEvents.PlayerDeath += OnPlayerDeath;
    }

    public void OnPlayerDeath()
    {
        SetWeapon(WeaponType.SquareWeapon);
    }

    public bool IsCustomWeapon()
    {
        return isCustomWeapon;
    }

    public float GetNormalizedReloadTime()
    {
        return this.currentWeapon.GetNormalizedReloadTime();
    }

    public float GetNormalizedWeaponTimeOut()
    {
        return this.weaponTimeoutTimer / this.weaponTimeout;
    }

    void OnFire()
    {
        this.currentWeapon.Fire();
    }

    void OnWeaponChange(WeaponType weapon)
    {
        this.SetWeapon(weapon);
    }

    void SetWeapon(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.Chaingun:
                currentWeapon = new ChaingunWeapon(this.player, this.gameObject);
                isCustomWeapon = true;
                weaponTimeoutTimer = weaponTimeout;
                break;
            case WeaponType.RapidFire:
                currentWeapon = new RapidFireWeapon(this.player, this.gameObject);
                isCustomWeapon = true;
                weaponTimeoutTimer = weaponTimeout;
                break;
            case WeaponType.Rockets:
                currentWeapon = new RocketWeapon(this.player, this.gameObject);
                isCustomWeapon = true;
                weaponTimeoutTimer = weaponTimeout;
                break;
            case WeaponType.SquareWeapon:
                currentWeapon = new SquareWeapon(this.player, this.gameObject);
                isCustomWeapon = false;
                weaponTimeoutTimer = weaponTimeout;
                break;
            case WeaponType.Stinger:
                currentWeapon = new StingerWeapon(this.player, this.gameObject);
                isCustomWeapon = true;
                weaponTimeoutTimer = weaponTimeout;
                break;
            case WeaponType.Shotgun:
                currentWeapon = new ShotgunWeapon(this.player, this.gameObject);
                isCustomWeapon = true;
                weaponTimeoutTimer = weaponTimeout;
                break;
            case WeaponType.Lazer:
                currentWeapon = new LazerWeapon(this.player, this.gameObject);
                isCustomWeapon = true;
                weaponTimeoutTimer = weaponTimeout;
                break;
        }
    }

    void Update()
    {
        if (isCustomWeapon)
        {
            weaponTimeoutTimer -= Time.deltaTime;
            if (weaponTimeoutTimer < 0.0f)
            {
                SetWeapon(WeaponType.SquareWeapon);
                isCustomWeapon = false;
            }
        }
        currentWeapon.Update();
    }
}
