using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    struct Pixel
    {
        public int Index;
        public Color Color;
    }

    public GameObject ReturnToLevelSelectText;

    private bool gameIsFinished = false;
    private int levelID;
    private List<Pixel> pixels;
    private List<int> layerTriggerIndexes;
    private GameObject[] nodes = { };
    private int currentIndex = 0;
    private TextMeshPro textMesh;
    private Assets.Scripts.TextState textState;
    private AudioSource audioSource;
    private float timeLeftInAudio;
    private bool isPlayingLoopAudio = false;
    private AudioClip bgMusic;
    private AudioClip bgMusicLoop;

    public GameObject PixelPrefab;
    public GameObject TextMeshObject;
    public GameObject Cursor;

    // Start is called before the first frame update
    void Start()
    {
        // Load images, sounds and story
        levelID = GlobalGameState.GameState.SelectedLevelID;
        var layers = ResourceLoader.LoadImages(levelID);
        var story = ResourceLoader.LoadStory(levelID);
        (bgMusic, bgMusicLoop) = ResourceLoader.LoadMusic(levelID);

        Cursor.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        textMesh = TextMeshObject.GetComponent<TextMeshPro>();
        textState = new Assets.Scripts.TextState(story);

        // Play the initial sound effect
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgMusic;
        timeLeftInAudio = audioSource.clip.length;
        audioSource.Play();

        // Do an initial set and force update here otherwise the first time the cursor renders it will be in the wrong place
        textMesh.SetText(textState.GetRichText(currentIndex));
        textMesh.ForceMeshUpdate();
        UpdateText();

        layerTriggerIndexes = new List<int>();
        pixels = new List<Pixel>();
        // Get pixel data from the textures
        for (var i = 0; i < layers.Count; i++)
        {
            var texture = layers[i];
            // Add a trigger for start of a new layer
            layerTriggerIndexes.Add(pixels.Count);

            // In Unity land, bottom left is 0,0
            // For our image we are using the same, but need to have the array sorted from top left instead
            for (var y = texture.height - 1; y >= 0; y--)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var pixel = texture.GetPixel(x, y);
                    if (pixel != Color.black)
                    {
                        pixels.Add(
                            new Pixel
                            {
                                Index = x + (y * texture.width),
                                Color = pixel
                            }
                        );
                    }
                }
            }
        }

        // Create pixel game objects and store references to them
        // Index is bottom left corner
        var nodeArr = new List<GameObject>();
        for (var y = 0; y < 32; y++)
        {
            for (var x = 0; x < 32; x++)
            {
                var node = Instantiate(PixelPrefab, transform);
                node.transform.localPosition = new Vector3(x - 16 + 0.5f, 0.5f, y - 16 + 0.5f);
                node.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                nodeArr.Add(node);
            }
        }
        nodes = nodeArr.ToArray();

    }

    void UpdateText()
    {
        textMesh.SetText(textState.GetRichText(currentIndex));
        var character = textMesh.textInfo.characterInfo[currentIndex];
        // Code for when using UI Text
        //var bottomLeft = textMesh.transform.TransformPoint(new Vector3(character.bottomLeft.x, character.baseLine - 8, 0));
        //Cursor.transform.position = bottomLeft;

        var bottomLeft = textMesh.transform.TransformPoint(new Vector3(character.origin + (Cursor.transform.localScale.x / 2), character.baseLine - 0.2f, character.bottomLeft.z));
        Cursor.transform.position = bottomLeft;
    }

    void HandleGameFinished()
    {
        Cursor.transform.localPosition = new Vector3(-500, -500, 0);
        gameIsFinished = true;
        GlobalGameState.GameState.SetLevelCompleted(levelID);
        ReturnToLevelSelectText.GetComponent<TextMeshPro>().SetText("Return to level select\n\n       (Level Complete)");
    }

    void HandleNextPixel()
    {
        if (gameIsFinished) return;
        // Draw the current pixel
        var pixel = pixels[currentIndex];
        nodes[pixel.Index].SendMessage("TurnOn", pixel.Color);
        currentIndex += 1;
        UpdateText();

        // If that was the last one, mark as completed
        if (currentIndex >= pixels.Count)
        {
            Debug.Log("Finished");
            HandleGameFinished();
            return;
        }

    }
    
    void HandleBackspace()
    {
        if (gameIsFinished) return;
        if (currentIndex <= 0)
        {
            return;
        }
        currentIndex -= 1;
        var pixel = pixels[currentIndex];
        nodes[pixel.Index].SendMessage("TurnOff");
        UpdateText();
    }

    void HandleReturnToMenu()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }

    // Update is called once per frame
    void Update()
    {
        // Sound update
        var musicEnabled = GlobalGameState.GameState.MusicEnabled;
        if (musicEnabled)
        {
            // un-mute since we're enabled
            audioSource.mute = false;
        } else
        {
            // Set mute since we're disabled
            audioSource.mute = true;
        }

        if (!isPlayingLoopAudio)
        {
            timeLeftInAudio -= Time.deltaTime;
            if (timeLeftInAudio <= 0f)
            {
                isPlayingLoopAudio = true;
                timeLeftInAudio = bgMusicLoop.length;
                audioSource.loop = true;
                audioSource.clip = bgMusicLoop;
                audioSource.Play();
            }
        }

        var str = Input.inputString;
        if (str.Length >= 1)
        {
            foreach (var c in str)
            {
                if (c == '\b')
                {
                    HandleBackspace();
                } else if (c == '\n' || c == '\r')
                {
                    // Enter, ignore
                }
                else
                {
                    // A character!
                    textState.EnterChar(currentIndex, c);
                    HandleNextPixel();
                }
            }
        }
    }
}
