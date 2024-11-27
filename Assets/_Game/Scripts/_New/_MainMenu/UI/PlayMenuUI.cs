using GravityWell.UI;
using UnityEngine;

namespace GravityWell.MainMenu
{
    public class PlayMenuUI : MenuUI
    {
        
        // Lot of Temp hack in this script for IDGC 24 build
        
        private IMenuHandler _mainMenu;
        
        
        [SerializeField] private GameObject continueButton;

        
        public override void Initialize(IMenuHandler handler)
        {
            Debug.Log($"PlayMenuUI Initializing -> isMain: {IsMain}");
            base.Initialize(handler);
        }

        public override void Enable()
        {
            if(SettingsUIHandler.Instance != null)
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
