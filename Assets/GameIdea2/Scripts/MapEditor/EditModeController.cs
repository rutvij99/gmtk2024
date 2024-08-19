using System;
using GameIdea2.Gameloop;
using GameIdea2.Scripts.Editor;
using GameIdea2.Scripts.MapEditor;
using GameIdea2.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

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

        [SerializeField] private EditorCursors editorCursors;
        [SerializeField] private EditmodeGUI gui;
        [SerializeField] private Camera camera;
        [SerializeField] private float panSensitivity = 10;
        [SerializeField] private float scaleSensitivity = 10;
        [SerializeField] private float zoomSensitivity = 10;
        [SerializeField] private float minZoom = 50;
        [SerializeField] private float maxZoom = 250;
        
        private const int PAN_MOUSE_BTN = 2;
        private const int MOVE_MOUSE_BTN = 0;
        private const int SCALE_MOUSE_BTN = 1;
        
        private bool panningBlocked = false;

        private Vector3 mouseStartWorldPos;
        private Interaction currentInteraction = Interaction.Undefined;

        private GameObject currentWorkspace;
        
        private void Start()
        {
            if (!camera)
                camera = Camera.main;

            if(!gui)
                gui = GetComponent<EditmodeGUI>();
            
            SetCurrentInteraction(Interaction.None);

            currentWorkspace = Universe.Instance.GetWorkspace();
            if (!currentWorkspace)
                currentWorkspace = Universe.Instance.CreateWorkspace();
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
            var zoomDelta = -Input.mouseScrollDelta.y * zoomSensitivity;
            if (zoomDelta != 0)
                gui.Interacted = true;
            
            camera.orthographicSize += zoomDelta;
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);
            
            if (!Input.GetMouseButton(PAN_MOUSE_BTN))
            {
                TryResetInteraction(Interaction.Pan);
                return;
            }

            if (Input.GetMouseButton(PAN_MOUSE_BTN) && InteractionAvailable())
            {
                gui.Interacted = true;
                SetCurrentInteraction(Interaction.Pan);
            }
            
            var panDelta = Input.mousePositionDelta;
            var translationDelta = new Vector3(-panDelta.x * Time.deltaTime * panSensitivity, 0, -panDelta.y * panSensitivity * Time.deltaTime);
            camera.transform.position += translationDelta;
        }

        private void ManageSelection()
        {
            if (Input.GetMouseButton(MOVE_MOUSE_BTN) || Input.GetMouseButton(SCALE_MOUSE_BTN))
            {
                if(!InteractionAvailable())
                    return;
                
                if(Editable.CurrentSelection)
                    return;
                
                RaycastHit hit;
                var ray = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if(!hit.collider)
                        return;
                    var editable = hit.collider.GetComponentInChildren<Editable>();
                    if(!editable)
                        return;
                    gui.Interacted = true;
                    editable.Select();
                    var pos = camera.ScreenToWorldPoint(GetMousePosition());
                    mouseStartWorldPos = new Vector3(pos.x, 0, pos.y);
                    if (Input.GetMouseButton(MOVE_MOUSE_BTN))
                    {
                        SetCurrentInteraction(Interaction.Move);
                    }
                    else if (Input.GetMouseButton(SCALE_MOUSE_BTN))
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

        private Vector3 GetMousePosition()
        {
            var pos = Input.mousePosition;
            if (camera.orthographic)
                return Input.mousePosition;
            
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
            
            if (Input.GetMouseButton(SCALE_MOUSE_BTN))
            {
                /*var pos = camera.ScreenToWorldPoint(GetMousePosition());
                var mouseWorldPos = new Vector3(pos.x, 0, pos.y);
                var scaleDir = (mouseWorldPos - mouseStartWorldPos).normalized.x;*/

                var mouseDelta = Input.mousePositionDelta.normalized;
                
                var scaleDelta = mouseDelta.x * scaleSensitivity;
                //Debug.Log($"Scale Delta : {scaleDelta}");
                var currentScale = selected.transform.localScale;
                currentScale += Vector3.one * (scaleDelta * Time.deltaTime);
                
                if (currentScale.x > 0.5f && currentScale.x < 1f)
                    currentScale = Vector3.one;
                else if (currentScale.x > scaleHeler.MaxScale)
                    currentScale = Vector3.one * scaleHeler.MaxScale;

                selected.transform.localScale = currentScale;
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

            if (Input.GetMouseButton(MOVE_MOUSE_BTN))
            {
                var worldPos = camera.ScreenToWorldPoint(GetMousePosition());
                var newPos = new Vector3(worldPos.x, 0, worldPos.z);
                
                selected.transform.position = newPos;
            }
        }
        
        public void  SpawnTerrestial(string key)
        {
            if (currentWorkspace == null)
                currentWorkspace = new GameObject();
            
            gui.Interacted = true;
            var asset = Resources.Load<GameObject>(key);
            if (asset)
            {
                var go = Instantiate(asset, new Vector3(camera.transform.position.x, 0, camera.transform.position.z),
                    Quaternion.identity, currentWorkspace.transform);
                go.AddComponent<Editable>();
            }
            
            Debug.Log($"Json: {Universe.Instance.SerializeLevel("test-name","test-author")}");
        }

        public void ResetCamera()
        {
            Universe.Instance.ResetCamera();
        }

        public void ResetLevel()
        {
            GameManager.Instance.RestartLevel();
        }
        
    }
}