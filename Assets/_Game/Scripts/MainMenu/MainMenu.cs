using System;
using DG.Tweening;
using GameIdea2.Audio;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameObject menuHolder;
	[SerializeField] private GameObject levelMenuHolder;
	[SerializeField] private GameObject levelSelectHolder;
	[SerializeField] private GameObject optionsHolder;	
	[SerializeField] private GameObject creditsHolder;

	private AudioSource sfxSource;
	private CanvasGroup optionsCanvasGrp;
	private CanvasGroup levelsCanvasGrp;
	private CanvasGroup menuCanvasGrp;
	private CanvasGroup levelMenuCanvasGrp;
	private CanvasGroup creditsCanvasGrp;

	private void Start()
	{
		menuCanvasGrp = menuHolder.GetComponent<CanvasGroup>();
		levelMenuCanvasGrp = levelMenuHolder.GetComponent<CanvasGroup>();
		optionsCanvasGrp = optionsHolder.GetComponent<CanvasGroup>();
		levelsCanvasGrp = levelSelectHolder.GetComponent<CanvasGroup>();
		creditsCanvasGrp = creditsHolder.GetComponent<CanvasGroup>();
		
		menuHolder.gameObject.SetActive(true);
		optionsHolder.gameObject.SetActive(false);
		levelSelectHolder.gameObject.SetActive(false);
		creditsHolder.gameObject.SetActive(false);
		sfxSource = GetComponent<AudioSource>();
		// ShowMenu();
	}

	public void ClickSFX()
	{
		AudioManager.Instance?.PlaySoundOfType(SoundTyes.UI);
		// if (!clickSound) return;
		// sfxSource.PlayOneShot(clickSound, volumeScale: 0.5f);
	}

	private void Reset()
	{
		levelsCanvasGrp.DOKill();
		optionsCanvasGrp.DOKill();
		menuCanvasGrp.DOKill();
	}
	
	public void ShowLevelMenu()
	{
		Reset();
		optionsCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			optionsCanvasGrp.gameObject.SetActive(false);
		});
		creditsCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			creditsCanvasGrp.gameObject.SetActive(false);
		});
		levelsCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			levelSelectHolder.gameObject.SetActive(false);
		});
		menuCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			menuHolder.gameObject.SetActive(false);
		});
		
		levelMenuCanvasGrp.gameObject.SetActive(true);
		levelMenuCanvasGrp.alpha = 0;
		levelMenuCanvasGrp.DOFade(1, 0.5f);
	}


	public void ShowLevelSelect()
	{
		Reset();
		optionsCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			optionsCanvasGrp.gameObject.SetActive(false);
		});
		creditsCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			creditsCanvasGrp.gameObject.SetActive(false);
		});
		// menuHolder.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
		// {
		// 	menuHolder.gameObject.SetActive(false);
		// });
		
		levelsCanvasGrp.gameObject.SetActive(true);
		levelsCanvasGrp.alpha = 0;
		levelsCanvasGrp.DOFade(1, 0.5f);
	}
	
	public void ShowCredits()
	{
		Reset();
		levelsCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			levelSelectHolder.gameObject.SetActive(false);
		});
		optionsCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			optionsCanvasGrp.gameObject.SetActive(false);
		});
		creditsCanvasGrp.gameObject.SetActive(true);
		creditsCanvasGrp.alpha = 0;
		creditsCanvasGrp.DOFade(1, 0.5f);
	}

	public void ShowOptions()
	{
		Reset();
		levelsCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			levelsCanvasGrp.gameObject.SetActive(false);
		});
		creditsCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			creditsCanvasGrp.gameObject.SetActive(false);
		});
		// menuHolder.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
		// {
		// 	menuHolder.gameObject.SetActive(false);
		// });
		
		optionsCanvasGrp.gameObject.SetActive(true);
		optionsCanvasGrp.alpha = 0;
		optionsCanvasGrp.DOFade(1, 0.5f);
	}

	public void ShowMenu()
	{
		Reset();
		
		optionsCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			optionsCanvasGrp.gameObject.SetActive(false);
		});
		levelsCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			levelsCanvasGrp.gameObject.SetActive(false);
		});
		creditsCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			creditsCanvasGrp.gameObject.SetActive(false);
		});
		levelMenuCanvasGrp.DOFade(0, 0.2f).OnComplete(() =>
		{
			levelMenuCanvasGrp.gameObject.SetActive(false);
		});
		
		menuHolder.gameObject.SetActive(true);
		// menuCanvasGrp.alpha = 0;
		menuCanvasGrp.DOFade(1, 0.5f);
	}
	
	public void LoadLevel(int i)
	{
		Debug.Log($"Loading Level {i}");
		AudioManager.Instance?.ChangeBackgroundMusic(i);
		GameConfig.LoadLevel(i);
	}

	public void LoadFreePlay()
	{
		// SceneManager.LoadScene($"GameIdea2Scene");
		SceneManager.LoadScene($"FreePlay");
	}

	public void Quit()
	{
		GameConfig.Exit();
	}

	public void LoadCommunitySelectScene()
	{
		GameConfig.LoadLevel("CustomLevelSelector");

	}

	public void LoadLevelEditor()
	{
		GameConfig.LoadLevel("LevelEditor");
	}

	public void ShowControls()
	{
		
	}

	public void LoadLastestLevel()
	{
		GameConfig.LoadLevel(GameConfig.GetLastCompletedLevel());
	}
}
