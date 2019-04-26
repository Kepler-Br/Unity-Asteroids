using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BlinkingText : MonoBehaviour
{
    public Action EndBlinking;

    [SerializeField]
    GameState activeOnGameState = GameState.StartState;
    Text textMesh;
    bool isVisible = true;
    [SerializeField]
    bool blink = true;
    [SerializeField]
    CustomTimer _blinkPeriod = null;

    public void Restart()
    {
        _blinkPeriod.Reset();
        blink = true;
    }

    void Awake()
    {
        textMesh = this.GetComponent<Text>();
        GameEvents.GameStateChanged += OnGameStateChanged;
    }

    void OnGameStateChanged(GameState gameState)
    {
        if (gameState == activeOnGameState)
        {
            blink = true;
            isVisible = true;
            textMesh.enabled = true;
        }
        else
        {
            blink = false;
            isVisible = false;
            textMesh.enabled = false;
        }
    }

    void Update()
    {
        if (!blink)
            return;

        if (isVisible)
            textMesh.enabled = true;
        else
            textMesh.enabled = false;

        if (_blinkPeriod.TryReset())
            isVisible = !isVisible;

    }
}
