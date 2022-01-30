using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    public GameObject SelectionHighlight;
    void Start()
    {
        SelectionHighlight.SetActive(false);
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
        SelectionHighlight.SetActive(false);
        gameObject.SetActive(false);
    }

}
