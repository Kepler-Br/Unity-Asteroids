using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnormousAsteroid : MonoBehaviour
{
    string _bossDescription = "Boss name: enormous asteroid.\nTarget: annihilate.";
    TextTyper _textTyper;
    // Start is called before the first frame update
    void Start()
    {
        _textTyper = FindObjectOfType<TextTyper>();
        _textTyper.TypeText(_bossDescription);
        _textTyper.OnTypingEnd += OnBossDescriptionTypeEnd;
    }

    void OnBossDescriptionTypeEnd()
    {
        _textTyper.OnTypingEnd -= OnBossDescriptionTypeEnd;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
