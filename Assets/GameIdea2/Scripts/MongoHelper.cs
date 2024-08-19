using System.Collections;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.Networking;

class CustomLevel
{
    public string title;
    public string author;
    public string levelData;
}

public class MongoHelper : MonoBehaviour
{
    private string getLevelNames = "https://ap-south-1.aws.data.mongodb-api.com/app/data-gywoypa/endpoint/getLevelNames";
    private string getLevelData = "https://ap-south-1.aws.data.mongodb-api.com/app/data-gywoypa/endpoint/getLevelData";
    private string insertLevel = "https://ap-south-1.aws.data.mongodb-api.com/app/data-gywoypa/endpoint/insertLevel";
    private string apiKey = "GoeYj6grOygNnuU711v1Yryph7ApqKh2S1gsfB0vGhuzGpDH9Hxyh8hrePB28kdk";

    public void Start()
    {
        // StartCoroutine(GetLevelNames());
        // StartCoroutine(GetLevelData());
        StartCoroutine(InsertLevelData());
    }

    IEnumerator GetLevelNames()
    {
        UnityWebRequest request = UnityWebRequest.Get(getLevelNames);
        request.SetRequestHeader("api-key", apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON response
            string jsonResponse = request.downloadHandler.text;
            // Process the data as needed
            Debug.Log(jsonResponse);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
    
    IEnumerator GetLevelData(string levelID = "66c2fd850126c59efd2268b8")
    {
        UnityWebRequest request = UnityWebRequest.Get(getLevelData+"?arg1="+levelID);
        request.SetRequestHeader("api-key", apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON response
            string jsonResponse = request.downloadHandler.text;
            // Process the data as needed
            Debug.Log(jsonResponse);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
    
    IEnumerator InsertLevelData()
    {
        CustomLevel customLevel = new CustomLevel();
        customLevel.author = "ww";
        customLevel.title = "testLevel";
        customLevel.levelData = "123456";
        UnityWebRequest request =
            UnityWebRequest.Post(insertLevel,  JsonUtility.ToJson(customLevel), "application/json");
        
        Debug.Log(JsonUtility.ToJson(customLevel));
        request.SetRequestHeader("api-key", apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON response
            string jsonResponse = request.downloadHandler.text;
            // Process the data as needed
            Debug.Log(jsonResponse);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}