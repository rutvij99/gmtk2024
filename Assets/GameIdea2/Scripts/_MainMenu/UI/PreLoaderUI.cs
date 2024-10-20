using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
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

		private bool loadingComplete = false;
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
		}

		private void Update()
		{
			if (!this.IsEnabled) return;

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

			if (loadingComplete && Input.anyKeyDown)
			{
				CompleteLoading();
			}
		}

		private void CompleteLoading()
		{
			_handler.OpenMenu(nextMenu);
		}

		public override void Disable()
		{
			ShowUI(false);
		}
	}
}
