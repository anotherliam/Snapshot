using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicController : MonoBehaviour
{
    public AudioClip AudioLoop;

    private float timeLeft;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timeLeft = audioSource.clip.length;
        if (!GlobalGameState.GameState.MusicEnabled)
        {
            audioSource.Pause();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0f)
            {
                audioSource.clip = AudioLoop;
                audioSource.loop = true;
            }
        }

        // Change wether we are playing
        audioSource.mute = !GlobalGameState.GameState.MusicEnabled;
    }
}
