using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLivesDisplayScript : MonoBehaviour
{
    [SerializeField] private GameObject[] liveIcons;
    private int lives = 0;

    int maxHealth;

    void Awake()
    {
        maxHealth = liveIcons.Length;
        lives = maxHealth;
        GameEvents.GameRestart += OnGameRestart;
        GameEvents.PlayerDeath += OnPlayerDeath;
        GameEvents.HealthPickup += OnHealthPickup;
    }

    void OnHealthPickup()
    {
        SetHealth(++lives);
    }

    void OnGameRestart()
    {
        lives = 3;
        SetHealth(lives);
    }

    void OnPlayerDeath()
    {
        SetHealth(--lives);
    }

    void SetHealth(int newHealth)
    {
        newHealth = newHealth > liveIcons.Length ? liveIcons.Length : newHealth;
        foreach (var liveIcon in this.liveIcons)
            liveIcon.SetActive(false);
        for (int i = 0; i < newHealth; i++)
            liveIcons[i].SetActive(true);
    }
}
