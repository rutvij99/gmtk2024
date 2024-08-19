using System.Collections.Generic;
using UnityEngine;

namespace GameIdea2.Scripts.Planets
{
    [CreateAssetMenu(menuName = "GMTK24/RockyPlanetAsset", fileName = "DefaultRockPlanetAsset")]
    public class RockyPlanetAsset : ScriptableObject
    {
        public List<Color> WaterColors;
        public List<Color> LandColors;
        public float MinNoiseStr = 10;
        public float MaxNoiseStr = 35;
        public float MinMass = 50;
        public float MaxMass = 100;
        public float MaxScale = 75;

        public float MinElevation = 0.0f;
        public float MaxElevation = 0.15f;

        [Range(0, 1f)] public float WaterProbability = 0.5f;
    }
}