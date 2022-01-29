using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{

    private Color EnabledColor = Color.blue;
    private Color DisabledColor = Color.red;
    private Color HoverColor = Color.white;

    private bool isEnabled;
    private Renderer rend;

    public Texture2D Off;
    public Texture2D On;
    public bool IsMusic;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        Refresh();
    }

    private void Refresh()
    {
        var gameState = GlobalGameState.GameState;
        isEnabled = IsMusic ? gameState.MusicEnabled : gameState.SoundEnabled;
        rend.material.mainTexture = isEnabled ? On : Off;
        ChangeColor();
    }

    private void ChangeColor()
    {
        rend.material.SetColor("_Color", isEnabled ? EnabledColor : DisabledColor);
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse Enter");
        rend.material.SetColor("_Color", HoverColor);
    }

    private void OnMouseExit()
    {
        ChangeColor();
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("Mouse Click");
        var gameState = GlobalGameState.GameState;
        if (IsMusic)
        {
            gameState.MusicEnabled = !gameState.MusicEnabled;
        } else
        {
            gameState.SoundEnabled = !gameState.SoundEnabled;
        }
        Refresh();
    }
}
