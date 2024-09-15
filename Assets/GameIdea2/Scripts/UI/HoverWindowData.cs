using System;
using UnityEngine;

namespace GameIdea2.Audio._UI
{
    public class HoverWindowData : MonoBehaviour
    {
        [SerializeField] private HoverGUI gui;
        public string Title;
        [TextArea]
        public string Desc;
        public string MassRange;
    }
}