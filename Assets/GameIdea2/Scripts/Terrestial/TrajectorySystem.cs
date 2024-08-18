using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameIdea2
{
    
    public class TrajectorySystem : MonoBehaviour
    {
        public TerrestialBody body;
        public int SimResolution = 200;
        public bool useFixedDeltaTime;
        public float timeStep = 0.1f;

        [SerializeField]
        private float radiusError = 0.01f;
        
        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField]
        private LayerMask mask;
        
        private void Start()
        {
            lineRenderer.positionCount = SimResolution;
        }

        private void FixedUpdate()
        {
            if(!Universe.Instance.Simulate)
                SimulateTrajectory();
            else
            {
                lineRenderer.positionCount = 0;
            }
        }

        private void SimulateTrajectory()
        {
            List<Vector3> positions = new List<Vector3>();
            Vector3 currentVelocity = body.GetComponent<TerrestialBody>().GetStartLinearVelocity();
            Vector3 currentPosition = body.transform.position;
            var radius = (GetComponent<SphereCollider>().radius * transform.localScale.x);
            
            for (int i = 0; i < SimResolution; i++)
            {
                positions.Add(currentPosition);
                var collisions = Physics.OverlapSphere(currentPosition, radius,mask);
                if (collisions.Length > 0)
                    break;

                if (useFixedDeltaTime)
                    timeStep = Time.fixedDeltaTime;
                
                Vector3 acceleration = ComputeAcceleration(currentPosition);
                currentVelocity += acceleration * timeStep;
                currentPosition += currentVelocity * timeStep;
            }

            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }

        private Vector3 ComputeAcceleration(Vector3 position)
        {
            Vector3 acceleration = Vector3.zero;

            var terrestialBodies = FindObjectsByType<TerrestialBody>(FindObjectsSortMode.None);
            foreach (var otherBody in terrestialBodies)
            {
                if (otherBody == body) continue;

                var direction = otherBody.transform.position - position;
                float distance = direction.magnitude;
                float forceMagnitude = TerrestialBody.GravitationalConstant * (body.Mass * otherBody.Mass) / Mathf.Pow(distance, 2);
                acceleration += direction.normalized * forceMagnitude;
                acceleration.y = 0;
            }

            return acceleration;
        }

        private void OnDrawGizmos()
        {
            var radius = GetComponent<SphereCollider>().radius * transform.localScale.x;
            Gizmos.DrawSphere(transform.position, radius+radiusError);
        }
    }
    
}