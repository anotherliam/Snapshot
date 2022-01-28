using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelController : MonoBehaviour
{
    enum PixelState
    {
        Idle,
        Changing
    }
    Renderer render;
    PixelState state = PixelState.Idle;
    float elapsedTime = 0f;
    Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        ChangeColor(Color.black);
    }

    void ChangeColor(Color color)
    {
        render.material.SetColor("_Color", color);
    }

    public void TurnOn(Color color)
    {
        elapsedTime = 0f;
        state = PixelState.Changing;
        ChangeColor(color);
    }

    public void TurnOff()
    {
        state = PixelState.Idle;
        ChangeColor(Color.black);
        transform.localScale = Vector3.zero;
    }

    public void Update()
    {
        if (state == PixelState.Changing)
        {
            if (elapsedTime < 0.3f)
            {
                var prog = elapsedTime / 0.3f;
                transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, prog);
                elapsedTime += Time.deltaTime;
            } else
            {
                transform.localScale = originalScale;
                state = PixelState.Idle;
                elapsedTime = 0;
            }
        }
    }
}
