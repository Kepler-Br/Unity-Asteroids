using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public Action Fire;

    [SerializeField]
    Rigidbody2D playerRigidBody = null;
    [SerializeField]
    GameObject rocketExhaust = null;

    [SerializeField]
    float thrustForce = 300.0f;
    [SerializeField]
    float rotationSpeed = 5.0f;
    [SerializeField]
    bool isProcessingInput = false;

    void Awake()
    {
        GameEvents.PlayerDeath += OnPlayerDeath;
        GameEvents.PlayerRespawn += OnPlayerRespawn;
        GameEvents.GameStateChanged += OnGameStateChanged;
    }

    void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.GameOverState)
            isProcessingInput = false;
        if (gameState == GameState.PlayingState)
            isProcessingInput = true;
        if (gameState == GameState.StartState)
            isProcessingInput = false;
    }

    void OnPlayerDeath()
    {
        playerRigidBody.velocity = Vector2.zero;
        isProcessingInput = false;
        rocketExhaust.SetActive(false);
    }

    void OnPlayerRespawn()
    {
        isProcessingInput = true;
    }

    void FixedUpdate()
    {
        if (!isProcessingInput)
            return;
        ProcessKeyboard();
        ProcessJoystick();
    }

    void ProcessKeyboard()
    {
        float fireButton = Input.GetAxis("Fire");
        if (fireButton > 0.0f)
            this.Fire?.Invoke();
        float verticalMovement = Input.GetAxis("Vertical-keyboard");
        if (verticalMovement > 0.0f)
            rocketExhaust.SetActive(true);
        else
            rocketExhaust.SetActive(false);

        playerRigidBody.AddForce(this.transform.up * this.thrustForce * verticalMovement);
        float rotation = Input.GetAxis("Horizontal-keyboard");
        transform.Rotate(0.0f, 0.0f, -rotationSpeed * rotation);
    }

    void ProcessJoystick()
    {
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
    }
}
