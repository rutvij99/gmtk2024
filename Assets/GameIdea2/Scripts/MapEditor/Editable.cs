using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GameIdea2.Scripts.Editor
{
    public class Editable : MonoBehaviour
    {
        [SerializeField] private GameObject SelectedGUI;
        
        private List<GameObject> HideList;
        public static Editable CurrentSelection;
        private void Start()
        {
            HideList = new List<GameObject>();
            SelectedGUI = transform.Find("Selected")?.gameObject;
            SelectedGUI?.SetActive(false);
            foreach (Transform child in transform)
            {
                if(!child)
                    continue;
                
                if(!child.name.EndsWith("HideMeWhenSelected"))
                    continue;
                
                if(!HideList.Contains(child.gameObject))
                    HideList.Add(child.gameObject);
            }
        }

        public void Deselect()
        {
            if (CurrentSelection == this)
                CurrentSelection = null;
            
            SelectedGUI?.SetActive(false);
            foreach (var go in HideList)
            {
                go.SetActive(true);
            }
        }
        
        public void Select()
        {
            if(CurrentSelection)
                CurrentSelection.Deselect();

            CurrentSelection = this;
            SelectedGUI?.SetActive(true);
            foreach (var go in HideList)
            {
                go.SetActive(false);
            }
        }
    }
}