using System.Collections.Generic;
using UnityEngine;

namespace GameIdea2.Scripts.Planets
{
    [CreateAssetMenu(menuName = "GMTK24/GassyPlanetAsset", fileName = "DefaultGassyPlanetAsset")]
    public class GassyPlanetAsset : ScriptableObject
    {
        [ColorUsage(true, true)]
        public List<Color> PrimaryColors;
        [ColorUsage(true, true)]
        public List<Color> SecondaryColors;
        public float MinAtmThickness = 10;
        public float MaxAtmThickness = 35;
        public float MinMass = 75;
        public float MaxMass = 175;
        public float MaxScale = 75;
    }
}