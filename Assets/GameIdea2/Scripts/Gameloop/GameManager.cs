using UnityEngine;
using GameIdea2.Stars;
using UnityEngine.SceneManagement;
using Task = System.Threading.Tasks.Task;

namespace GameIdea2.Gameloop
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (!instance)
                {
                    instance = FindFirstObjectByType<GameManager>();
                }

                return instance;
            }
        }

        private bool reachedTraget;
        public bool ReachedTarget
        {
            get { return reachedTraget; }
            set
            {
                reachedTraget = value;
                if (reachedTraget)
                    OnReachedTarget();
            }
        }
        
        private async void OnReachedTarget()
        {
            foreach (var star in FindObjectsByType<Star>(FindObjectsSortMode.None))
            {
                star.transform.localScale = Vector3.zero;
            }
            await Task.Delay(1500);
            RestartLevel();
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}