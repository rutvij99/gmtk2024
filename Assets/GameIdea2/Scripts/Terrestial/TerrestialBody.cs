using GameIdea2.Audio;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameIdea2
{
    [RequireComponent(typeof(Rigidbody))]
    public class TerrestialBody : MonoBehaviour
    {
        public const float GravitationalConstant = 1000;
        
        [FormerlySerializedAs("ExplosionFX")] [SerializeField] private GameObject explosionFX;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private TerrestialCollisionRule collisionRule;
        [SerializeField] private bool startWithForce;
        [SerializeField] private float startForceMag;
        
        private Vector3 _startForceDir;

        public float Mass => rb.mass;
        
        public Vector3 GetStartLinearVelocity()
        {
            if(!startWithForce)
                return Vector3.zero;
            return _startForceDir.normalized * startForceMag;
        }

        public void UpdateStartDir()
        {
            _startForceDir = transform.forward;
            Universe.Instance?.MarkDirty(this.gameObject);
        }
        
        private void Awake()
        {
            UpdateStartDir();
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (startWithForce)
            {
                rb.linearVelocity = GetStartLinearVelocity();
            }
        }

        private void Update()
        {
            if (!Universe.Instance.Simulate)
                UpdateStartDir();
                
            var velocity = rb.linearVelocity.normalized;
            if(velocity.magnitude > 0)
                transform.forward = velocity;

        }

        private void FixedUpdate()
        {
            var terrestialBodies = FindObjectsByType<TerrestialBody>(FindObjectsSortMode.None);
            foreach (var terrObj in terrestialBodies)
            {
                if(terrObj.Equals(this))
                    continue;
            
                var objMass = terrObj.rb.mass;
                var dist = Vector3.Distance(transform.position, terrObj.transform.position);
                var forceMult = ComputeForce(rb.mass, objMass, dist);
                var dir = (transform.position - terrObj.transform.position).normalized;
                terrObj.rb.linearVelocity += dir * (forceMult * Time.fixedDeltaTime);
                //terrObj.rb.AddForce(dir * (forceMult), ForceMode.Acceleration);
            }
        }

        private float ComputeForce(float mass1, float mass2, float dist)
        {
            return GravitationalConstant * (mass1 * mass2) / Mathf.Pow(dist, 2);
        }

        private void OnCollisionEnter(Collision other)
        {
            if(!Universe.Instance.Simulate)
                return;
            
            var terrestialBody = other.gameObject.GetComponent<TerrestialBody>();
            if (terrestialBody && collisionRule== TerrestialCollisionRule.Destroyable)
            {
                Destroy(this.gameObject);
                Debug.Log("Dishoom!!");
                AudioManager.Instance?.PlaySoundOfType(SoundTyes.Boom);
                if (explosionFX)
                {
                    var go = Instantiate(explosionFX, transform.position, Quaternion.identity);
                    Destroy(go, 1);
                }
            }
        }
    }
    
}

