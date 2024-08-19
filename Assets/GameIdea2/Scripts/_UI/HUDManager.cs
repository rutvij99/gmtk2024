using System;
using DG.Tweening;
using GameIdea2;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


[System.Serializable]
public class UploadMetaData
{
    public string authorName;
    public string leveName;
}

public class HUDManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup editorView;
    [SerializeField] private CanvasGroup simView;
    [SerializeField] private CanvasGroup hud;
    
    [Header("hud stuff")]
    [SerializeField] private GameObject resetWorldButton;
    [SerializeField] private GameObject exitSimulationButton;
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private CanvasGroup controlsMenu;
    
    [Header("upload window")]
    [SerializeField] private CanvasGroup uploadWindow;
    [SerializeField] private TMP_InputField authorInput;
    [SerializeField] private TMP_InputField levelInput;
    private UploadMetaData uploadMeta = new UploadMetaData();

    [Header("sim view stuff")]
    [SerializeField] private CanvasGroup levelCompleteWindow;
    [SerializeField] private RectTransform gallery;
    [SerializeField] private GameObject galleryOpenIcon;
    [SerializeField] private GameObject galleryCloseIcon;

    [Space(10)]
    [Header("Events")]
    public UnityEvent OnResetCameraClicked = new UnityEvent();
    public UnityEvent OnResetWorldClicked = new UnityEvent();
    public UnityEvent OnExitSimulationClicked = new UnityEvent();
    public UnityEvent OnExitToMainMenuClicked = new UnityEvent();
    public UnityEvent OnLoadNextLevelClicked = new UnityEvent();
    public UnityEvent OnEnterSimulationClicked = new UnityEvent();
    public UnityEvent<string,string> OnUploadClicked = new UnityEvent<string,string>();

    public static HUDManager instance;

    private void Start()
    {
        instance = this;
    }

    public void EnableEditorView()
    {
        
        resetWorldButton?.gameObject.SetActive(true);
        exitSimulationButton?.gameObject.SetActive(false);
    }

    public void EnableSimView()
    {
        
        resetWorldButton?.gameObject.SetActive(false);
        exitSimulationButton?.gameObject.SetActive(true);
    }

    public void EnableLevelComplete()
    {
        levelCompleteWindow?.gameObject.SetActive(true);
        // levelCompleteWindow.DOFade(1, 0.5f).OnComplete(() =>
        // {
        //     pauseMenu.gameObject.SetActive(false);
        // });
    }

    public void ToggleGallery()
    {
        gallery.DOKill();
        if (galleryOpenIcon.gameObject.activeInHierarchy)
        {
            // open gallery
            galleryOpenIcon.gameObject.SetActive(false);
            galleryCloseIcon.gameObject.SetActive(true);
            gallery.DOSizeDelta(new Vector2(gallery.sizeDelta.x, 850), 0.5f);
            
        }
        else
        {
            // close gallery
            galleryOpenIcon.gameObject.SetActive(true);
            galleryCloseIcon.gameObject.SetActive(false);
            gallery.DOSizeDelta(new Vector2(gallery.sizeDelta.x, 60), 0.5f);
        }
    }
    
    public void TogglePause()
    {
        if (pauseMenu.gameObject.activeInHierarchy)
        {
            // pauseMenu.alpha = 1;
            pauseMenu.DOFade(0, 0.1f).OnComplete(() =>
            {
                pauseMenu.gameObject.SetActive(false);
            });
        }
        else
        {
            pauseMenu.gameObject.SetActive(true);
            pauseMenu.alpha = 0;
            pauseMenu.DOFade(1, 0.5f).OnComplete(() =>
            {
                
            });
        }
    }

    public void ToggleUploadWindow()
    {
        if (uploadWindow.gameObject.activeInHierarchy)
        {
            // close
            uploadWindow.DOFade(0, 0.1f).OnComplete(() =>
            {           
                uploadWindow.gameObject.SetActive(false);
            });
        }
        else
        {    
    
            uploadMeta = new UploadMetaData();
            // open
            uploadWindow.gameObject.SetActive(true);
            uploadWindow.alpha = 0;
            uploadWindow.DOFade(1, 0.5f).OnComplete(() =>
            {

            });
        }
    }
    
    public void Button_ToggleControlsMenu()
    {
        if (controlsMenu.gameObject.activeInHierarchy)
        {
            // close
            controlsMenu.DOFade(0, 0.1f).OnComplete(() =>
            {           
                controlsMenu.gameObject.SetActive(false);
            });
        }
        else
        {
            // uploadMeta = new UploadMetaData();
            // open
            controlsMenu.gameObject.SetActive(true);
            controlsMenu.alpha = 0;
            controlsMenu.DOFade(1, 0.5f).OnComplete(() =>
            {

            });
        }
    }
    
    public void Button_UploadConfirm()
    {
        OnUploadClicked?.Invoke(uploadMeta.leveName, uploadMeta.authorName);
    }
    
    public void Button_ExitSim()
    {
        OnExitSimulationClicked?.Invoke();
    }
    
    public void Button_ResetCamera()
    {
        OnResetCameraClicked?.Invoke();
    }
    
    public void Button_ResetWorld()
    {
        OnResetWorldClicked?.Invoke();
    }

    public void Button_ExitToMainMenu()
    {
        OnExitToMainMenuClicked?.Invoke();
        // do other cleanup
        GameConfig.LoadMainMenu();
    }

    public void Button_Exit()
    {
        GameConfig.Exit();
    }


    public void Button_Simulate()
    {
        OnEnterSimulationClicked?.Invoke();
        var universe = Universe.Instance;
        if (universe != null) universe.Simulate = true;
    }


    public void Button_LoadNextLevel()
    {
        OnLoadNextLevelClicked?.Invoke();
    }
}
