using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitDisplayScript : MonoBehaviour
{
    [SerializeField]
    private int score = 0;
    private const uint digitCount = 6;
    private Text textMesh;

    void Start()
    {
        textMesh = this.GetComponent<Text>();
        score = 0;
        UpdateDisplay();

        GameEvents.GameRestart += OnGameRestart;
        GameEvents.AsteroidDestroyed += OnAsteroidDestroyed;
    }


    void OnGameRestart()
    {
        score = 0;
        this.UpdateDisplay();
    }

    void OnAsteroidDestroyed(int score)
    {
        this.score += score;
        this.UpdateDisplay();
    }

    void UpdateDisplay()
    {
        uint digitsInScore = (uint)Mathf.Log10(score);
        digitsInScore = digitsInScore > digitCount ? digitCount : digitsInScore;
        textMesh.text = new string('0', (int)(digitCount - digitsInScore));
        textMesh.text += score.ToString();
    }
}
