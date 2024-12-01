using System;
using System.Collections.Generic;
using GravityWell.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GravityWell.MainMenu
{
	public class LevelMenuUI : MenuUI
	{
		// Lot of Temp hack in this script for IDGC 24 build

		private IMenuHandler _mainMenu;
		
		[SerializeField] private GameObject levelButtonPrefab;
		[SerializeField] private ScrollRect scrollRect;
		[SerializeField] private Transform levelSelectHolder;
		[SerializeField] private List<GameObject> levels;

		private GameObject lastSelected;


		private void Awake()
		{
			levelButtonPrefab.SetActive(true);
			var lastUnlocked = GameConfig.MAX_LEVELS;
			for (int i = 0; i < GameConfig.MAX_LEVELS; i++)
			{
				var obj = Instantiate(levelButtonPrefab, levelSelectHolder);
				var btn = obj.GetComponentInChildren<AdvancedButton>();
				btn.GetComponent<LabelField>().SetText($"Level {i + 1}");
				var level = i;
				if (i > lastUnlocked)
				{
					btn.isEnabled = false;
				}
				else
				{
					btn.onClick.AddListener(()=>
					{
						this.LoadLevel(level);
					});
				}
				levels.Add(btn.gameObject);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(levelSelectHolder.GetComponent<RectTransform>());
			Destroy(levelButtonPrefab.gameObject);
		}

		public override void Initialize(IMenuHandler handler)
		{
			Debug.Log($"PlayMenuUI Initializing -> isMain: {IsMain}");
			base.Initialize(handler);
			
		}

		public override void Enable()
		{
			if (SettingsUIHandler.Instance != null)
				SettingsUIHandler.Instance.ShowContextMenu(true);
			ShowUI(true);
			SelectFirstElement();
			// hack to show or hide continue button from old config data
		}

		public override void Disable()
		{
			if (SettingsUIHandler.Instance != null)
				SettingsUIHandler.Instance.ShowContextMenu(false);
			ShowUI(false);
		}

		private void Update()
		{
			if (!IsEnabled) return;
			// In unity if the selected button is inside a scroll view and it is masked and not visible in the viewport
			// then scroll till it is inside viewport
			
			// Check if the currently selected GameObject has changed
			if (EventSystem.current.currentSelectedGameObject != lastSelected)
			{
				lastSelected = EventSystem.current.currentSelectedGameObject;
				// Ensure the selected GameObject is within the ScrollRect content
				if (lastSelected != null && lastSelected.transform.IsChildOf(scrollRect.content))
				{
					RectTransform target = lastSelected.GetComponent<RectTransform>();
					if (target != null)
					{
						EnsureVisibleY(target);
					}
				}
			}
		}

		public void EnsureVisibleY(RectTransform target)
		{
			var targetIndex = target.GetSiblingIndex();
			var count = ActiveChildCount(target.parent) - 1;
			scrollRect.verticalScrollbar.value = Mathf.Clamp(1 - (targetIndex / (float)count), 0, 1);

			// Debug.Log($"EnsureVisibleY({targetIndex}, {count}) -> {scrollRect.verticalScrollbar.value}");
		}

		private int ActiveChildCount(Transform parent)
		{
			// return parent.childCount;
			var count = 0;
			foreach (Transform child in parent)
			{
				if(child.gameObject.activeSelf) count++;
			}
			return count;
		}

		public void LoadLevelMenu()
		{
		}

		public void LoadCommunitySelectScene()
		{
			GameConfig.LoadLevel("CustomLevelSelector");
		}

		public void LoadLevelEditor()
		{
			GameConfig.LoadLevel("LevelEditor");
		}

		public void LoadLastestLevel()
		{
			GameConfig.LoadLevel(GameConfig.GetLastCompletedLevel());
		}

		public void LoadLevel(int i)
		{
			Debug.Log($"Loading Level {i}");
			// AudioManager.Instance?.ChangeBackgroundMusic(i);
			GameConfig.LoadLevel(i);
		}

		public void LoadFreePlay()
		{
			GameConfig.LoadLevel($"FreePlay");
		}
	}
}