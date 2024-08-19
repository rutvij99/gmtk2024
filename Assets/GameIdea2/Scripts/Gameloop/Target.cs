using System;
using UnityEngine;

namespace GameIdea2.Gameloop
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private GameObject ReachHereText;

        private void Awake()
        {
            var others = FindObjectsByType<Target>(FindObjectsSortMode.None);
            foreach (var other in others)
            {
                if(other && other != this)
                    Destroy(other.gameObject);
            }
        }

        private void Update()
        {
            if(!ReachHereText || !Universe.Instance)
                return;
            
            ReachHereText?.SetActive(!Universe.Instance.Simulate);
        }
    }
}