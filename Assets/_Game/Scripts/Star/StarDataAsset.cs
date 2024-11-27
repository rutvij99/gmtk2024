using UnityEngine;
using UnityEngine.Serialization;

namespace GameIdea2.Stars
{
    [CreateAssetMenu(menuName = "GMTK24/StarColorAsset", fileName = "DefaultStarAsset")]
    public class StarDataAsset : ScriptableObject
    {
        public GameObject StarExplosionFx;
        [GradientUsage(true)] public Gradient StarColor;
        
        [FormerlySerializedAs("maxScale")] 
        public float MaxScale=100;
        
        [FormerlySerializedAs("massInfulence")] 
        public float MassInfulence=1000;
        
        [FormerlySerializedAs("colorInfluenceOperation")] 
        public Operation ColorInfluenceOperation = Operation.Multiply;
        
        [FormerlySerializedAs("animateTillingOffset")] 
        public bool AnimateTillingOffset = true;
        
        [FormerlySerializedAs("textureOffsetDir")] 
        public Vector2 TextureOffsetDir = Vector2.one;
        
        [FormerlySerializedAs("minOffsetSpeed")]
        public float MinOffsetSpeed = 0.1f;
        
        [FormerlySerializedAs("maxOffsetSpeed")] public float MaxOffsetSpeed = 0.5f;
        public float MinMass = 1500f;
        public float MaxMass = 7500f;
    }
}