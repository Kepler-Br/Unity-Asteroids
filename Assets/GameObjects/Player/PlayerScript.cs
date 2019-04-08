using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float thrustForce = 80.0f;
    public float rotationSpeed = 3.0f;
    public float weaponTimeout = 15.0f;

    private GameManager gameManager;
    private Rigidbody2D playerRigidBody;
    private IPlayerWeapon currentWeapon;
    private Material material;
    private float weaponTimeoutTimer = 0.0f;
    private bool isCustomWeapon = false;
    private int materialColorID;

    void Start()
    {
        currentWeapon = new RocketWeapon(this.gameObject);
        gameManager = GameObject.FindObjectOfType<GameManager>();
        playerRigidBody = this.GetComponent<Rigidbody2D>();
        material = GetComponent<Renderer>().material;

        weaponTimeoutTimer = weaponTimeout;
        materialColorID = Shader.PropertyToID("_MainColor");
    }

    void SetWeapon(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.Chaingun:
                currentWeapon = new ChaingunWeapon(this.gameObject);
                isCustomWeapon = true;
                weaponTimeoutTimer = weaponTimeout;
                break;
            case WeaponType.RapidFire:
                currentWeapon = new RapidFireWeapon(this.gameObject);
                isCustomWeapon = true;
                weaponTimeoutTimer = weaponTimeout;
                break;
            case WeaponType.Rockets:
                currentWeapon = new RocketWeapon(this.gameObject);
                isCustomWeapon = true;
                weaponTimeoutTimer = weaponTimeout;
                break;
            case WeaponType.SquareWeapon:
                currentWeapon = new SquareWeapon(this.gameObject);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Asteroid")
            gameManager.SendMessage("DeleteAsteroids");
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
        UpdatePlayerColor();
        currentWeapon.Update();
    }

    void UpdatePlayerColor()
    {
        Color resultColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if (isCustomWeapon)
        {
            float greenBlueColors = 1.0f - weaponTimeoutTimer / weaponTimeout;
            resultColor *= new Color(1.0f, greenBlueColors, greenBlueColors, 1.0f);
        }
        const float effectStrength = 0.6f;
        resultColor *= 1.0f - this.currentWeapon.GetNormalizedReloadTime() * effectStrength;

        // material.SetColor(materialColorID, new Color(1.0f - weaponTimeoutTimer / weaponTimeout, 1.0f, 1.0f));
        // material.SetColor(materialColorID, new Color(1.0f - this.currentWeapon.GetNormalizedReloadTime() / 2.0f, 1.0f - this.currentWeapon.GetNormalizedReloadTime() / 2.0f, 1.0f - this.currentWeapon.GetNormalizedReloadTime() / 2.0f, 1.0f));
        material.SetColor(materialColorID, resultColor);
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
