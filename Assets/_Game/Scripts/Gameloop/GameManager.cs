using System;
using System.Collections;
using System.Collections.Generic;
using GameIdea2.Audio;
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

        private int totalPlayers;
        
        private void Start()
        {
            Cursor.visible = true;
            if (Universe.Instance)
                Universe.Instance.OnSimStarted += OnSimStarted;
        }

        private async void OnReachedTarget()
        {
            totalPlayers -= 1;
            if (totalPlayers > 0)
                return;
            
            foreach (var star in FindObjectsByType<Star>(FindObjectsSortMode.None))
            {
                star.transform.localScale = Vector3.zero;
            }

            AudioManager.Instance?.PlayBsClips();
            var scenes = new List<string>()
            {
                "FreePLay",
                "LevelEditor",
                "CustomLevelPlayer"
            };
            Debug.Log($"check update {this.gameObject.scene.name}");
            if (!scenes.Contains(this.gameObject.scene.name))
            {
                GameConfig.SetLevelComplete();
                Debug.Log($"can update level pref");
            }
            else
            {
                Debug.Log($"can't update level pref");
            }
            HUDManager.instance.EnableLevelComplete();
        }

        public void NextLevelLoad()
        {
            Universe.Instance?.CleanWorkspace();
            GameConfig.LevelFinished();
        }

        private void OnSimStarted(int totalPlayers)
        {
            this.totalPlayers = totalPlayers;
        }
        
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}