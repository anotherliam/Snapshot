using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    class LevelSelectPolaroidController : MonoBehaviour
    {

        public Texture2D CompletedTexture;
        public GameObject SelectionObject;
        public GameObject PaperObject;
        public GameObject PolaroidObject;
        public int LevelID;

        void Start()
        {
            var completed = GlobalGameState.GameState.IsLevelCompleted(LevelID);
            if (completed)
            {
                PolaroidObject.GetComponent<Renderer>().material.mainTexture = CompletedTexture;
                PaperObject.SetActive(true);
            } else
            {
                PaperObject.SetActive(false);
            }
        }

        private void OnMouseEnter()
        {
            SelectionObject.SetActive(true);
        }
        private void OnMouseExit()
        {
            SelectionObject.SetActive(false);
        }
        private void OnMouseUpAsButton()
        {
            GlobalGameState.GameState.SelectedLevelID = LevelID;
            SceneManager.LoadScene("SampleScene");
        }

    }
}
