using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectItem : MonoBehaviour
{
    Vector3 startPos;
    Vector3 hoverPos;
    bool isHovering = false;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        hoverPos = new Vector3(startPos.x, startPos.y, startPos.z - 0.35f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHovering)
        {
            Debug.Log("Is hoverin");
            isHovering = false;
            transform.position = Vector3.MoveTowards(transform.position, hoverPos, Time.deltaTime * 2.5f);
        } else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime * 2.5f);
        }
    }

    public void HandleHover()
    {
        isHovering = true;
    }
    public void HandleClick()
    {
        Debug.Log($"Clicked {name}");
        SceneManager.LoadScene("SampleScene");
    }
}
