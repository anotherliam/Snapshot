using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButton : MonoBehaviour
{
    public GameObject SelectionHighlight;
    public GameObject CreditsObject;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        SelectionHighlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        SelectionHighlight.SetActive(false);
    }

    private void OnMouseUpAsButton()
    {
        CreditsObject.SetActive(!CreditsObject.activeSelf);
    }
}
