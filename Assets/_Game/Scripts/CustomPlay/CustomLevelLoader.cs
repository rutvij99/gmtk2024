using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace GameIdea2.CustomPlay
{
    public class CustomLevelLoader : MonoBehaviour
    {
        [SerializeField] private Api api;
        [SerializeField] private string overrideLevelId;
        [SerializeField] private GameObject loadingScreen;
        
        private static Dictionary<string, CustomLevel> cachedLevels;
        
        private static string activeLevelId;
        private static bool keepWorkspace;
        public static void LoadLevel(string levelId)
        {
            activeLevelId = levelId;
            keepWorkspace = true;
            SceneManager.LoadScene("CustomLevelPlayer");
        }
        
        private async void Start()
        {
            if (Application.isEditor && !String.IsNullOrEmpty(overrideLevelId))
                activeLevelId = overrideLevelId;

            if (cachedLevels == null)
                cachedLevels = new Dictionary<string, CustomLevel>();

            if (String.IsNullOrEmpty(activeLevelId))
            {
                OnLevelDataLoadFailed();
                return;
            }

            if (cachedLevels.ContainsKey(activeLevelId))
            {
                OnLevelDataLoaded(cachedLevels[activeLevelId]);
            }
            else
            {
                StartCoroutine(MongoHelper.GetLevelData(api.FetchLevelAPI, activeLevelId, OnLevelDataLoaded,
                    OnLevelDataLoadFailed));
            }
        }

        private void OnLevelDataLoaded(CustomLevel data)
        {
            if(!cachedLevels.ContainsKey(activeLevelId))
                cachedLevels.Add(activeLevelId, data);
            
            Universe.Instance.LoadLevelFromJson(data.levelData, keepWorkspace);
            loadingScreen.SetActive(false);
        }

        private void OnLevelDataLoadFailed()
        {
            SceneManager.LoadScene("CustomLevelSelector");
        }
        
    }
}