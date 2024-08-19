using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameIdea2.Scripts.Planets
{
    [RequireComponent(typeof(TerrestialBody))]
    public class RockyPlanet : MonoBehaviour
    {
        [SerializeField] private GameObject ExplosionFx;
        [SerializeField] private RockyPlanetAsset DataAsset;

        private Rigidbody rb;
        
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            var waterBody = transform.GetChild(0);
            var mat = GetComponent<MeshRenderer>().material;
            var waterMat = waterBody.GetComponent<MeshRenderer>().material;
            
            mat.SetColor("_Base", DataAsset.LandColors[Random.Range(0, DataAsset.LandColors.Count)]);
            waterMat.SetColor("_Base", DataAsset.WaterColors[Random.Range(0, DataAsset.WaterColors.Count)]);
            
            mat.SetFloat("_NoiseStrength", Mathf.Lerp(DataAsset.MinNoiseStr, DataAsset.MaxNoiseStr, Random.Range(0,1f)));
            mat.SetFloat("_MaxElevation", Mathf.Lerp(DataAsset.MinElevation, DataAsset.MaxElevation, Random.Range(0,1f)));

            if (Random.Range(0f, 1f) <= DataAsset.WaterProbability)
            {
                waterBody.gameObject.SetActive(true);
            }
            else
            {
                waterBody.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (transform.localScale.x <= 0)
            {
                DestroyPlanet();
                return;
            }
            rb.mass = Mathf.Lerp(DataAsset.MinMass, DataAsset.MaxMass, transform.localScale.x/ DataAsset.MaxScale);
        }

        private void DestroyPlanet()
        {
            var g = Instantiate(ExplosionFx, transform.position, Quaternion.identity);
            Destroy(g.gameObject, 2);
            Destroy(this.gameObject);
        }
    }
}