using System;
using System.Collections;
using System.Collections.Generic;
using GravityWell.Core.Config;
using GravityWell.Common.Helpers;
using GravityWell.Core.Input;
using GravityWell.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GravityWell.MainMenu
{
    public class MainMenuManager : MonoBehaviour, IMenuHandler
    {
        [SerializeField] private CanvasGroup mainMenuCanvas;
        
        private Stack<MenuUI> menuStack = new Stack<MenuUI>();
        private List<MenuUI> menus;
        private MenuUI stackRoot;
        
        
        private PlayerInput playerInput;
        public PlayerInput PlayerInput => playerInput;

        protected void Awake()
        {
            // Cursor.visible = false;
            // Cursor.lockState = CursorLockMode.Locked;
            playerInput = GetComponent<PlayerInput>();
            List<MenuUI> menuList = new List<MenuUI>();
            mainMenuCanvas.GetComponentsInChildren(true, menuList);
            
            menus = new List<MenuUI>();
            foreach (var menu in menuList)
            {
                menus.Add(menu);
                menu.Initialize(this);
                menu.Disable();
                if (menu.IsMain)
                {
                    stackRoot = menu;
                }
            }
        }

        private void OnEnable()
        {
            InputManager.OnNavigatePerformed += OnNavigationValueChanged;
        }

        private void OnDisable()
        {
            InputManager.OnNavigatePerformed -= OnNavigationValueChanged;
        }

        private void OnNavigationValueChanged()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                menuStack.Peek().SelectFirstElement();
            }
        }

        private void Start()
        {
            // InputProvider.Input.Menu.SetCallbacks(this);
            // InputProvider.Input.Menu.Enable();

            if (menus == null)
            {
                Debug.Log($"No menu UI found in scene.");
                return;
            }
            OpenMenu(stackRoot);
            
            // show preloading updates
            // allow entry
            // mainmenu
            // free flow
        }

        private IEnumerator EnterStateRoutine()
        {
            yield break;
        }
        

        void IMenuHandler.ExitGame()
        {
            Core.Config.GameConfig.ExitGame();
        }

        public void OpenMenu(MenuUI menu)
        {
            if (menuStack.Count > 0)
            {
                menuStack.Peek().Disable();
            }
            menuStack.Push(menu);
            menu.Enable();
        }

        public void CloseMenu()
        {
            if (menuStack.Count > 0)
            {
                if (menuStack.Peek().IsMain)
                {
                    menuStack.Peek().Enable();
                    return;
                }
                var topMenu = menuStack.Pop();
                topMenu.Disable();
                if (menuStack.Count > 0)
                {
                    menuStack.Peek().Enable(); // Reopen or show the new top menu
                }
            }
        }
    }
}
