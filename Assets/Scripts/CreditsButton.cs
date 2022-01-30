using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButton : MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject SelectionHighlight;
    public GameObject CreditsObject;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        if (GlobalGameState.GameState.SoundEnabled)
        {
            audioSource.Play();
        }
        SelectionHighlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        SelectionHighlight.SetActive(false);
    }

    private void OnMouseUpAsButton()
    {
        CreditsObject.SetActive(!CreditsObject.activeSelf);
    }
}
