using System;
using System.Collections;
using System.Collections.Generic;
// using AYellowpaper.SerializedCollections;
using DG.Tweening;
using GravityWell.Core.Input;
using GravityWell.UI;
using TMPro;
using UnityEngine;

namespace GravityWell.MainMenu
{
	public class PreLoaderUI : MenuUI
	{
		public override bool IsMain => true;
		
		[SerializeField] private MenuUI nextMenu;
		[SerializeField] private GameObject _downloadingConfigs;
		[SerializeField] private GameObject _continueLabel;
		[SerializeField] private ControlIconSet continueIconSet;

		private bool loadingComplete = false;
		
		private bool isReEntry = false;
		
		private Coroutine manualUpdateCoroutine;
		public override void Initialize(IMenuHandler handler)
		{
			Debug.Log($"PreLoaderUI Initializing -> isMain: {IsMain}");
			base.Initialize(handler);
		}

		public override void Enable()
		{
			ShowUI(true);
			_continueLabel?.SetActive(loadingComplete);
			_downloadingConfigs.SetActive(!loadingComplete);
			loadingComplete = false;
			if (manualUpdateCoroutine != null)
			{
				StopCoroutine(manualUpdateCoroutine);
			}
			manualUpdateCoroutine = StartCoroutine(ManualUpdate());
			OnControlsChanged(InputManager.ControlType);
			InputManager.OnControlChanged += OnControlsChanged;
		}
		
		public void OnControlsChanged(ControlType controlType)
		{
			continueIconSet.keyboard.gameObject.SetActive(controlType == ControlType.Keyboard);
			continueIconSet.ps.gameObject.SetActive(false);
			continueIconSet.xbox.gameObject.SetActive(controlType != ControlType.Keyboard);
		}

		private IEnumerator ManualUpdate()
		{
			yield return new WaitForSeconds(0.1f);
			while (true)
			{
				if (!this.IsEnabled) yield return null;

				if (!loadingComplete && Core.Config.GameConfig.IsConfigReady)
				{
					loadingComplete = true;
					DOTween.Sequence().AppendInterval(1)
						.AppendCallback(() =>
						{
							_continueLabel?.SetActive(loadingComplete);
							_downloadingConfigs.SetActive(!loadingComplete);
						});
				}
			

				if (loadingComplete && InputManager.SubmitStarted)
				{
					isReEntry = true;
					CompleteLoading();
					manualUpdateCoroutine = null;
					yield break;
				}
				yield return null;
			}
		}
		private void Update()
		{
			
		}

		private void CompleteLoading()
		{
			_handler.OpenMenu(nextMenu);
		}

		public override void Disable()
		{
			InputManager.OnControlChanged -= OnControlsChanged;
			ShowUI(false);
		}
	}
}
