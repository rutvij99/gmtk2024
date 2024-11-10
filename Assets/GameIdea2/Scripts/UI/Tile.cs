using System;
using Michsky.MUIP;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameIdea2.Audio._UI
{
    public class Tile : MonoBehaviour
    {
        private static bool _isDragging = false;
        private static GameObject _dragObj = null;
        private static bool _validPosition;
        
        private TileData data;
        private EventTrigger eventTrigger;
        private SphereCollider validityCollider;

        [SerializeField] private LayerMask mask;
        
        private void Start()
        {
            data = GetComponent<TileData>();
            eventTrigger = GetComponent<EventTrigger>();

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            EventTrigger.Entry clickEntry = new EventTrigger.Entry();
            clickEntry.eventID = EventTriggerType.PointerClick;
            clickEntry.callback.AddListener(OnTileClick);
            
            EventTrigger.Entry onDragBegin = new EventTrigger.Entry();
            onDragBegin.eventID = EventTriggerType.BeginDrag;
            onDragBegin.callback.AddListener(OnTileDragBegin);
            
            EventTrigger.Entry onDragEnd = new EventTrigger.Entry();
            onDragEnd.eventID = EventTriggerType.EndDrag;
            onDragEnd.callback.AddListener(OnTileDragEnd);
            
            EventTrigger.Entry onDragging = new EventTrigger.Entry();
            onDragging.eventID = EventTriggerType.Drag;
            onDragging.callback.AddListener(OnTileDrag);
            
            //eventTrigger.triggers.Add(clickEntry);
            eventTrigger.triggers.Add(onDragBegin);
            eventTrigger.triggers.Add(onDragEnd);
            eventTrigger.triggers.Add(onDragging);
        }

        private void OnTileClick(BaseEventData e)
        {
            HUDManager.instance.ButtonTerrestrialBodySpawn(data.PrefabName);
        }

        private void OnTileDragBegin(BaseEventData e)
        {
            if(_isDragging)
                return;
            
            _isDragging = true;
            _dragObj = GetObjectRepresentation();
            if(!_dragObj)
                return;
            validityCollider = _dragObj.GetComponent<SphereCollider>();
            Debug.Log("Drag Start");
        }

        private GameObject GetObjectRepresentation()
        {
            return Instantiate(Resources.Load<GameObject>(data.DragRepresentator));
        }
        
        private void OnTileDrag(BaseEventData e)
        {
            if (!_isDragging) return;
            if(!_dragObj) return;
            Debug.Log("Dragging");
            var pointerData = (PointerEventData)e;
            if(pointerData == null) return;
            _dragObj.transform.position =
                EditModeController.TransformMouseToWorld(EditModeController.ReferenceCamera, EditModeController.GetMousePosition(EditModeController.ReferenceCamera));

            if (validityCollider)
            {
                var col = Physics.OverlapSphere(_dragObj.transform.position, validityCollider.radius, mask);
                _validPosition = col.Length <= 0;
                _dragObj.transform.Find("Valid").gameObject.SetActive(_validPosition);
                _dragObj.transform.Find("InValid").gameObject.SetActive(!_validPosition);
            }
        }
        
        private void OnTileDragEnd(BaseEventData e)
        {
            if(!_isDragging)
                return;
            
            _isDragging = false;
            if(_dragObj)
                Destroy(_dragObj);
            Debug.Log("Drag End");
            var pointerData = (PointerEventData)e;
            if(pointerData == null) return;
            var pos = EditModeController.TransformMouseToWorld(EditModeController.ReferenceCamera, EditModeController.GetMousePosition(EditModeController.ReferenceCamera));
            var controllerInst = FindFirstObjectByType<EditModeController>();
            if(!controllerInst)
                return;
            
            if(_validPosition)
                controllerInst.SpawnTerrestial(data.PrefabName, pos);
        }
    }
}