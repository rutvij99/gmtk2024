using System;
using DG.Tweening;
using GameIdea2;
using GameIdea2.Audio;
using GameIdea2.Gameloop;
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
    [SerializeField] private GameObject simulateButton;
    [SerializeField] private GameObject hintGUI;
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
    
    public bool Interacted {
        get
        {
            if (!hintGUI)
                return false;

            return hintGUI.activeSelf;
        }
        set
        {
            if(!hintGUI)
                return;
                
            hintGUI.SetActive(!value);
        }
    }

    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    public void EnableEditorView()
    {
        simView.gameObject.SetActive(false);
        resetWorldButton?.gameObject.SetActive(true);
        exitSimulationButton?.gameObject.SetActive(false);
    }

    public void EnableSimView()
    {
        simView.gameObject.SetActive(true);
        editorView.gameObject.SetActive(false);
        resetWorldButton?.gameObject.SetActive(false);
        exitSimulationButton?.gameObject.SetActive(true);
        PlayClick();
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
        PlayClick();
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
        PlayClick();
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
        PlayClick();
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
        PlayClick();
    }
    
    public void Button_UploadConfirm()
    {
        OnUploadClicked?.Invoke(levelInput.text, authorInput.text);
        PlayClick();
    }
    
    public void Button_ExitSim()
    {
        OnExitSimulationClicked?.Invoke();
        FindFirstObjectByType<EditModeController>()?.ResetLevel();
        PlayClick();
    }
    
    public void Button_ResetCamera()
    {
        OnResetCameraClicked?.Invoke();
        FindFirstObjectByType<EditModeController>()?.ResetCamera();
        PlayClick();
    }
    
    public void Button_ResetWorld()
    {
        OnResetWorldClicked?.Invoke();
        Universe.Instance.CleanWorkspace();
        FindFirstObjectByType<EditModeController>()?.ResetLevel();
        PlayClick();
    }

    public void Button_ExitToMainMenu()
    {
        OnExitToMainMenuClicked?.Invoke();
        // do other cleanup
        GameConfig.LoadLevel("MainMenu_New");

        PlayClick();
    }

    public void Button_Exit()
    {
        GameConfig.Exit();
        PlayClick();
    }
    
    public void ButtonTerrestrialBodySpawn(string key)
    {
        PlayClick();
        FindFirstObjectByType<EditModeController>()?.SpawnTerrestial(key);
    }

    public void PlayClick()
    {
        AudioManager.Instance?.PlaySoundOfType(SoundTyes.UI);
    }
    

    public void Button_Simulate()
    {
        EnableSimView();
        simulateButton.gameObject.SetActive(false);
        exitSimulationButton.SetActive(true);
        OnEnterSimulationClicked?.Invoke();
        var universe = Universe.Instance;
        if (universe != null) universe.Simulate = true;
        PlayClick();
    }


    public void Button_RestartLevel()
    {
        Universe.Instance.CleanWorkspace();
        GameManager.Instance.RestartLevel();
        PlayClick();
    }

    public void Button_LoadNextLevel()
    {
        GameManager.Instance.NextLevelLoad();
        OnLoadNextLevelClicked?.Invoke();
        PlayClick();
    }
    
    public void Button_LoadLevelComunity()
    {
        GameConfig.LoadLevel("CustomLevelSelector");
        PlayClick();
    }
}
