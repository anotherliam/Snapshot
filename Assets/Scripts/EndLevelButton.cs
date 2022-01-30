using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevelButton : MonoBehaviour
{
    private readonly float FADE_DURATION = 1.0f;
    private readonly Color TRANSPARENT_BLACK = new Color(0, 0, 0, 0);

    private TextMeshPro textMesh;
    private AudioSource audioSource;
    private AudioSource bgMusicAudioSource;
    private Color initialColor;
    private bool isFading;
    private float fadeProgress;
    public GameObject Text;
    public GameObject BGMusicGameObject;
    public AudioClip OnExitSound;
    public Image BlackImage;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bgMusicAudioSource = BGMusicGameObject.GetComponent<AudioSource>();
        textMesh = Text.GetComponent<TextMeshPro>();
        initialColor = textMesh.color;
    }

    private void OnMouseEnter()
    {
        textMesh.color = Color.white;
        if (GlobalGameState.GameState.SoundEnabled && !isFading)
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
        if (isFading) return;
        audioSource.PlayOneShot(OnExitSound);
        isFading = true;
        fadeProgress = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFading)
        {
            fadeProgress += Time.deltaTime;
            var prog = fadeProgress / FADE_DURATION;
            var newColor = Color.Lerp(TRANSPARENT_BLACK, Color.black, prog);
            var newVol = Mathf.Lerp(1, 0, prog);
            BlackImage.color = newColor;
            bgMusicAudioSource.volume = newVol;
            if (prog >= 1f)
            {
                SceneManager.LoadScene("LevelSelectScene");
            }
        }
    }
}
