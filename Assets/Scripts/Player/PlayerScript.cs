using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    GameObject[] lines = null;
    Material[] linesMaterials = null;
    int lineColorId;

    public GameObject destroyedShipPrefab = null;
    public GameObject weaponFirePlace = null;
    public int lives = 3;

    [SerializeField]
    AudioSource deathSound = null;
    WeaponFirePlace weaponFirePlaceScript = null;


    void Start()
    {
        int lineCount = lines.Length;
        linesMaterials = new Material[lineCount];
        lineColorId = Shader.PropertyToID("_MainColor");
        for (int i = 0; i < lineCount; i++)
            linesMaterials[i] = lines[i].GetComponent<Renderer>().material;
        weaponFirePlaceScript = weaponFirePlace.GetComponent<WeaponFirePlace>();
        GameEvents.GameStateChanged += OnGameStateChanged;
    }

    void SetLinesColor(Color color)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            linesMaterials[i] = lines[i].GetComponent<Renderer>().material;
            linesMaterials[i].SetColor(lineColorId, color);
        }
    }

    void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.GameOverState)
        {
            foreach (var line in lines)
                line.SetActive(false);
        }
        if (gameState == GameState.PlayingState)
        {
            foreach (var line in lines)
                line.SetActive(true);
            lives = 3;
        }
        if (gameState == GameState.StartState)
        {
            foreach (var line in lines)
                line.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Asteroid")
        {
            lives--;
            LiveLost();
            if (lives == 0)
            {
                foreach (var line in lines)
                    line.SetActive(true);
                GameEvents.OnGameOver();
            }
            deathSound.Play();
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
        foreach (var line in lines)
            line.SetActive(false);
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;

        GameEvents.OnPlayerDeath();
    }

    void OnDeathAnimationEnd()
    {
        foreach (var line in lines)
            line.SetActive(true);
        GameEvents.OnPlayerRespawn();
    }

    void Replay()
    {
        lives = 3;
        foreach (var line in lines)
            line.SetActive(true);

        GameEvents.OnGameRestart();
    }

    void Update()
    {
        UpdatePlayerColor();
    }

    void UpdatePlayerColor()
    {
        Color resultColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if (weaponFirePlaceScript.IsCustomWeapon())
        {
            float greenBlueColors = 1.0f - weaponFirePlaceScript.GetNormalizedWeaponTimeOut();
            resultColor *= new Color(1.0f, greenBlueColors, greenBlueColors, 1.0f);
        }
        const float effectStrength = 0.6f;
        resultColor *= 1.0f - weaponFirePlaceScript.GetNormalizedReloadTime() * effectStrength;
        SetLinesColor(resultColor);

        // material.SetColor(materialColorID, new Color(1.0f - weaponTimeoutTimer / weaponTimeout, 1.0f, 1.0f));
        // material.SetColor(materialColorID, new Color(1.0f - this.currentWeapon.GetNormalizedReloadTime() / 2.0f, 1.0f - this.currentWeapon.GetNormalizedReloadTime() / 2.0f, 1.0f - this.currentWeapon.GetNormalizedReloadTime() / 2.0f, 1.0f));
        // material.SetColor(materialColorID, resultColor);
    }
}
