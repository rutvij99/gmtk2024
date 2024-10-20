using System;
using DG.Tweening;
using GravityWell.Common.Helpers;
using GravityWell.Core.Config;
using UnityEngine;
using UnityEngine.Serialization;

namespace GravityWell.UI
{
	[DefaultExecutionOrder(-1)]
	public class SettingsUIHandler : MonoBehaviour
	{
		public static SettingsUIHandler Instance { get; private set; }

		public bool IsConfirmationRequired { get; private set; }

		protected void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Destroy(this.gameObject);
				return;
			}
			ResetContextMenu();
		}

		#region Gameplay Settings
		public void OnLanguageChanged(int languageIndex)
		{
			// IsConfirmationRequired = true;
			Core.Config.GameConfig.Instance.SettingsDataModifier.ModifyGameplaySettings(gameplaySettings =>
				gameplaySettings.Language = (Language)languageIndex);
		}

		public void OnZoomChanged(float zoomSensitivity)
		{
			Core.Config.GameConfig.Instance.SettingsDataModifier.ModifyGameplaySettings(gameplaySettings =>
				gameplaySettings.ZoomSensitivity = zoomSensitivity);
		}

		public void OnPanChanged(float panSensitivity)
		{
			Core.Config.GameConfig.Instance.SettingsDataModifier.ModifyGameplaySettings(gameplaySettings =>
				gameplaySettings.PanSensitivity = panSensitivity);
		}
		#endregion


		#region Settings Context Menu
		[SerializeField] private CanvasGroup _contextMenuCanvasGroup;
		[SerializeField] private GameObject _applySettingsButton;
		[SerializeField] private GameObject _resetToDefaultsButton;


		private void ResetContextMenu()
		{
			ShowContextMenu(false);
		}
		
		public void ShowContextMenu(bool show, float toggleDuration = 0f, Action onComplete = null)
		{
			_contextMenuCanvasGroup.gameObject.SetActive(show);
			if (show)
			{
				_contextMenuCanvasGroup.alpha = 0;
			}
			_contextMenuCanvasGroup.DOFade(show ? 1 : 0, toggleDuration).OnComplete(() =>
			{
				_contextMenuCanvasGroup.blocksRaycasts = _contextMenuCanvasGroup.interactable = show;
				onComplete?.Invoke();
			});
			ShowApplyContext(false);
			ShowResetContext(false);
		}
		
		public void ShowApplyContext(bool show)
		{
			_applySettingsButton.gameObject.SetActive(show);
		}
		
		public void ShowResetContext(bool show)
		{
			_resetToDefaultsButton.gameObject.SetActive(show);
		}
		
		public void ApplySettings()
		{
			
		}

		public void TryResetToDefaults()
		{
			
		}

		private void ResetToDefaults()
		{
			
		}
		#endregion
	}
}