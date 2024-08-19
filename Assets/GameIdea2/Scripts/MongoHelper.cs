using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class CustomLevel
{
    public string title;
    public string author;
    public string levelData;
}

public class MongoHelper : MonoBehaviour
{
    private static string apiKey = "GoeYj6grOygNnuU711v1Yryph7ApqKh2S1gsfB0vGhuzGpDH9Hxyh8hrePB28kdk";

    public IEnumerator GetLevelNames(string apiURL, Action<string> onComplete, Action onFail)
    {
        UnityWebRequest request = UnityWebRequest.Get(apiURL);
        request.SetRequestHeader("api-key", apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON response
            string jsonResponse = request.downloadHandler.text;
            // Process the data as needed
            onComplete?.Invoke(jsonResponse);
        }
        else
        {
            Debug.LogError(request.error);
            onFail?.Invoke();
        }
    }
    
    public static IEnumerator GetLevelData(string apiURL, string levelID, Action<CustomLevel> onComplete, Action onFail)
    {
        UnityWebRequest request = UnityWebRequest.Get(apiURL+"?arg1="+levelID);
        request.SetRequestHeader("api-key", apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON response
            string jsonResponse = request.downloadHandler.text;
            // Process the data as needed
            var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
            onComplete?.Invoke(new CustomLevel(){levelData = obj["level_data"]});
        }
        else
        {
            Debug.LogError(request.error);
            onFail?.Invoke();
        }
    }
    
    public IEnumerator InsertLevelData(string apiURL, string jsonData, Action onComplete, Action onError)
    {
        CustomLevel customLevel = new CustomLevel();
        customLevel.author = "ww_";
        customLevel.title = "level_data";
        customLevel.levelData = jsonData;
        UnityWebRequest request =
            UnityWebRequest.Post(apiURL,  JsonUtility.ToJson(customLevel), "application/json");
        
        Debug.Log(JsonUtility.ToJson(customLevel));
        request.SetRequestHeader("api-key", apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON response
            string jsonResponse = request.downloadHandler.text;
            // Process the data as needed
            Debug.Log(jsonResponse);
            onComplete?.Invoke();
        }
        else
        {
            Debug.LogError(request.error);
            onError?.Invoke();
        }
    }
}