using System;
using UnityEngine;

namespace GameIdea2.Scripts.Editor
{
    public class Editable : MonoBehaviour
    {
        [SerializeField] private GameObject SelectedGUI; 
        
        public static Editable CurrentSelection;

        public float MaxScale = 100;
        
        private void Start()
        {
            SelectedGUI.SetActive(false);
        }

        public void Deselect()
        {
            if (CurrentSelection == this)
                CurrentSelection = null;
            
            SelectedGUI.SetActive(false);
        }
        
        public void Select()
        {
            if(CurrentSelection)
                CurrentSelection.Deselect();

            CurrentSelection = this;
            SelectedGUI.SetActive(true);
        }
    }
}