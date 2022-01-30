using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    private readonly float FADE_TIME = 1f;
    private readonly Color ColorTransparentBlack = new Color(0, 0, 0, 0);
    private Image imageComponent;
    private AudioSource audioSource;
    private AudioSource bgMusicAudioSource;
    private bool isLoading = false;
    private float audioTimeLeft = 0f;
    private float fadeProgress = 0f;

    public GameObject BlackImage;
    public GameObject BGMusicObject;


    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bgMusicAudioSource = BGMusicObject.GetComponent<AudioSource>();
        imageComponent = BlackImage.GetComponent<Image>();
    }

    public void HandleLoadLevel(int levelID)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;
        GlobalGameState.GameState.SelectedLevelID = levelID;
        fadeProgress = 0f;
        if (GlobalGameState.GameState.SoundEnabled)
        {
            audioSource.Play();
            audioTimeLeft = audioSource.clip.length;
        }
    }

    public void Update()
    {
        if (isLoading)
        {
            audioTimeLeft -= Time.deltaTime;
            fadeProgress += Time.deltaTime;
            var prog = fadeProgress / FADE_TIME;
            var newCol = Color.Lerp(ColorTransparentBlack, Color.black, prog);
            var newVol = Mathf.Lerp(1.0f, 0f, prog);
            bgMusicAudioSource.volume = newVol;
            imageComponent.color = newCol;
            if (audioTimeLeft <= 0f && prog >= 1.0f)
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }


}
