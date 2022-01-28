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

    private Pixel[][] colorLayers = { };
    private int currentLayer = 0;
    private int currentIndex = 0;
    private int currentTextIndex = 0;
    public int CurrentIndex;
    private GameObject[] nodes = { };
    private float lastTriggerTime = 0.0f;
    private TextMeshProUGUI textMesh;
    private Assets.Scripts.TextState textState;

    private const float REPEAT_TIME = 1f / 30f; // 30 times per second

    public Texture2D[] Layers;
    public GameObject PixelPrefab;
    public GameObject TextMeshObject;
    public GameObject Cursor;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = TextMeshObject.GetComponent<TextMeshProUGUI>();
        textState = new Assets.Scripts.TextState("As the waves gently oscillate toward us we ponder what it is that makes the ocean so calming. Emma would argue it's not all the fish out there that is for sure. There's pirates though so maybe they aren't so bad if you can have those, she likely imagines. Of course there aren't any fish on land usually. That's actually the kind of place you'd find humans, sitting around on the shoreline drinking beers and stuff, just hanging out. Humans love that kind of thing honestly it's great. :) Course then there's that big goddamn sky out there, literally bigger than the whole planet what's up with that? Got clouds and everything, can't go anywhere without seeing that thing. To be quite honest it's so big that it might actually cause some trouble if we didn't have a lot to say about it, especially if we're holding off on talking about the interesting parts so we can have them all pop up in a pleasant way when we talk about them, something to think about anyway. Look at those stars shine. And don't forget about the moon!");
        UpdateText();
        var generatedLayers = new List<Pixel[]>();
        // Get pixel data from the textures
        for (var i = 0; i < Layers.Length; i++)
        {
            var texture = Layers[i];
            var pixelLayer = new List<Pixel>();
            // In Unity land, bottom left is 0,0
            // For our image we are using the same, but need to have the array sorted from top left instead
            for (var y = texture.height - 1; y >= 0; y--)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    var pixel = texture.GetPixel(x, y);
                    if (pixel != Color.black)
                    {
                        pixelLayer.Add(
                            new Pixel
                            {
                                Index = x + (y * texture.width),
                                Color = pixel
                            }
                        );
                    }
                }
            }
            generatedLayers.Add(pixelLayer.ToArray());
        }
        colorLayers = generatedLayers.ToArray();

        // Create pixel game objects and store references to them
        // Index is bottom left corner
        var nodeArr = new List<GameObject>();
        for (var y = 0; y < 32; y++)
        {
            for (var x = 0; x < 32; x++)
            {
                var node = Instantiate(PixelPrefab, new Vector3(x - 16 + 0.5f, 1, y - 16 + 0.5f), Quaternion.identity, transform);
                node.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                nodeArr.Add(node);
            }
        }
        nodes = nodeArr.ToArray();

    }

    void UpdateText()
    {
        textMesh.SetText(textState.GetRichText(currentTextIndex));
        var character = textMesh.textInfo.characterInfo[currentTextIndex];
        var bottomLeft = textMesh.transform.TransformPoint(new Vector3(character.bottomLeft.x, character.baseLine - 8, 0));
        Cursor.transform.position = bottomLeft;
    }


    void HandleFinishLayer()
    {
        // If were on the last layer, wipe everything
        if (currentLayer >= colorLayers.Length)
        {
            currentLayer = 0;
            currentIndex = 0;
            foreach (var node in nodes)
            {
                node.SendMessage("TurnOff");
            }

        }
        else
        {
            var layer = colorLayers[currentLayer];
            // Finish all the pixels in this layer
            foreach (var pixel in layer.Skip(currentIndex).ToArray())
            {
                nodes[pixel.Index].SendMessage("TurnOn", pixel.Color);
            }
            currentLayer += 1;
            currentIndex = 0;
        }
    }

    void HandleNextPixel()
    {
        // If were on the last layer, do nothing
        if (currentLayer >= colorLayers.Length)
        {
            return;
        }
        // Go to next letter as well?
        currentTextIndex++;
        UpdateText();
        // If we are on the last index, go to next layer
        if (currentIndex >= colorLayers[currentLayer].Length)
        {
            currentIndex = 0;
            currentLayer += 1;
            if (currentLayer >= colorLayers.Length)
            {
                return;
            }
        }
        // Draw the next pixel
        var pixel = colorLayers[currentLayer][currentIndex];
        nodes[pixel.Index].SendMessage("TurnOn", pixel.Color);
        currentIndex += 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            HandleFinishLayer();
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleNextPixel();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (lastTriggerTime + REPEAT_TIME <= Time.time)
            {
                lastTriggerTime = Time.time;
                HandleNextPixel();
            }

        }
    }
}
