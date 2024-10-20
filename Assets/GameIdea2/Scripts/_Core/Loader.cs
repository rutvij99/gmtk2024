using System;
using DG.Tweening;
using GravityWell.Common.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace GravityWell.Core
{
    [DefaultExecutionOrder(-99)]
    public class Loader : Singleton<Loader>
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private CanvasGroup loadingGroup;
        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private Image progressBar;

        private bool canDisable = false;


        protected override void Awake()
        {
            base.Awake();
            if(Instance != this) return;
            loadingPanel.SetActive(false);
        }
        
        private void Update()
        {
            if(!canDisable) return;
            if (Input.anyKeyDown)
            {
                HideLoadingUI();
            }
        }
        
        public void ShowLoadingUI(string message)
        {
            canDisable = false;
            loadingPanel.SetActive(true);
            loadingGroup.interactable = true;
            loadingGroup.blocksRaycasts = true;
            loadingGroup.alpha = 1;
            loadingText.text = message;
        }

        public void HideLoadingUI()
        {
            canDisable = false;
            loadingGroup.interactable = false;
            loadingGroup.blocksRaycasts = false;
            loadingGroup.alpha = 0;
            loadingPanel.SetActive(false);
        }
        
        private void AllowHideOnInput(string message)
        {
            Instance.canDisable = true;
            Instance.loadingText.text = message;
        }
        
        private void UpdateProgressUI(float progress)
        {
            progressBar.fillAmount = progress; // progress should be between 0 and 1
        }
        
        
        public static void Show(string message = "Loading")
        {
            Instance.ShowLoadingUI(message);
        }

        public static void Hide()
        {
            Instance.HideLoadingUI();
        }
        
        public static void UpdateProgress(float progress)
        {
            Instance.UpdateProgressUI(progress);
        }

        
        public static void AllowDisabling(string message = "Press Any Key To Continue")
        {
            Instance.AllowHideOnInput(message);
        }
    }
}
