using System;
using GameIdea2.Scripts.Terrestial;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameIdea2.Stars
{
    public enum Operation {
        Add,
        Subtract,
        Multiply,
        Divide
    }
    
    public class Star : MonoBehaviour
    {
        [FormerlySerializedAs("VisaulFXAsset")] [SerializeField] private StarDataAsset DataAsset;

        [SerializeField]
        private bool sizeAffectsMass;
        private Material mainMaterial;
        private Light light;
        private Rigidbody rb;
        private float colorValue=0;
        private float massEffect = 0;
        private float scaleEffect = 0;

        private IDirtyableBehaviour dirtyable;
        
        private void Start()
        {
            mainMaterial = GetComponentInChildren<MeshRenderer>().material;
            light = GetComponentInChildren<Light>();
            rb = GetComponent<Rigidbody>();
            dirtyable = GetComponent<IDirtyableBehaviour>();
        }

        private void Update()
        {
            if (sizeAffectsMass)
            {
                var lerpEval = transform.localScale.x / DataAsset.MaxScale;
                rb.mass = Mathf.Lerp(DataAsset.MinMass, DataAsset.MaxMass, lerpEval);
                dirtyable.MarkDirty();
            }

            if (DataAsset.AnimateTillingOffset)
            {

                mainMaterial.mainTextureOffset += DataAsset.TextureOffsetDir * (Mathf.Lerp(DataAsset.MinOffsetSpeed, DataAsset.MaxOffsetSpeed,
                    transform.localScale.magnitude / DataAsset.MaxScale) * Time.deltaTime);
            }

            if (sizeAffectsMass)
            {
                colorValue = transform.localScale.magnitude / DataAsset.MaxScale;
            }
            else
            {
                scaleEffect = Mathf.Clamp01(transform.localScale.magnitude / DataAsset.MaxScale);
                massEffect = (rb.mass / DataAsset.MassInfulence);
                colorValue = Mathf.Clamp01(Operate(scaleEffect,massEffect, DataAsset.ColorInfluenceOperation));
            }
            
            if (mainMaterial)
                mainMaterial.color = DataAsset.StarColor.Evaluate(colorValue);

            if(light)
                light.color = DataAsset.StarColor.Evaluate(colorValue);
            
            if (transform.localScale.x > DataAsset.MaxScale || transform.localScale.x < 1)
            {
                var go = Instantiate(DataAsset.StarExplosionFx, transform.position, Quaternion.identity);
                Destroy(go, 5);
                Destroy(this.gameObject);
            }
        }

        private float Operate(float val1, float val2, Operation op)
        {
            switch (op)
            {
                case Operation.Add: 
                    return val1 + val2;
                
                case Operation.Subtract: 
                    return val1 - val2;
                
                case Operation.Multiply: 
                    return val1 * val2;
                
                case Operation.Divide: 
                    return val1 / val2;
            }

            return 0;
        }
        
    }
}