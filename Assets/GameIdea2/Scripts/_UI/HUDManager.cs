using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


[System.Serializable]
public class UploadMetaData
{
    private string authorName;
    private string leveName;
}
public class HUDManager : MonoBehaviour
{
    [SerializeField] private GameObject resetWorldButton;
    [SerializeField] private GameObject exitSimulationButton;
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private CanvasGroup uploadWindow;

    private UploadMetaData uploadMeta = new UploadMetaData();
    
    [Space(10)]
    public UnityEvent OnResetCameraClicked = new UnityEvent();
    public UnityEvent OnResetWorldClicked = new UnityEvent();
    public UnityEvent OnExitSimulationClicked = new UnityEvent();
    public UnityEvent OnExitToMainMenuClicked = new UnityEvent();
    public UnityEvent OnLoadNextLevelClicked = new UnityEvent();
    public UnityEvent<UploadMetaData> OnUploadClicked = new UnityEvent<UploadMetaData>();
    
    
    public void OnEnterEditor()
    {
        resetWorldButton?.gameObject.SetActive(true);
        exitSimulationButton?.gameObject.SetActive(false);
    }
    
    public void OnEnterSimulation()
    {
        resetWorldButton?.gameObject.SetActive(false);
        exitSimulationButton?.gameObject.SetActive(true);
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

    public void Button_UploadConfirm()
    {
        OnUploadClicked?.Invoke(uploadMeta);
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



    public void Button_LoadNextLevel()
    {
        OnLoadNextLevelClicked?.Invoke();
    }
}
