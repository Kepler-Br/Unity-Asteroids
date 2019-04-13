using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float thrustForce = 80.0f;
    public float rotationSpeed = 3.0f;
    public float weaponTimeout = 15.0f;
    public GameObject destroyedShipPrefab;
    public GameObject playerLivesDisplay;
    public int lives = 3;

    private GameManager gameManager;
    private Rigidbody2D playerRigidBody;
    private IPlayerWeapon currentWeapon;
    MeshRenderer meshRenderer;
    private Material material;
    private float weaponTimeoutTimer = 0.0f;
    private bool isCustomWeapon = false;
    private int materialColorID;
    private bool destroyed = false;

    void Start()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        currentWeapon = new StingerWeapon(this.gameObject);
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
            case WeaponType.Stinger:
                currentWeapon = new StingerWeapon(this.gameObject);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Asteroid")
        {
            lives--;
            gameManager.SendMessage("ClearPlayField");
            LiveLost();
            if (lives == 0)
                gameManager.SendMessage("OnGameOver");
        }
    }

    void LiveLost()
    {
        GameObject destroyedShip = Instantiate(destroyedShipPrefab, this.transform.position, this.transform.rotation);
        DestroyedShip destroyedShipScript = destroyedShip.GetComponent<DestroyedShip>();
        if (lives > 0)
            destroyedShipScript.rebuildAnimation = true;
        else
            destroyedShipScript.rebuildAnimation = false;
        meshRenderer.enabled = false;
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        destroyed = true;
        playerRigidBody.velocity = Vector2.zero;
        playerLivesDisplay.SendMessage("SetIconCount", lives);
    }

    void OnDeathAnimationEnd()
    {

        meshRenderer.enabled = true;
        destroyed = false;
        gameManager.SendMessage("OnPlayerRespawn");
    }

    void Replay()
    {
        lives = 3;
        gameManager.SendMessage("OnPlayerRestart");
        meshRenderer.enabled = true;
        destroyed = false;
        playerLivesDisplay.SendMessage("SetIconCount", lives);
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
        ProcessInput();
    }

    void ProcessInput()
    {
        if (lives == 0 && Input.GetKey(KeyCode.Return))
        {
            Replay();
        }
        if (destroyed)
            return;


        float verticalMovement = Input.GetAxis("Vertical-keyboard");
        playerRigidBody.AddForce(this.transform.up * this.thrustForce * verticalMovement);

        float fire = Input.GetAxis("Fire");
        if (fire > 0.0f)
            currentWeapon.Fire();

        float rotation = Input.GetAxis("Horizontal-keyboard");
        transform.Rotate(0.0f, 0.0f, -rotationSpeed * rotation);

        float joystickY = Input.GetAxis("Vertical-joystick");
        float joystickX = Input.GetAxis("Horizontal-joystick");
        Vector3 resultMovement = new Vector3(joystickX, joystickY, 0.0f);
        playerRigidBody.AddForce(resultMovement * this.thrustForce);
        float angle = Vector3.Angle(Vector3.left, resultMovement);

        float aimAngle = Mathf.Atan2(-joystickX, joystickY) * Mathf.Rad2Deg;

        if (joystickY != 0 && joystickX != 0)

        {
            this.transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        }
        // transform.rotation  = Quaternion.LookRotation(resultMovement);

    }
}
