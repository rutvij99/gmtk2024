using UnityEngine;


namespace GravityWell.Core
{
	public class GameStateFactory
	{
		GameStateMachine _machineContext;

		public GameStateFactory(GameStateMachine currentMachineContext) { _machineContext = currentMachineContext; }

		public GameBaseState GetState(GameStates state)
		{
			switch (state)
			{
				case GameStates.Splash:
					return Splash();
				case GameStates.MainMenu:
					return MainMenu();
				case GameStates.StoryEditor:
					break;
				case GameStates.LevelEditor:
					break;
				default:
					return null;
			}
			return null;
		}
		
		public GameBaseState Splash() { return new SplashState(_machineContext, this); }
		public GameBaseState MainMenu() { return new MainMenuState(_machineContext, this); }
		// public GameBaseState StoryEditor() { return new PlayerJumpState(_context, this); }
		// public GameBaseState LevelEditor() { return new PlayerCrouchState(_context, this); }
	}
}
