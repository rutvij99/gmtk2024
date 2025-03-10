#if UNITY_EDITOR
using UnityEditor;
#endif
using GameIdea2.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameConfig
{
    private const string LEVEL_PREF = "LevelCompletedKey";
    public const int MAX_LEVELS = 15;
    public static int CurrentLevel;
    
    public static void SetLevelComplete()
    {
        if (PlayerPrefs.GetInt(LEVEL_PREF, 0) > CurrentLevel) return;
        Debug.Log($"setting level complete {CurrentLevel}");
        PlayerPrefs.SetInt(LEVEL_PREF, CurrentLevel + 1);
        PlayerPrefs.Save();
    }

    public static int GetLastCompletedLevel()
    {
       return PlayerPrefs.GetInt(LEVEL_PREF, 0);
    }

    public static void LevelFinished()
    {
	    if(CurrentLevel + 1 < MAX_LEVELS)
			LoadLevel(CurrentLevel + 1);
    }

    public static void LoadLevel(int id)
    {
	    if (id + 1 < MAX_LEVELS)
	    {
		    CurrentLevel = id;
		    SceneManager.LoadScene($"Level {id:00}");
	    }
    }
    
    public static void LoadLevel(string name)
    {
	    // if (SceneManager.GetSceneByName(name).IsValid())
	    AudioManager.Instance?.ChangeBackgroundMusic();
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
