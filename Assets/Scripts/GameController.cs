using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{

    struct Pixel
    {
        public int Index;
        public Color Color;
    }

    private List<Pixel> pixels;
    private List<int> layerTriggerIndexes;
    private GameObject[] nodes = { };
    private int currentIndex = 0;
    private float lastTriggerTime = 0.0f;
    private TextMeshPro textMesh;
    private Assets.Scripts.TextState textState;
    private AudioSource audioSource;
    private float timeLeftInAudio;
    private bool isPlayingIntroSound = true;
    private AudioClip bgMusic;

    private const float REPEAT_TIME = 1f / 30f; // 30 times per second

    public GameObject PixelPrefab;
    public GameObject TextMeshObject;
    public GameObject Cursor;

    // Start is called before the first frame update
    void Start()
    {
        // Load images, sounds and story
        var levelID = GlobalGameState.GameState.SelectedLevelID;
        var layers = ResourceLoader.LoadImages(levelID);
        var story = ResourceLoader.LoadStory(levelID);
        bgMusic = ResourceLoader.LoadMusic(levelID);

        Cursor.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        textMesh = TextMeshObject.GetComponent<TextMeshPro>();
        textState = new Assets.Scripts.TextState(story);

        // Do some sound stuff
        audioSource = GetComponent<AudioSource>();
        timeLeftInAudio = audioSource.clip.length;
        
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

    void HandleNextPixel()
    {
        // If weve finished
        if (currentIndex >= pixels.Count)
        {
            Debug.Log("Finished");
            Cursor.transform.localScale = new Vector3(0, 0, 0);
            return;
        }
        // Draw the next pixel
        var pixel = pixels[currentIndex];
        nodes[pixel.Index].SendMessage("TurnOn", pixel.Color);
        currentIndex += 1;
        UpdateText();

    }
    
    void HandleBackspace()
    {
        if (currentIndex <= 0)
        {
            return;
        }
        currentIndex -= 1;
        var pixel = pixels[currentIndex];
        nodes[pixel.Index].SendMessage("TurnOff");
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {

        // Sound update
        if (isPlayingIntroSound)
        {
            timeLeftInAudio -= Time.deltaTime;
            if (timeLeftInAudio <= 0f)
            {
                isPlayingIntroSound = false;
                audioSource.loop = true;
                audioSource.clip = bgMusic;
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
        //if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    HandleFinishLayer();
        //}
        //else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    HandleNextPixel();
        //}
        //else if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    if (lastTriggerTime + REPEAT_TIME <= Time.time)
        //    {
        //        lastTriggerTime = Time.time;
        //        HandleNextPixel();
        //    }

        //}
    }
}
