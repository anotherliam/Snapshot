using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private readonly float FADE_TIME = 1.5f;
    private readonly Color ColorTransparentBlack = new Color(0, 0, 0, 0);
    private Image imageComponent;
    private bool isFading = true;
    private float fadeProgress = 0f;

    public GameObject BlackImage;


    public void Start()
    {
        imageComponent = BlackImage.GetComponent<Image>();
    }


    public void Update()
    {
        if (isFading)
        {
            fadeProgress += Time.deltaTime;
            var prog = fadeProgress / FADE_TIME;
            var newCol = Color.Lerp(Color.black, ColorTransparentBlack, prog);
            imageComponent.color = newCol;
            if (prog >= 1.0f)
            {
                isFading = false;
                BlackImage.SetActive(false);
            }
        }
    }


}
