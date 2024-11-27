using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GravityWell.UI
{
	public class AdvancedSelectable : Selectable, IPointerClickHandler, ISubmitHandler
	{
		protected TMP_Text textField;
		private FontStyles originalStyle;
		private Vector3 originalScale;

		// Scale properties
		private float scaleDuration = 0.2f; // Time it takes to scale
		private float scalePunchAmount = 1.2f; // Target scale size when pointer enters


		[SerializeField] private bool m_isEnabled = true;
		public bool isEnabled
		{
			get { return m_isEnabled; }
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_isEnabled, value))
				{
					if (m_isEnabled)
					{
						Enable();
					}
					else
					{
						Disable();
					}
				}
			}
		}
		protected override void Awake()
		{
			textField = GetComponentInChildren<TMP_Text>();
			originalStyle = textField.fontStyle; // Store the original font style
			originalScale = textField.transform.localScale; // Store the original scale of the button
			base.Awake();
		}

		protected override void Start()
		{
			base.Start();
			
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			EndHighlighting();
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			StartHighlighting();
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			EndHighlighting();
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			EventSystem.current.SetSelectedGameObject(this.gameObject);
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);
		}
		
		public void OnSubmit(BaseEventData eventData)
		{
			
		}
		
		

		public void StartHighlighting(bool instant = false)
		{
			// Set text to bold
			if (textField != null)
			{
				textField.fontStyle |= FontStyles.Bold;
			}
			// Smooth scale up with punch effect using DOTween
			textField.transform.DOScale(originalScale * scalePunchAmount, instant ? 0 : scaleDuration).SetEase(Ease.OutBack);
		}

		public void EndHighlighting()
		{
			// Revert text to original style
			if (textField != null)
			{
				textField.fontStyle = originalStyle;
			}

			// Smooth scale down back to original using DOTween
			textField.transform.DOScale(originalScale, scaleDuration).SetEase(Ease.OutBack);
		}
		
		public virtual void Enable()
		{
#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
#endif
			// interactable = true;
			textField.DOFade(1f, 0);
		}
		public virtual void Disable()
		{
#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
#endif
			Debug.Log($"{this.gameObject.name} is disabled 1");
			// interactable = false;
			textField.DOFade(0.25f, 0);
		}
	}
}
