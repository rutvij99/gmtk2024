using UnityEngine;

public static class GameConfig
{
    private const string LEVEL_PREF = "LevelCompletedKey";
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
}
