using GameIdea2.Scripts.Terrestial;
using UnityEngine;

namespace GameIdea2.Scripts.Planets
{
    public class GassyPlanet : MonoBehaviour
    {
        [SerializeField] private GameObject ExplosionFx;
        [SerializeField] private GassyPlanetAsset DataAsset;

        private Rigidbody rb;
        private IDirtyableBehaviour dirtyable;
        private void Start()
        {
            rb = GetComponentInChildren<Rigidbody>();
            dirtyable = GetComponent<IDirtyableBehaviour>();
            if(Universe.Instance.Simulate)
                return;
            
            var mat = GetComponentInChildren<MeshRenderer>().material;
            mat.SetColor("_PrimaryColor", DataAsset.PrimaryColors[Random.Range(0, DataAsset.PrimaryColors.Count)]);
            mat.SetColor("_SecondaryColor", DataAsset.SecondaryColors[Random.Range(0, DataAsset.SecondaryColors.Count)]);
            mat.SetFloat("_AtmosphereTurbulance", Mathf.Lerp(DataAsset.MinAtmThickness, DataAsset.MaxAtmThickness, Random.Range(0,1f)));
        }

        private void Update()
        {
            if (transform.localScale.x <= 0)
            {
                DestroyPlanet();
                return;
            }
            rb.mass = Mathf.Lerp(DataAsset.MinMass, DataAsset.MaxMass, transform.localScale.x/ DataAsset.MaxScale);
            dirtyable.MarkDirty();
        }

        private void DestroyPlanet()
        {
            var g = Instantiate(ExplosionFx, transform.position, Quaternion.identity);
            Destroy(g.gameObject, 2);
            Destroy(this.gameObject);
        }
    }
}