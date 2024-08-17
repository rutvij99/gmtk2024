using System;
using UnityEngine;

namespace GameIdea2.UI
{
    public class EditmodeGUI : MonoBehaviour
    {
        [SerializeField] private GameObject HintGUI;
        public bool Interacted {
            get
            {
                if (!HintGUI)
                    return false;

                return HintGUI.activeSelf;
            }
            set
            {
                if(!HintGUI)
                    return;
                
                HintGUI.SetActive(!value);
            }
        }

        public void EnableSimulation()
        {
            var universe = Universe.Instance;
            if (universe != null) universe.Simulate = true;
        }

        public void RequestTerrestialBodySpawn(string key)
        {
            GetComponent<EditModeController>().SpawnTerrestial(key);
        }
        
    }
}