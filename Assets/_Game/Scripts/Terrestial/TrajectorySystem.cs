using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameIdea2.Scripts.Terrestial;
using GameIdea2.Terrestial;
using UnityEngine;

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

        private TerrestrialDataObject parentDataObject;
        private List<Vector3> simBuffer;
        private float fixedDeltaTime;
        private bool redrawNeeded = false;
        
        private void Start()
        {
            simBuffer = new List<Vector3>();
            lineRenderer.positionCount = SimResolution;
            parentDataObject = GetComponent<TerrestialBody>().DataObject;
            SimulateTrajectory();
        }

        private void FixedUpdate()
        {
            fixedDeltaTime = Time.fixedDeltaTime;
            if (Universe.Instance.Simulate)
            {
                lineRenderer.positionCount = 0;
            }
            else
            {
                DrawTrajectory();
            }
        }
        
        public void DrawTrajectory()
        {
            if(!redrawNeeded)
                return;
            
            lineRenderer.positionCount = simBuffer.Count;
            for(int indx = 0; indx < simBuffer.Count; indx++)
            {
                var currentPos = simBuffer[indx];
                lineRenderer.positionCount = indx + 1;
                lineRenderer.SetPosition(indx, currentPos);
                
                var radius = (parentDataObject.CollisionRadius * parentDataObject.ObjectScale);
                var collisions = Physics.OverlapSphere(currentPos, radius,mask);
                if (collisions.Length > 0)
                    break;
            }

            redrawNeeded = false;
        }

        private void TrajectorySimulationOperation()
        {
            simBuffer.Clear();
            Vector3 currentVelocity = parentDataObject.StartLinearVelocity;
            Vector3 currentPosition = parentDataObject.Position;
            for (int i = 0; i < SimResolution; i++)
            {
                simBuffer.Add(currentPosition);
                if (useFixedDeltaTime)
                    timeStep = fixedDeltaTime;
                
                Vector3 acceleration = ComputeAcceleration(currentPosition);
                currentVelocity += acceleration * timeStep;
                currentPosition += currentVelocity * timeStep;
            }
            redrawNeeded = true;
        }
        
        private Vector3 ComputeAcceleration(Vector3 position)
        {
            Vector3 acceleration = Vector3.zero;
            foreach (var otherBody in TerrestrialDataObjectPool.GetPool())
            {
                if (otherBody.Value.Equals(parentDataObject)) 
                    continue;

                var direction = otherBody.Value.Position - position;
                float distance = direction.magnitude;
                float forceMagnitude = TerrestialBody.GravitationalConstant * (parentDataObject.Mass * otherBody.Value.Mass) / Mathf.Pow(distance, 2);
                acceleration += direction.normalized * forceMagnitude;
                acceleration.y = 0;
            }

            return acceleration;
        }

        public void SimulateTrajectory()
        {
            TrajectoryJobsQueue.GetQueue().Add(new TrajectoryJob()
            {
                InvokeableAction = TrajectorySimulationOperation
            });
        }
    }
    
}