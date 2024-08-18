using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	// [SerializeField] private LevelSelect LevelSelect

	public void ShowLevelSelect()
	{
			
	}
	
	public void LoadLevel()
	{
		SceneManager.LoadScene($"Level{GameConfig.GetLastCompletedLevel()}");
	}

	public void LoadFreePlay()
	{
		SceneManager.LoadScene($"FreePlay");
	}

	public void ShowOptions()
	{
		
	}

	public void Quit()
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
}
