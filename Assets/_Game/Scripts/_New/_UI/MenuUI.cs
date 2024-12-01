using System;
using DG.Tweening;
using GravityWell.Core.Config;
using GravityWell.Core.Input;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GravityWell.UI
{
	public abstract class MenuUI : MonoBehaviour
	{
		protected  IMenuHandler _handler;
		public virtual bool IsMain { get; }
		
		protected CanvasGroup _canvasGroup;

		protected GameObject firstSelected;
		
		protected bool IsEnabled
		{
			get
			{
				if (_canvasGroup == null) return false;
				return _canvasGroup.interactable;
			}
		}

		public virtual void Initialize(IMenuHandler handler)
		{
			this.gameObject.SetActive(true);
			_handler = handler;
			InputManager.OnCancelPressed += OnBackClicked;
			_canvasGroup = this.GetComponent<CanvasGroup>();
		}

		private void OnBackClicked()
		{
			if (IsEnabled)
			{
				GoBack();
			}
		}

		public abstract void Enable();
		public abstract void Disable();
		
		protected virtual void Update()
		{
			// if (IsEnabled && Input.GetKeyDown(KeyCode.Escape))
			// {
			// 	GoBack();
			// }
		}

		protected void ShowUI(bool show, float toggleDuration = 0f, Action onComplete = null)
		{
			_canvasGroup.DOFade(show ? 1 : 0, toggleDuration).OnComplete(() =>
			{
				_canvasGroup.blocksRaycasts = _canvasGroup.interactable = show;	
				onComplete?.Invoke();
			});
		}


		public void SelectFirstElement()
		{
			// Find the first Button in the children of the current transform
			var firstButton = GetComponentInChildren<Selectable>(false);
			if (firstButton != null)
			{
				// Cache it as firstSelected
				firstSelected = firstButton.gameObject;

				// Select the first button
				EventSystem.current.SetSelectedGameObject(firstSelected);
				// Debug.Log($"Selected {EventSystem.current.currentSelectedGameObject}");
			}
			else
			{
				Debug.LogWarning("No button found in children!");
			}
		}
		
		
		public void Button_OpenMenu(MenuUI menuUI)
		{
			_handler.OpenMenu(menuUI);
		}

		public void Button_GoBack()
		{
			GoBack();
		}

		public virtual void GoBack()
		{
			_handler.CloseMenu();
		}
	}
}
