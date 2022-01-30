using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypingSounds : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] Clips;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void HandlePlayRandomClip()
    {
        if (Assets.Scripts.GlobalGameState.GameState.SoundEnabled)
        {
            var clipIdx = Random.Range(0, Clips.Length);
            audioSource.PlayOneShot(Clips[clipIdx]);
        }
    }
}
