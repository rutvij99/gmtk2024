using System;
using DG.Tweening;
using GravityWell.Common.Helpers;
using GravityWell.Core.Config;
using GravityWell.Core.Input;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GravityWell.UI
{
	[System.Serializable]
	public class ControlIconSet
	{
		public string name;
		public Image keyboard;
		public Image xbox;
		public Image ps;
	}
	[DefaultExecutionOrder(-1)]
	public class SettingsUIHandler : MonoBehaviour
	{
		public static SettingsUIHandler Instance { get; private set; }

		public bool IsConfirmationRequired { get; private set; }
		
		private const string backTextFormat = "Back";
		private const string selectTextFormat = "Select";
		[SerializeField] private ControlIconSet backIconSet;
		[SerializeField] private ControlIconSet selectIconSet;
		[SerializeField] private TMP_Text backText;
		[SerializeField] private TMP_Text selectText;
		private string backTextValue;
		private string selectTextValue;


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

		private void OnEnable()
		{
			OnControlsChanged(InputManager.ControlType);
			InputManager.OnControlChanged += OnControlsChanged;
		}

		private void OnDisable()
		{
			InputManager.OnControlChanged -= OnControlsChanged;
		}

		public void OnControlsChanged(ControlType controlType)
		{
			backIconSet.keyboard.gameObject.SetActive(controlType == ControlType.Keyboard);
			backIconSet.ps.gameObject.SetActive(false);
			backIconSet.xbox.gameObject.SetActive(controlType != ControlType.Keyboard);
			
			selectIconSet.keyboard.gameObject.SetActive(controlType == ControlType.Keyboard);
			selectIconSet.ps.gameObject.SetActive(false);
			selectIconSet.xbox.gameObject.SetActive(controlType != ControlType.Keyboard);
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
		[SerializeField] private GameObject _selectSettingsButton;
		[SerializeField] private GameObject _resetToDefaultsButton;


		private void ResetContextMenu()
		{
			ShowContextMenu(false);
		}
		
		public void ShowContextMenu(bool show)
		{
			ShowContextMenu(show, 0);
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
			ShowSelectContext(true);
			ShowApplyContext(false);
			ShowResetContext(false);
		}
		
		public void ShowSelectContext(bool show)
		{
			_selectSettingsButton.gameObject.SetActive(show);
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