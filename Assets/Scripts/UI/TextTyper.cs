using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TextTyper : MonoBehaviour
{
    public Action OnTypingEnd;
    public Action OnTextHidden;

    [SerializeField]
    Text _textField = null;
    [SerializeField]
    AudioSource _typeSound = null;
    [SerializeField]
    CustomTimer _typeSpeed = null;
    [SerializeField]
    CustomTimer _timeBeforeHideText = null;

    string _textToType = "";
    int _stringIndex;
    bool _typingEnded = false;

    public void TypeText(string text)
    {
        _textToType = text;
        _textField.enabled = true;
        _textField.text = "";
        _typingEnded = false;
        _typeSpeed.Reset();
    }

    void Start()
    {
        // _textField.enabled = false;
    }

    void Update()
    {
        if (_typeSpeed.TryReset() && _stringIndex < _textToType.Length)
        {
            char currentChar = _textToType[_stringIndex];
            _textField.text += currentChar;
            _stringIndex++;
            // if (currentChar != ' ' || currentChar!='\n')
            //     _typeSound.Play();
            // Debug.Log(currentChar == ' ');
            if (currentChar != ' ' && currentChar != '\n')
                _typeSound.Play();
        }
        if (_stringIndex == _textToType.Length && !_typingEnded)
        {
            OnTypingEnd?.Invoke();
            _timeBeforeHideText.Reset();
            _typingEnded = true;
        }
        if (_typingEnded && _timeBeforeHideText.TryReset())
        {
            OnTextHidden?.Invoke();
            _textField.enabled = false;
        }
    }
}
