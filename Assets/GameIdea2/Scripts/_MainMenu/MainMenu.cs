using System;
using DG.Tweening;
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
	private void Start()
	{
		menuHolder.gameObject.SetActive(true);
		optionsHolder.gameObject.SetActive(false);
		levelSelectHolder.gameObject.SetActive(false);
		sfxSource = GetComponent<AudioSource>();
		// ShowMenu();
	}

	public void ClickSFX()
	{
		if (!clickSound) return;
		sfxSource.PlayOneShot(clickSound, volumeScale: 0.5f);
	}

	public void ShowLevelSelect()
	{
		menuHolder.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
		{
			menuHolder.gameObject.SetActive(false);
		});
		levelSelectHolder.gameObject.SetActive(true);
		levelSelectHolder.GetComponent<CanvasGroup>().alpha = 0;
		levelSelectHolder.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
	}

	public void ShowOptions()
	{
		menuHolder.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
		{
			menuHolder.gameObject.SetActive(false);
		});
		optionsHolder.gameObject.SetActive(true);
		optionsHolder.GetComponent<CanvasGroup>().alpha = 0;
		optionsHolder.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
	}

	public void ShowMenu()
	{
		optionsHolder.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
		{
			optionsHolder.gameObject.SetActive(false);
		});
		levelSelectHolder.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
		{
			levelSelectHolder.gameObject.SetActive(false);
		});
		menuHolder.gameObject.SetActive(true);
		menuHolder.GetComponent<CanvasGroup>().alpha = 0;
		menuHolder.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
	}
	
	public void LoadLevel(int i)
	{
		Debug.Log($"Loading Level {i}");
		SceneManager.LoadScene($"Level{i}");
	}

	public void LoadFreePlay()
	{
		SceneManager.LoadScene($"FreePlay");
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
