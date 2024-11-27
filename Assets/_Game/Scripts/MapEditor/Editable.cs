using System;
using System.Collections.Generic;
using GameIdea2.Gameloop;
using Unity.VisualScripting;
using UnityEngine;

namespace GameIdea2.Scripts.Editor
{
    public class Editable : MonoBehaviour
    {
        [SerializeField] private GameObject SelectedGUI;
        
        private List<GameObject> HideList;
        public static Editable CurrentSelection;
        private bool ogState;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb)
                ogState = rb.isKinematic;
            
            UpdateKinematics();
        }

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

        private void UpdateKinematics()
        {
            if (Universe.Instance.Simulate && rb)
            {
                if(GetComponent<Player>())
                    rb.isKinematic = false;
                else
                {
                    rb.isKinematic = ogState;
                }
            }
            else if (!Universe.Instance.Simulate && rb)
            {
                rb.isKinematic = true;
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