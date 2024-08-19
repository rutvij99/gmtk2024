#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameConfig
{
    private const string LEVEL_PREF = "LevelCompletedKey";
    public const int MAX_LEVELS = 20;
    public static void SetLevelComplete(int level)
    {
        if (PlayerPrefs.GetInt(LEVEL_PREF, 1) >= level) return;
        PlayerPrefs.SetInt(LEVEL_PREF, level);
        PlayerPrefs.Save();
    }

    public static int GetLastCompletedLevel()
    {
       return PlayerPrefs.GetInt(LEVEL_PREF, 1);
    }

    public static void LoadLevel(int id)
    {
	    SceneManager.LoadScene($"Level {id.ToString("00")}");
    }
    
    public static void LoadLevel(string name)
    {
	    if (SceneManager.GetSceneByName(name).IsValid())
		    SceneManager.LoadScene(name);
    }

    public static void Exit()
    {
#if UNITY_EDITOR
	    // Check if the editor is currently in Play Mode
	    if (EditorApplication.isPlaying)
	    {
		    // Stop Play Mode
		    EditorApplication.isPlaying = false;
	    }
#endif
	    Application.Quit();
    }

    public static void LoadMainMenu()
    {
	    SceneManager.LoadScene("MainMenu");
    }
}
