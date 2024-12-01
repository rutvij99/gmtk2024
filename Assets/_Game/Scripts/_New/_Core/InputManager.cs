using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Serialization;


namespace GravityWell.Core.Input
{
    public enum ControlType
    {
        Keyboard,
        Gamepad
    }
    [DefaultExecutionOrder(-100)]
    public class InputManager : MonoBehaviour
    {
        private const string SCRIPT_NAME = "InputManager";
        public static InputManager Instance { get; private set; }
        
        public static event Action<ControlType> OnControlChanged;

        [SerializeField] private string MapID = "Game";
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private InputSystemUIInputModule _uiInputModule;
        [SerializeField] private PlayerInput _playerInput;

        private ControlType _controlType;
        
        public static ControlType ControlType => Instance._controlType;


        #region Input Actions
        private InputActionMap actionMap;
        private InputAction submitAction;
        private InputAction selectAction;
        private InputAction scaleAction;
        private InputAction panAction;
        
        private InputAction mousePosition;
        private InputAction mousePositionDelta;
        
        private InputAction moveAction;
        private InputAction zoomAction;

        #endregion
        
        #region Input Values
        // todo: Full cleanup and streamlining on how all input data in handled and passed
        public static event Action OnNavigatePerformed;
        public static event Action OnCancelPressed;
        
        public static bool SubmitStarted => Instance.submitAction.WasPressedThisFrame();
        public static bool SelectItemStarted => Instance.selectAction.WasPressedThisFrame();
        public static bool SelectItemEnded => Instance.selectAction.WasReleasedThisFrame();
        public static bool SelectItem => Instance.selectAction.IsPressed();
        public static bool ScaleItem => Instance.scaleAction.IsPressed();
        public static bool PanCamera => Instance.panAction.IsPressed();
        
        public static float ZoomInput { get; private set; }
        public static Vector2 MoveInput { get; private set; }
        
        // Keyboard Only
        public static Vector2 MousePosition { get; private set; }
        public static Vector2 MousePositionDelta { get; private set; }
        

        #endregion
        
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(Instance.gameObject);
            }
            Instance = this;

            if(_eventSystem == null)
                _eventSystem = GetComponentInChildren<EventSystem>();
            if(_uiInputModule == null)
                _uiInputModule = GetComponentInChildren<InputSystemUIInputModule>();
            if(_playerInput == null)
                _playerInput = GetComponentInChildren<PlayerInput>();
            
            if (_playerInput == null) return;
            actionMap = _playerInput.actions.FindActionMap(MapID);
            if (actionMap == null) return;
            //Input actions
            moveAction = actionMap.FindAction("Move");
            zoomAction = actionMap.FindAction("ScrollWheel");
            mousePosition = actionMap.FindAction("Point");
            mousePositionDelta = actionMap.FindAction("PositionDelta");

            submitAction = actionMap.FindAction("Submit");
            selectAction = actionMap.FindAction("Click");
            scaleAction = actionMap.FindAction("RightClick");
            panAction = actionMap.FindAction("MiddleClick");
            actionMap.FindAction("Navigate").performed += context => OnNavigatePerformed?.Invoke();
            actionMap.FindAction("Cancel").performed += context => OnCancelPressed?.Invoke();
        }

        private void OnEnable()
        {
            RegisterMethods();
        }

        private void OnDisable()
        {
            UnregisterMethods();
        }

        public void RegisterMethods()
        {
            if (_playerInput == null) return;
            _playerInput.onControlsChanged += OnControlsChanged;
            _playerInput.onDeviceLost += OnDeviceLost;
            _playerInput.onDeviceRegained += OnDeviceRegained;
            
        }
        
        public void UnregisterMethods()
        {
            if (_playerInput == null) return;
            _playerInput.onControlsChanged -= OnControlsChanged;
            _playerInput.onDeviceLost -= OnDeviceLost;
            _playerInput.onDeviceRegained -= OnDeviceRegained;
        }

        private void OnDeviceRegained(PlayerInput obj)
        {
            
        }

        private void OnDeviceLost(PlayerInput obj)
        {
            
        }

        private void OnControlsChanged(PlayerInput obj)
        {
            switch (obj.currentControlScheme)
            {
                case "Keyboard&Mouse":
                    _controlType = ControlType.Keyboard;
                    break;
                case "Gamepad":
                    _controlType = ControlType.Gamepad;
                    break;
            }
            // _controlType =  ControlType.Gamepad
            Debug.Log($"[{SCRIPT_NAME}] OnControlsChanged: {obj.currentControlScheme}");
            OnControlChanged?.Invoke(_controlType);
        }


        private void Update()
        {
            MoveInput = moveAction.ReadValue<Vector2>();
            MousePosition = mousePosition.ReadValue<Vector2>();
            MousePositionDelta = mousePositionDelta.ReadValue<Vector2>();
            ZoomInput = zoomAction.ReadValue<Vector2>().y;
        }
    }
}
