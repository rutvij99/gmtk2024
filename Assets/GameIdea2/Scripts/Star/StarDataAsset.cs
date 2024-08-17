using UnityEngine;
using UnityEngine.Serialization;

namespace GameIdea2.Stars
{
    [CreateAssetMenu(menuName = "GMTK24/StarColorAsset", fileName = "DefaultStarAsset")]
    public class StarDataAsset : ScriptableObject
    {
        [SerializeField] public GameObject StarExplosionFx;
        [GradientUsage(true)] public Gradient StarColor;
        [FormerlySerializedAs("maxScale")] [SerializeField] public float MaxScale=100;
        [FormerlySerializedAs("massInfulence")] [SerializeField] public float MassInfulence=1000;
        [FormerlySerializedAs("colorInfluenceOperation")] [SerializeField] public Operation ColorInfluenceOperation = Operation.Multiply;
        [FormerlySerializedAs("animateTillingOffset")] [SerializeField] public bool AnimateTillingOffset = true;
        [FormerlySerializedAs("textureOffsetDir")] [SerializeField] public Vector2 TextureOffsetDir = Vector2.one;
        [FormerlySerializedAs("minOffsetSpeed")] [SerializeField] public float MinOffsetSpeed = 0.1f;
        [FormerlySerializedAs("maxOffsetSpeed")] [SerializeField] public float MaxOffsetSpeed = 0.5f;
    }
}