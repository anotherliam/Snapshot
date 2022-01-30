using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelButton : MonoBehaviour
{
    private TextMeshPro textMesh;
    private AudioSource audioSource;
    private Color initialColor;
    public GameObject Text;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        textMesh = Text.GetComponent<TextMeshPro>();
        initialColor = textMesh.color;
    }

    private void OnMouseEnter()
    {
        textMesh.color = Color.white;
        if (GlobalGameState.GameState.SoundEnabled)
        {
            audioSource.Play();
        }
    }

    private void OnMouseExit()
    {
        textMesh.color = initialColor;
    }

    private void OnMouseUpAsButton()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
