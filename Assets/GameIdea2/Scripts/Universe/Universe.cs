using System;
using System.Collections;
using GameIdea2.Gameloop;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameIdea2
{
    public class Universe : MonoBehaviour
    {
        public const string WORKSPACE_NAME = "UniverseWorkspace";
        
        private GameObject workspace;
        public System.Action<int> OnSimStarted;

        private Coroutine recalcRoutine;
        
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
                int totalPlayers=0;
                foreach (var body in FindObjectsByType<TerrestialBody>(FindObjectsSortMode.None))
                {
                    if(body.transform.IsChildOf(workspace.transform))
                        continue;
                    
                    body.enabled = true;
                    var player = body.GetComponentInChildren<Player>();
                    if (player)
                    {
                        totalPlayers += 1;
                        player.GetComponent<SphereCollider>().enabled = true;
                    }
                }
                
                OnSimStarted?.Invoke(totalPlayers);
            }

            if (workspace)
            {
                Instantiate(workspace);
                workspace.SetActive(false);
                
                DontDestroyOnLoad(workspace);    
            }
            
            if (!resetCamera)
                EnableAllTerestialBodies();
            else
                ResetCamera(EnableAllTerestialBodies);
        }

        public void ResetCamera(Action onComplete=null)
        {
            StartCoroutine(ResetCamera(0.25f, onComplete));
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

        public GameObject GetWorkspace()
        {
            if (workspace)
                return workspace;

            workspace = FindWorkspace();
            if (workspace)
            {
                workspace.SetActive(true);
                SceneManager.MoveGameObjectToScene(workspace, SceneManager.GetActiveScene());
            }
            
            return workspace;
        }

        private GameObject FindWorkspace()
        {
            foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.name.Equals(WORKSPACE_NAME))
                    return go;
            }
            return null;
        }
        
        public GameObject CreateWorkspace()
        {
            if (!workspace)
            {
                workspace = FindWorkspace();
            }
            
            if(workspace)
                Destroy(workspace);

            workspace = new GameObject(WORKSPACE_NAME);
            return workspace;
        }

        public void CleanWorkspace()
        {
            if(workspace)
                Destroy(workspace);
        }
        
        public string SerializeLevel(string levelName, string authorName)
        {
            if (!workspace)
                return null;
            
            return LevelMap.SerialiseWorkspace(workspace, levelName, authorName);
        }

        public void LoadLevelFromJson(string json, bool keepWorkspace)
        {
            if(String.IsNullOrEmpty(json))
                return;

            var map = LevelMap.LoadMapFromJson(json);
            if (map == null)
                return;
            
            foreach (var obj in map.TerrestrialObjects)
            {
                var goRef = Resources.Load<GameObject>(obj.Key);
                var goInst = Instantiate(goRef, obj.Position, Quaternion.Euler(obj.Rotation));
                goInst.transform.localScale = obj.Scale;
            }
            
            if(!keepWorkspace)
                CreateWorkspace();
        }

        public void MarkDirty(GameObject obj)
        {
            if(recalcRoutine != null)
                return;

            recalcRoutine = StartCoroutine(RecalculateTrajectory(obj));
        }

        IEnumerator RecalculateTrajectory(GameObject obj)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            
            if (!obj)
            {
                foreach (var trajecorySys in FindObjectsByType<TrajectorySystem>(FindObjectsSortMode.None))
                {
                    trajecorySys.SimulateTrajectory();
                    yield return new WaitForEndOfFrame();
                }

                recalcRoutine = null;
                yield break;
            }
            
            var trajectory = obj.GetComponent<TrajectorySystem>();
            if (trajectory)
            {
                trajectory.SimulateTrajectory();
                recalcRoutine = null;
                yield break;
            }
            
            foreach (var trajecorySys in FindObjectsByType<TrajectorySystem>(FindObjectsSortMode.None))
            {
                trajecorySys.SimulateTrajectory();
                yield return new WaitForEndOfFrame();
            }
            recalcRoutine = null;
        }
    }
}