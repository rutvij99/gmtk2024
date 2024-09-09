using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace GameIdea2.Scripts.Terrestial
{
    public class TrajectoryJob
    {
        public Action InvokeableAction;
    }
    
    public class TrajectoryJobsQueue : MonoBehaviour
    {
        private static TrajectoryJobsQueue instance;
        private ConcurrentQueue<TrajectoryJob> jobs;

        private float fixedDeltaTime;
        private Thread jobsThread;

        private void OnDestroy()
        {
            if (jobsThread != null)
            {
                jobsThread.Abort();
            }
        }

        private void Initialise()
        {
            if (instance && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            jobs = new ConcurrentQueue<TrajectoryJob>();
            DontDestroyOnLoad(this.gameObject);

            jobsThread = new Thread(JobsLoop);
            jobsThread.Start();
        }

        private void FixedUpdate()
        {
            fixedDeltaTime = Time.fixedDeltaTime;
        }

        private async void JobsLoop()
        {
            while (jobsThread != null)
            {
                foreach (var job in jobs)
                {
                    try
                    {
                        job.InvokeableAction?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                jobs.Clear();
                await Task.Delay((int)(fixedDeltaTime * 1000));
            }
        }
        
        public void Add(TrajectoryJob job)
        {
            jobs.Enqueue(job);
        }
        
        public static TrajectoryJobsQueue GetQueue()
        {
            if (instance) return instance;
            
            instance = new GameObject("TrajectoryJobsQueue").AddComponent<TrajectoryJobsQueue>();
            instance.Initialise();
            return instance;
        }
    }
}