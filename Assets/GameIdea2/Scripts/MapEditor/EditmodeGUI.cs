using System;
using System.Collections;
using UnityEngine;

namespace GameIdea2.UI
{
    public class EditmodeGUI : MonoBehaviour
    {
        [SerializeField] private GameObject EditOnlyGUI;
        [SerializeField] private GameObject HintGUI;
        [SerializeField] private GameObject blueprintPlane;
        
        [Header("-- Gallery --")]
        [SerializeField] private RectTransform galleryRT;
        [SerializeField] private GameObject galleryOpenIco;
        [SerializeField] private GameObject galleryCloseIco;
            
        private bool galleryOpen = true;
        private Coroutine galleryLerpRoutine;
        
        public bool Interacted {
            get
            {
                if (!HintGUI)
                    return false;

                return HintGUI.activeSelf;
            }
            set
            {
                if(!HintGUI)
                    return;
                
                HintGUI.SetActive(!value);
            }
        }

        public void EnableSimulation()
        {
            EditOnlyGUI.SetActive(false);
            blueprintPlane.SetActive(false);
            var universe = Universe.Instance;
            if (universe != null) universe.Simulate = true;
        }

        public void RequestTerrestialBodySpawn(string key)
        {
            FindFirstObjectByType<EditModeController>()?.SpawnTerrestial(key);
        }

        public void ResetCamera()
        {
            FindFirstObjectByType<EditModeController>()?.ResetCamera();
        }

        public void ResetLevel()
        {
            FindFirstObjectByType<EditModeController>()?.ResetLevel();
        }

        public void ToggleGallery()
        {
            if(!galleryRT)
                return;
            
            galleryOpen = !galleryOpen;
            galleryOpenIco.SetActive(!galleryOpen);
            galleryCloseIco.SetActive(galleryOpen);
            
            if(galleryLerpRoutine != null)
                StopCoroutine(galleryLerpRoutine);

            galleryLerpRoutine = StartCoroutine(LerpGallery());
        }

        private IEnumerator LerpGallery()
        {
            var close = new Vector2(-galleryRT.sizeDelta.x, 0);
            var open = Vector2.zero;
            var start = galleryRT.anchoredPosition;
            var end = galleryOpen ? open : close;
            float timeStep = 0;
            while (timeStep <= 1)
            {
                timeStep += Time.deltaTime / 0.25f;
                galleryRT.anchoredPosition = Vector2.Lerp(start, end, timeStep);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}