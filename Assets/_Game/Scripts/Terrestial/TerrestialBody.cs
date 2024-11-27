using System;
using System.Collections.Generic;
using GameIdea2.Audio;
using GameIdea2.Scripts.Terrestial;
using GameIdea2.Terrestial;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameIdea2
{
    [RequireComponent(typeof(Rigidbody))]
    public class TerrestialBody : MonoBehaviour, IDirtyableBehaviour
    {
        public const float GravitationalConstant = 1000;
        
        [FormerlySerializedAs("ExplosionFX")] [SerializeField] private GameObject explosionFX;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private TerrestialCollisionRule collisionRule;
        [SerializeField] private bool startWithForce;
        [SerializeField] private float startForceMag;

        public TerrestrialDataObject DataObject { get; private set; }
        
        private TerrestialBody[] bodies = null;
        private Vector3 _startForceDir;
        private string DataObjectId;
        private SphereCollider Collider;
        
        
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
            Collider = GetComponent<SphereCollider>();
            DataObjectId = Guid.NewGuid().ToString();
            DataObject = new TerrestrialDataObject(DataObjectId);

            TerrestrialDataObjectPool.GetPool().Add(DataObjectId, DataObject);
            UpdateStartDir();
            UpdateDataObject();
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (startWithForce)
            {
                rb.linearVelocity = GetStartLinearVelocity();
            }
        }

        private void UpdateDataObject()
        {
            DataObject.Position = transform.position;
            DataObject.StartLinearVelocity = GetStartLinearVelocity();
            DataObject.Mass = rb.mass;
            DataObject.CollisionRadius = Collider.radius;
            DataObject.ObjectScale = transform.localScale.x;
        }
        
        private void Update()
        {
            if (!Universe.Instance.Simulate)
            {
                if((Vector3.Distance(transform.forward, _startForceDir) >= 0.01f))
                    UpdateStartDir(); 
             
                UpdateDataObject();
                return;
            }
            
            var velocity = rb.linearVelocity.normalized;
            if(velocity.magnitude > 0)
                transform.forward = velocity;

        }

        private void FixedUpdate()
        {
            if (!Universe.Instance.Simulate)
                return;
            
            if(bodies == null)
                bodies = FindObjectsByType<TerrestialBody>(FindObjectsSortMode.None);
            
            foreach (var terrObj in bodies)
            {
                if(!terrObj)
                    continue;
                
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

        private void OnDestroy()
        {
            if(Universe.Instance && !Universe.Instance.Simulate)
                Universe.Instance.MarkDirty(null);

            if (DataObject != null)
                TerrestrialDataObjectPool.GetPool().Remove(DataObjectId);
        }

        public void MarkDirty()
        {
            UpdateDataObject();
        }
    }
    
}

