using GravityWell.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GravityWell.Core
{
	public class MainMenuState : GameBaseState
	{
		public override GameStates Type => GameStates.Splash;

		public MainMenuState(GameStateMachine machineContext, GameStateFactory gameStateFactory) : base(machineContext, gameStateFactory) { }

		public override void EnterState()
		{
			Init();
		}

		public override void ExitState()
		{
			Cleanup();
		}

		public override void UpdateState()
		{
			
		}


		private const string SCENE = "MainMenu_New";
		private Scene splashScene;
		
		private void Init()
		{
			SceneManager.LoadScene(SCENE, LoadSceneMode.Single);
			SceneManager.GetSceneByName(SCENE);
		}

		private void Cleanup()
		{
			SceneManager.UnloadSceneAsync(splashScene);
		}
	}
}
