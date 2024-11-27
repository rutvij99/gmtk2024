using UnityEngine;

namespace GameIdea2.Stars
{
    public class Blackhole : MonoBehaviour
    {
        [SerializeField] private GameObject ExplosionFx;
        private void Update()
        {
            if (transform.localScale.x <= 0)
            {
                DestroyMe();
                return;
            }
        }

        private void DestroyMe()
        {
            var go = Instantiate(ExplosionFx, transform.position, Quaternion.identity);
            Destroy(go, 10);
            Destroy(this.gameObject);
        }
    }
}