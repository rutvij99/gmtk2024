using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GravityWell.UI
{
	public class CarouselSelectorGameObject : AdvancedSelectable
	{
		[SerializeField] private List<GameObject> _carouselItems;
		[SerializeField] private Transform _display;
		[SerializeField] private RectTransform _previousButton;
		[SerializeField] private RectTransform _nextButton;
		
		[SerializeField] private CarouselEventIndex _OnIndexValueChanged = new CarouselEventIndex();
		[SerializeField] private CarouselEventGameObject _OnValueChanged = new CarouselEventGameObject();

		public CarouselEventIndex onIndexValueChanged { get { return _OnIndexValueChanged; } set { _OnIndexValueChanged = value; } }
		public CarouselEventGameObject onValueChanged { get { return _OnValueChanged; } set { _OnValueChanged = value; } }

		private int _currentIndex;
		public GameObject value;

		protected override void Awake()
		{
			base.Awake();
			if ((_carouselItems == null || _carouselItems.Count == 0) && _display != null && _display.childCount > 0)
			{
				_carouselItems = new List<GameObject>();
				foreach (Transform child in _display)
				{
					_carouselItems.Add(child.gameObject);
				}
			}
			if (_carouselItems == null || _carouselItems.Count == 0)
			{
				Debug.LogWarning($"No carousel items assigned in CarouselSelector");
			}
			else
			{
				if(value != null || !_carouselItems.Contains(value))
					Set(0);
				else
				{
					var index = _carouselItems.IndexOf(value);
					Set(index);
				}
			}
		}

		public void SetNext()
		{
			if (_carouselItems == null || _carouselItems.Count == 0) return;
			// Increment the index, wrapping around if needed
			Set((_currentIndex + 1) % _carouselItems.Count);
		}

		public void SetPrevious()
		{
			if (_carouselItems == null || _carouselItems.Count == 0) return;
			// Decrement the index, wrapping around if needed
			Set((_currentIndex - 1 + _carouselItems.Count) % _carouselItems.Count);
		}

		public void Set(int value)
		{
			_currentIndex = value;
			UpdateDisplay();
			_OnIndexValueChanged?.Invoke(_currentIndex);
			_OnValueChanged?.Invoke(_carouselItems[_currentIndex]);
		}
		
		private void UpdateDisplay()
		{
			// Update the UI text to display the current value
			if (_display != null)
			{
				for (var index = 0; index < _carouselItems.Count; index++)
				{
					_carouselItems[index].SetActive(_currentIndex == index);
				}
			}
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			if (_nextButton != null &&
			    RectTransformUtility.RectangleContainsScreenPoint(_nextButton, eventData.pointerPressRaycast.screenPosition,
				    eventData.enterEventCamera))
			{
				SetNext();
			}
			else if (_previousButton != null &&
			         RectTransformUtility.RectangleContainsScreenPoint(_previousButton, eventData.pointerPressRaycast.screenPosition,
				         eventData.enterEventCamera))
			{
				SetPrevious();
			}
		}

		public override void OnMove(AxisEventData eventData)
		{
			if (!IsActive() || !IsInteractable())
			{
				base.OnMove(eventData);
				return;
			}

			switch (eventData.moveDir)
			{
				case MoveDirection.Left:
					if (FindSelectableOnLeft() == null)
						SetPrevious();
					else
						base.OnMove(eventData);
					break;
				case MoveDirection.Right:
					if (FindSelectableOnRight() == null)
						SetNext();
					else
						base.OnMove(eventData);
					break;
				default:
					base.OnMove(eventData);
					break;
			}
		}
		
		[Serializable]
		/// <summary>
		/// Event type used by the UI.Slider.
		/// </summary>
		public class CarouselEventIndex : UnityEvent<int> {}
		
		[Serializable]
		/// <summary>
		/// Event type used by the UI.Slider.
		/// </summary>
		public class CarouselEventGameObject : UnityEvent<GameObject> {}
	}
}