using System;
using System.Collections;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using Task = System.Threading.Tasks.Task;

namespace GameIdea2
{
    public class Universe : MonoBehaviour
    {
        private static Universe instance;
        public static Universe Instance
        {
            get
            {
                if (!instance)
                {
                    instance = FindFirstObjectByType<Universe>();
                }

                return instance;
            }
        }

        private bool simulateTerrestialBodies = false;
        public bool Simulate
        {
            get { return simulateTerrestialBodies; }
            set
            {
                simulateTerrestialBodies = value;
                if (simulateTerrestialBodies)
                {
                    EnableSimulation(false);
                }
            }
        }

        private void EnableSimulation(bool resetCamera)
        {
            void EnableAllTerestialBodies()
            {
                foreach (var body in FindObjectsByType<TerrestialBody>(FindObjectsSortMode.None))
                {
                    body.enabled = true;
                }
            }
            
            if(!resetCamera)
                EnableAllTerestialBodies();
            else 
                StartCoroutine(ResetCamera(0.25f, EnableAllTerestialBodies));
        }

        IEnumerator ResetCamera(float dur, Action onComplete=null)
        {
            var mainCam = Camera.main;
            if (!mainCam)
            {
                onComplete?.Invoke();
                yield break;
            }

            if (dur <= 0)
            {
                mainCam.transform.position = new Vector3(0,mainCam.transform.position.y, 0);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                float timeStep = 0;
                var start = mainCam.transform.position;
                var end = new Vector3(0, start.y, 0);
                while (timeStep <= 1)
                {
                    timeStep += Time.deltaTime / dur;
                    mainCam.transform.position = Vector3.Lerp(start, end, timeStep);
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForEndOfFrame();
            }
            
            onComplete?.Invoke();
        }

        public void GenerateTrajectory()
        {
            CreateSceneParameters sceneParameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
            Scene simScene = SceneManager.CreateScene("Simulation", sceneParameters);
            var simPhysicsScene = simScene.GetPhysicsScene();
            
        }
    }
}