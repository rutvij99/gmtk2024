using System;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class HDRLight : MonoBehaviour
{
    [SerializeField, ColorUsage(false, true)]
    private Color color;

    [SerializeField] private bool pickFromMaterial;
    [SerializeField] private Material srcMaterial;

    private Light lightSrc;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightSrc = GetComponent<Light>();
        UpdateLightColor();
    }

    private void UpdateLightColor()
    {
        if(pickFromMaterial)
            lightSrc.color = srcMaterial.color;
        else
        {
            lightSrc.color = color;
        }
    }
    
    private void FixedUpdate()
    {
#if UNITY_EDITOR
        UpdateLightColor();
#endif
    }
}
