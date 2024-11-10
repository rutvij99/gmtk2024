using GravityWell.Common.Helpers;
using UnityEngine;


namespace GravityWell.Core
{
	public enum GameStates
	{
		Splash,
		MainMenu,
		StoryEditor,
		LevelEditor
	}
	
	public class GameStateMachine : Singleton<GameStateMachine>
	{
		private GameStateFactory _factory;
		
		private GameBaseState _currentState;

		public static GameBaseState CurrentState => Instance._currentState;

		protected override void Awake()
		{
			base.Awake();
			if(Instance != this) return;
			_factory = new GameStateFactory(this);
			_currentState = _factory.Splash();
			_currentState.EnterState();
		}
		
		private void Update()
		{
			if (_currentState == null) return;

			_currentState.UpdateState();
		}
		
		internal void SwitchState(GameBaseState newState)
		{
			_currentState.ExitState();
			newState.EnterState();

			_currentState = newState;
		}
		
		internal void SwitchState(GameStates gameState)
		{
			var newState = _factory.GetState(gameState);
			if(newState == null) return;
			_currentState.ExitState();
			newState.EnterState();
			_currentState = newState;
		}

	}
}
