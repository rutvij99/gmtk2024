using System;
using UnityEngine;

namespace GameIdea2.Audio._UI
{
    public class TileData : MonoBehaviour
    {
        [SerializeField] private HoverGUI gui;
        public string Title;
        [TextArea]
        public string Desc;
        public string MassRange;
        public string PrefabName;
        public string DragRepresentator;
    }
}