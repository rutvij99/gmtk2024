using System;
using GravityWell.UI;
using UnityEngine;

namespace GravityWell.MainMenu
{
    public class MainMenuUI : MenuUI
    {
        public bool IsMain => false;
        
        private IMenuHandler _mainMenu;
        
        public override void Initialize(IMenuHandler handler)
        {
            Debug.Log($"MainMenuUI Initializing -> isMain: {IsMain}");
            base.Initialize(handler);
        }

        public override void Enable()
        {
            if(SettingsUIHandler.Instance != null)
                SettingsUIHandler.Instance.ShowContextMenu(true);
            ShowUI(true);
            SelectFirstElement();
        }

        public override void Disable()
        {
            if (SettingsUIHandler.Instance != null)
                SettingsUIHandler.Instance.ShowContextMenu(false);
            ShowUI(false);
        }
        
        public void ShowCredits()
        {
            // _handler.OpenMenu(settingsUI);
        }

        public void ExitGame()
        {
            _handler.ExitGame();
        }
    }
}
