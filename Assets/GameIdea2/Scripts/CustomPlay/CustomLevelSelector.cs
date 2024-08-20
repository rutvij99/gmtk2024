using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameIdea2.CustomPlay
{
    public class CustomLevelSelector : MonoBehaviour
    {
        [SerializeField] private Api api;
        [SerializeField] private GameObject bttnRef;
        [SerializeField] private Transform guiParent;
        [SerializeField] private GameObject Loader;
        
        public void Close()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private async void Start()
        {
            Loader.SetActive(true);
            StartCoroutine(MongoHelper.GetLevelNames(api.FetchListAPI, OnLevelDataRecieved, Close));
        }

        private void OnLevelDataRecieved(string data)
        {
            //Debug.Log(data);
            var serialised = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);
            foreach (var kv in serialised)
            {
                var id = kv["_id"];
                var title = kv["title"];
                // var author = kv["author"];
                var bttn = Instantiate(bttnRef, guiParent);
                bttn.SetActive(true);
                bttn.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = title;
                // bttn.transform.GetChild(2).GetComponent<TMPro.TMP_Text>().text = author;
                bttn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log($"Loading level {title}");
                    OnLevelLoadRequested(id);
                });
            }
            
            Loader.SetActive(false);
        }

        private void OnLevelLoadRequested(string levelId)
        {
            CustomLevelLoader.LoadLevel(levelId);
        }
    }
}