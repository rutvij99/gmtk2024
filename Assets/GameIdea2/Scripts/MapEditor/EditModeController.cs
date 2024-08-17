using System;
using GameIdea2.Scripts.Editor;
using GameIdea2.UI;
using UnityEngine;

namespace GameIdea2
{
    public class EditModeController : MonoBehaviour
    {
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
        
        private Vector3 mouseWorldStartPos = Vector3.zero;
        private bool panning = false;
        private float prevScaleDist = 0;
        
        private void Start()
        {
            if (!camera)
                camera = Camera.main;

            gui = GetComponent<EditmodeGUI>();
        }

        private void Update()
        {
            UpdateCamera();
            
            if(Universe.Instance.Simulate)
                return;
            
            ManageSelection();
            MoveSelection();
            ScaleSelection();
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
                panning = false;
                return;
            }

            if (Input.GetMouseButton(PAN_MOUSE_BTN) && !panning)
            {
                gui.Interacted = true;
                panning = true;
            }
            
            var panDelta = Input.mousePositionDelta;
            var translationDelta = new Vector3(-panDelta.x * Time.deltaTime * panSensitivity, 0, -panDelta.y * panSensitivity * Time.deltaTime);
            camera.transform.position += translationDelta;
        }

        private void ManageSelection()
        {
            if (Input.GetMouseButton(MOVE_MOUSE_BTN) || Input.GetMouseButton(SCALE_MOUSE_BTN))
            {
                if(Editable.CurrentSelection)
                    return;
                
                var worldPos = camera.ScreenToWorldPoint(GetMousePosition());
                var mouseWorldRawPos = new Vector3(worldPos.x, 0, worldPos.z);
                mouseWorldStartPos = new Vector3(mouseWorldRawPos.x, 0, mouseWorldRawPos.z);
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
                }
            }
            else
            {
                if(Editable.CurrentSelection)
                    Editable.CurrentSelection.Deselect();
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
            if(!selected)
                return;
            if (Input.GetMouseButton(SCALE_MOUSE_BTN))
            {
                var pos = camera.ScreenToWorldPoint(GetMousePosition());
                var mouseWorldPos = new Vector3(pos.x, 0, pos.y);
                var dist = Vector3.Distance(mouseWorldPos, selected.transform.position);
                var scaledDist = dist * scaleSensitivity;
                var currentScale = selected.transform.localScale;
                if (prevScaleDist > scaledDist)
                    currentScale -= Vector3.one * (scaledDist * Time.deltaTime);
                else if(prevScaleDist < scaledDist)
                {
                    currentScale += Vector3.one * (scaledDist * Time.deltaTime);
                }

                if (currentScale.x > 0.5f && currentScale.x < 1f)
                    currentScale = Vector3.one;
                else if (currentScale.x > 100)
                    currentScale = Vector3.one * 100;

                selected.transform.localScale = currentScale;
                prevScaleDist = scaledDist;
            }
        }
        
        private void MoveSelection()
        {
            var selected = Editable.CurrentSelection;
            if(!selected)
                return;
            if (Input.GetMouseButton(MOVE_MOUSE_BTN))
            {
                var worldPos = camera.ScreenToWorldPoint(GetMousePosition());
                var newPos = new Vector3(worldPos.x, 0, worldPos.z);
                
                selected.transform.position = newPos;
            }
        }
        
        public void  SpawnTerrestial(string key)
        {
            gui.Interacted = true;
            var asset = Resources.Load<GameObject>(key);
            if (asset)
            {
                Instantiate(asset, new Vector3(camera.transform.position.x, 0, camera.transform.position.z),
                    Quaternion.identity);
            }
        }
    }
}