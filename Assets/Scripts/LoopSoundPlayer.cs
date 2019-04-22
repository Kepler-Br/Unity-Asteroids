using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopSoundPlayer : MonoBehaviour
{
    [SerializeField] AudioSource beginning = null;
    [SerializeField] AudioSource middle = null;
    bool isMuting = false;
    bool isVolumeUp = false;
    [SerializeField] bool isPlaying = false;

    [SerializeField] bool isPlayingBeginning = true;

    void Start()
    {
        GameEvents.GameStateChanged += OnGameStateChanged;
        GameEvents.PlayerDeath += OnPlayerDeath;
        GameEvents.PlayerRespawn += OnPlayerRespawn;

    }

    void OnPlayerDeath()
    {
        isMuting = true;
        isVolumeUp = false;
    }

    void OnPlayerRespawn()
    {
        isMuting = false;
        isVolumeUp = true;
    }

    void OnGameStateChanged(GameState gameState)
    {
        if (gameState == GameState.PlayingState)
        {
            isPlaying = true;
            isVolumeUp = false;
            isMuting = false;
            beginning.Play();
            middle.Stop();
            isPlayingBeginning = true;
            beginning.volume = 1.0f;
            middle.volume = 1.0f;
        }
        if (gameState == GameState.GameOverState)
        {
            isMuting = true;
            isVolumeUp = false;
        }

    }

    AudioSource GetCurrentSource()
    {
        if (isPlayingBeginning)
            return beginning;
        else
            return middle;
    }

    void SlowMute()
    {
        AudioSource currentSource = GetCurrentSource();
        float newVolume = Mathf.Lerp(currentSource.volume, 0, 0.08f);
        currentSource.volume = newVolume;
    }

    void SlowVolumeUp()
    {
        AudioSource currentSource = GetCurrentSource();
        float newVolume = Mathf.Lerp(currentSource.volume, 1, 0.08f);
        currentSource.volume = newVolume;
    }

    void FixedUpdate()
    {
        if (!isPlaying)
            return;
        if (isPlayingBeginning && !beginning.isPlaying)
        {
            isPlayingBeginning = false;
            middle.Play();
            beginning.Stop();
        }
        if (isPlaying && !isPlayingBeginning && !middle.isPlaying)
        {
            middle.Play();
        }
        if (isMuting)
            SlowMute();
        if (isVolumeUp)
            SlowVolumeUp();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
