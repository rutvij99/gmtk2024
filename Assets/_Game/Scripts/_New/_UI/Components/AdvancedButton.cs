using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GravityWell.UI
{
	public class AdvancedButton : AdvancedSelectable, IPointerClickHandler, ISubmitHandler
	{
		[Serializable] public class ButtonClickedEvent : UnityEvent {}
		// Exposed events
		[Serializable] public class ButtonHoverStartEvent : UnityEvent {}
		[Serializable] public class ButtonSelectedEvent : UnityEvent {}
		[Serializable] public class ButtonDisabledEvent : UnityEvent {}
		[Serializable] public class ButtonEnabledEvent : UnityEvent {}
		[Serializable] public class ButtonHoverEndEvent : UnityEvent {} // New highlight removed event

		[SerializeField] private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

		[SerializeField] private ButtonHoverStartEvent m_OnHoverStart = new ButtonHoverStartEvent();
		[SerializeField] private ButtonHoverEndEvent m_OnHoverEnd = new ButtonHoverEndEvent(); // Serialized field for Unity Inspector
		[SerializeField] private ButtonSelectedEvent m_OnSelected = new ButtonSelectedEvent();
		[SerializeField] private ButtonEnabledEvent m_OnEnabled = new ButtonEnabledEvent();
		[SerializeField] private ButtonDisabledEvent m_OnDisabled = new ButtonDisabledEvent();

		protected AdvancedButton()
		{
		}
		
		


		public ButtonClickedEvent onClick
		{
			get { return m_OnClick; }
			set { m_OnClick = value; }
		}

		public ButtonSelectedEvent onSelected => m_OnSelected;
		public ButtonHoverStartEvent OnHoverStart => m_OnHoverStart;
		public ButtonHoverEndEvent OnHoverEnd => m_OnHoverEnd; // Public accessor for highlight removed
		public ButtonEnabledEvent onEnabled => m_OnEnabled;
		public ButtonDisabledEvent onDisabled => m_OnDisabled;

		protected override void Awake()
		{
			base.Awake();
			if(isEnabled) Enable();
			else Disable();
		}

		private void Press()
		{
			if (!IsActive() || !IsInteractable() || !isEnabled)
				return;

			UISystemProfilerApi.AddMarker("Button.onClick", this);
			m_OnClick.Invoke();
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			m_OnHoverStart.Invoke(); // Invoke highlighted event
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
			m_OnHoverEnd.Invoke(); // Invoke highlight removed event
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
				return;

			Press();
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			m_OnSelected.Invoke(); // Invoke selected event
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			m_OnHoverEnd.Invoke(); // Invoke highlight removed event
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			Press();

			if (!IsActive() || !IsInteractable())
				return;

			DoStateTransition(SelectionState.Pressed, false);
			StartCoroutine(OnFinishSubmit());
		}

		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			base.DoStateTransition(state, instant);
			if (!gameObject.activeInHierarchy)
				return;
			// if (state == SelectionState.Disabled)
			// {
			// 	Debug.Log($"{this.gameObject.name} is disabled");
			// 	m_OnDisabled.Invoke(); // Invoke disabled event
			// }
		}

		private IEnumerator OnFinishSubmit()
		{
			var fadeTime = colors.fadeDuration;
			var elapsedTime = 0f;

			while (elapsedTime < fadeTime)
			{
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}

			DoStateTransition(currentSelectionState, false);
		}
		
		public override void Enable()
		{
			base.Enable();
#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
#endif
			m_OnEnabled?.Invoke();
		}
		public override void Disable()
		{
			base.Disable();
#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
#endif
			m_OnDisabled.Invoke();
		}
	}
}