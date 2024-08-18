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
	[SerializeField] private GameObject levelSelectHolder;
	[SerializeField] private GameObject optionsHolder;

	[SerializeField] private AudioClip clickSound;
	private AudioSource sfxSource;
	private CanvasGroup optionsCanvasGrp;
	private CanvasGroup levelsCanvasGrp;
	private CanvasGroup menuCanvasGrp;

	private void Start()
	{
		menuCanvasGrp = menuHolder.GetComponent<CanvasGroup>();
		optionsCanvasGrp = optionsHolder.GetComponent<CanvasGroup>();
		levelsCanvasGrp = levelSelectHolder.GetComponent<CanvasGroup>();
		
		menuHolder.gameObject.SetActive(true);
		optionsHolder.gameObject.SetActive(false);
		levelSelectHolder.gameObject.SetActive(false);
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

	public void ShowLevelSelect()
	{
		Reset();
		menuHolder.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
		{
			menuHolder.gameObject.SetActive(false);
		});
		
		levelSelectHolder.gameObject.SetActive(true);
		levelsCanvasGrp.alpha = 0;
		levelsCanvasGrp.DOFade(1, 0.5f);
	}

	public void ShowOptions()
	{
		Reset();
		menuHolder.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
		{
			menuHolder.gameObject.SetActive(false);
		});
		
		optionsHolder.gameObject.SetActive(true);
		optionsCanvasGrp.alpha = 0;
		optionsCanvasGrp.DOFade(1, 0.5f);
	}

	public void ShowMenu()
	{
		Reset();
		
		optionsHolder.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
		{
			optionsHolder.gameObject.SetActive(false);
		});
		levelSelectHolder.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
		{
			levelSelectHolder.gameObject.SetActive(false);
		});
		
		menuHolder.gameObject.SetActive(true);
		menuCanvasGrp.alpha = 0;
		menuCanvasGrp.DOFade(1, 0.5f);
	}
	
	public void LoadLevel(int i)
	{
		Debug.Log($"Loading Level {i}");
		SceneManager.LoadScene($"Level{i}");
	}

	public void LoadFreePlay()
	{
		SceneManager.LoadScene($"GameIdea2Scene");
		// SceneManager.LoadScene($"FreePlay");
	}

	public void Quit()
	{
#if UNITY_EDITOR
		// Check if the editor is currently in Play Mode
		if (EditorApplication.isPlaying)
		{
			// Stop Play Mode
			EditorApplication.isPlaying = false;
		}
#endif
		Application.Quit();
	}
}
