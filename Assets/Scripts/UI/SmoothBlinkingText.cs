using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SmoothBlinkingText : MonoBehaviour
{
    public Action EndBlinking;

    [SerializeField]
    Text _text = null;
    [SerializeField]
    CustomTimer _blinkPeriod = null;
    [SerializeField]
    AudioSource _sound = null;

    [SerializeField]
    bool _isBlinking = true;
    Color _textBaseColor;

    [SerializeField]
    int _willBlinkTimes = 3;
    [SerializeField]
    int _blinkTimes = 0;

    public void StartBlink()
    {
        _text.enabled = true;
        _blinkTimes = 0;
        _isBlinking = true;
        _text.color = Color.black;
    }

    void Start()
    {
        _textBaseColor = _text.color;
        _text.color = Color.black;
        _text.enabled = false;
    }

    void Update()
    {
        if (!_isBlinking)
            return;
        if (_blinkPeriod.TryReset() && _blinkTimes < _willBlinkTimes)
        {
            _sound.Play();
            _blinkTimes++;
        }
        if (_blinkTimes >= _willBlinkTimes)
        {
            _text.enabled = false;
            _isBlinking = false;
            EndBlinking?.Invoke();
        }
        _text.color = _textBaseColor * Mathf.Sin(_blinkPeriod.GetNormalizedRemainingTime() * Mathf.PI);
    }
}
