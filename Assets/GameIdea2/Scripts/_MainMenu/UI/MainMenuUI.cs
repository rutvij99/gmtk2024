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
            ShowUI(true);
            SelectFirstElement();
        }

        public override void Disable()
        {
            ShowUI(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _handler.CloseMenu();
            }
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
