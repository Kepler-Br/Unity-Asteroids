using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    [SerializeField] GameState activeOnGameState = GameState.StartState;
    Text textMesh;
    [SerializeField] bool isVisible = true;
    [SerializeField] bool blink = true;
    [SerializeField] float blinkPeriond = 2.0f;

    float timer = 0.0f;

    void Awake()
    {
        timer = blinkPeriond;
        textMesh = this.GetComponent<Text>();
        GameEvents.GameStateChanged += OnGameStateChanged;
    }

    void OnGameStateChanged(GameState gameState)
    {
        if( gameState == activeOnGameState)
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
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            timer = blinkPeriond;
            isVisible = !isVisible;
            if (isVisible)
            {
                textMesh.enabled = true;
            }
            else
            {
                textMesh.enabled = false;
            }
        }
    }
}
