using System.Text;
using GameIdea2.Compression;
using GameIdea2.CustomPlay;
using GameIdea2.Gameloop;
using GameIdea2.Scripts.Editor;
using GameIdea2.Scripts.MapEditor;
using GameIdea2.UI;
using GravityWell.Core.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GameIdea2
{
    public class EditModeController : MonoBehaviour
    {
        
        private enum Interaction
        {
            Undefined=0,
            None,
            Pan,
            Move, 
            Scale
        }

        [SerializeField] private Api api;
        [SerializeField] private EditorCursors editorCursors;
        [SerializeField] private EditmodeGUI gui;
        [SerializeField] private float panSensitivity = 10;
        [SerializeField] private float scaleSensitivity = 10;
        [SerializeField] private float zoomSensitivity = 10;
        [SerializeField] private float minZoom = 50;
        [SerializeField] private float maxZoom = 250;
        
        public static Camera ReferenceCamera;
        
        private const int PAN_MOUSE_BTN = 2;
        private const int MOVE_MOUSE_BTN = 0;
        private const int SCALE_MOUSE_BTN = 1;
        private bool panningBlocked = false;
        
        private Interaction currentInteraction = Interaction.Undefined;

        private GameObject currentWorkspace;
        
        private void Start()
        {
            if (!ReferenceCamera)
                ReferenceCamera = Camera.main;

            if(!gui)
                gui = GetComponent<EditmodeGUI>();
            
            SetCurrentInteraction(Interaction.None);
            currentWorkspace = Universe.Instance.GetWorkspace();
            if (!currentWorkspace)
                currentWorkspace = Universe.Instance.CreateWorkspace();

            HUDManager.instance.OnUploadClicked.AddListener(SaveLevel);
        }

        private Vector2 panInput;
        private Vector2 scrollInput;
        public void OnNaivgationChanged(InputAction.CallbackContext context)
        {
            panInput = context.ReadValue<Vector2>();
        }
        public void OnScrollChanged(InputAction.CallbackContext context)
        {
            Debug.Log($"context {context.ReadValue<Vector2>()}");
            scrollInput = context.ReadValue<Vector2>();
        }

        private bool IsUIOverGUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
        
        private void Update()
        {
            if(IsUIOverGUI())
                return;
            
            UpdateCamera();
            
            if(Universe.Instance.Simulate)
                return;
            
            ManageSelection();
            MoveSelection();
            ScaleSelection();
        }

        private void SetCursor(Interaction type)
        {
            if(!editorCursors)
                return;
            
            Texture2D cursorIco = null;
            switch (type)
            {
                case Interaction.None:
                    cursorIco = editorCursors.DefaultCursor;
                    break;
                case Interaction.Pan:
                    cursorIco = editorCursors.PanCursor;
                    break;
                case Interaction.Move:
                    cursorIco = editorCursors.MoveCursor;
                    break;
                case Interaction.Scale:
                    cursorIco = editorCursors.ScaleCursor;
                    break;
            }
            
            if (!cursorIco)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                return;
            }
            
            Cursor.SetCursor(cursorIco, currentInteraction == Interaction.None?Vector2.zero : Vector2.one*0.5f, CursorMode.ForceSoftware);
        }

        private bool InteractionAvailable()
        {
            return currentInteraction == Interaction.None || currentInteraction == Interaction.Undefined;
        }

        private void SetCurrentInteraction(Interaction myInteraction)
        {
            if (InteractionAvailable() && currentInteraction != myInteraction)
            {
                currentInteraction = myInteraction;
                SetCursor(myInteraction);
            }
        }
        
        private void TryResetInteraction(Interaction myInteraction)
        {
            if (currentInteraction == myInteraction)
            {
                currentInteraction = Interaction.None;
                SetCursor(Interaction.None);
            }
        }
        
        private void UpdateCamera()
        {
            var zoomDelta = -InputManager.ZoomInput * zoomSensitivity;
            // var zoomDelta = -scrollInput.y * zoomSensitivity;
            if (zoomDelta != 0)
                HUDManager.instance.Interacted = true;
            
            ReferenceCamera.orthographicSize += zoomDelta;
            ReferenceCamera.orthographicSize = Mathf.Clamp(ReferenceCamera.orthographicSize, minZoom, maxZoom);

            var xAxis = InputManager.MoveInput.x;
            var yAxis = InputManager.MoveInput.y;

            if ((yAxis != 0 || xAxis != 0) && !InputManager.PanCamera)
            {
                HUDManager.instance.Interacted = true;
                var pD = new Vector3(xAxis, yAxis) * -1f;
                var tD = new Vector3(-pD.x * Time.deltaTime * panSensitivity, 0, -pD.y * panSensitivity * Time.deltaTime);
                ReferenceCamera.transform.position += tD;
                return;
            }
            
            if (!InputManager.PanCamera)
            {
                TryResetInteraction(Interaction.Pan);
                return;
            }

            if (InputManager.PanCamera && InteractionAvailable())
            {
                HUDManager.instance.Interacted = true;
                SetCurrentInteraction(Interaction.Pan);
            }
            
            var panDelta = InputManager.MousePositionDelta;
            var translationDelta = new Vector3(-panDelta.x * Time.deltaTime * panSensitivity, 0, -panDelta.y * panSensitivity * Time.deltaTime);
            ReferenceCamera.transform.position += translationDelta;
        }
        
        

        private void ManageSelection()
        {
            if (InputManager.SelectItem || InputManager.ScaleItem)
            {
                if(!InteractionAvailable())
                    return;
                
                if(Editable.CurrentSelection)
                    return;
                
                RaycastHit hit;
                var ray = ReferenceCamera.ScreenPointToRay(InputManager.MousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if(!hit.collider)
                        return;
                    var editable = hit.collider.GetComponentInChildren<Editable>();
                    if(!editable)
                        return;
                    HUDManager.instance.Interacted = true;
                    editable.Select();
                    if (InputManager.SelectItem)
                    {
                        SetCurrentInteraction(Interaction.Move);
                    }
                    else if (InputManager.ScaleItem)
                    {
                        SetCurrentInteraction(Interaction.Scale);
                    }
                }
            }
            else
            {
                if (Editable.CurrentSelection)
                {
                    Editable.CurrentSelection.Deselect();
                }
            }
        }

        public static Vector3 GetMousePosition(Camera camera)
        {
            var pos = InputManager.MousePosition;
            if (camera.orthographic)
                return InputManager.MousePosition;
            
            return new Vector3(pos.x, pos.y, camera.transform.position.y);
        }
        
        private void ScaleSelection()
        {
            var selected = Editable.CurrentSelection;
           
            if (!selected)
            {
                if( currentInteraction==Interaction.Scale)
                    TryResetInteraction(Interaction.Scale);
                return;
            }

            var scaleHeler = selected.GetComponent<ScaleHelper>();
            if (!scaleHeler)
            {
                if( currentInteraction==Interaction.Scale)
                    TryResetInteraction(Interaction.Scale);
                return;
            }
            
            if (InputManager.ScaleItem)
            {
                /*var pos = camera.ScreenToWorldPoint(GetMousePosition());
                var mouseWorldPos = new Vector3(pos.x, 0, pos.y);
                var scaleDir = (mouseWorldPos - mouseStartWorldPos).normalized.x;*/

                var mouseDelta = InputManager.MousePositionDelta.normalized;
                
                var scaleDelta = mouseDelta.x * scaleSensitivity;
                //Debug.Log($"Scale Delta : {scaleDelta}");
                var currentScale = selected.transform.localScale;
                currentScale += Vector3.one * (scaleDelta * Time.deltaTime);
                
                if (currentScale.x > 0.5f && currentScale.x < 1f)
                    currentScale = Vector3.one;
                else if (currentScale.x > scaleHeler.MaxScale)
                    currentScale = Vector3.one * scaleHeler.MaxScale;

                var dirty = Vector3.Distance(selected.transform.localScale, currentScale) >= 0.01f;
                selected.transform.localScale = currentScale;
                if(dirty)
                    Universe.Instance.MarkDirty(selected.gameObject);
            }
        }
        
        private void MoveSelection()
        {
            var selected = Editable.CurrentSelection;
            if (!selected)
            {
                if( currentInteraction==Interaction.Move)
                    TryResetInteraction(Interaction.Move);
                return;
            }

            if (InputManager.SelectItem)
            {
                var worldPos = TransformMouseToWorld(ReferenceCamera, GetMousePosition(ReferenceCamera));
                var newPos = new Vector3(worldPos.x, 0, worldPos.z);

                var dirty = !Mathf.Approximately(Vector3.Distance(selected.transform.position, newPos), 0);
                selected.transform.position = newPos;
                if(dirty)
                    Universe.Instance.MarkDirty(selected.gameObject);
            }
        }

        public static Vector3 TransformMouseToWorld(Camera camera, Vector3 mousePos)
        {
            var worldPos = camera.ScreenToWorldPoint(mousePos);
            return new Vector3(worldPos.x, 0, worldPos.z);
        }

        public void SpawnTerrestial(string key)
        {
            SpawnTerrestial(key,
                new Vector3(ReferenceCamera.transform.position.x, 0, ReferenceCamera.transform.position.z));
        }        
        
        public void  SpawnTerrestial(string key, Vector3 position)
        {
            if (!currentWorkspace)
            {
                currentWorkspace = Universe.Instance.GetWorkspace();
                if (!currentWorkspace)
                    currentWorkspace = Universe.Instance.CreateWorkspace();
            }
            
            
            HUDManager.instance.Interacted = true;
            var asset = Resources.Load<GameObject>(key);
            if (asset)
            {
                var go = Instantiate(asset, position,
                    Quaternion.identity, currentWorkspace.transform);
                go.AddComponent<Editable>();
                go.AddComponent<Spawned>();
                Universe.Instance.MarkDirty(go);
            }
        }

        public void ResetCamera()
        {
            Universe.Instance.ResetCamera();
        }

        public void ResetLevel()
        {
            GameManager.Instance.RestartLevel();
        }

        public void SaveLevel(string name, string author)
        {
            var json = Universe.Instance.SerializeLevel(name, author);
            StartCoroutine(MongoHelper.InsertLevelData(api.UploadLevelAPI, name, author,json, OnUploadComplete, OnUploadFailed));
        }

        private void OnUploadComplete()
        {
            HUDManager.instance.ToggleUploadWindow();
            Debug.Log("Upload Success");
        }
        
        private void OnUploadFailed()
        {
            HUDManager.instance.ToggleUploadWindow();
            Debug.Log("Upload Failed");
        }
    }
}