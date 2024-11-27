using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GravityWell.Core
{
	public class SplashState : GameBaseState
	{
		public override GameStates Type => GameStates.Splash;
		
		public SplashState(GameStateMachine machineContext, GameStateFactory gameStateFactory) : base(machineContext, gameStateFactory) { }

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


		private const string SCENE = "SplashScreen";
		private Scene splashScene;
		
		private void Init()
		{
			SceneManager.LoadScene(SCENE, LoadSceneMode.Single);
			splashScene = SceneManager.GetSceneByName(SCENE);
		}

		public void OnSplashComplete()
		{
			_machineContext.SwitchState(GameStates.MainMenu);
		}

		private void Cleanup()
		{
			SceneManager.UnloadSceneAsync(splashScene);
		}
	}
}
