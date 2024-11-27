using UnityEngine;

namespace GravityWell.Core
{
	public abstract class GameBaseState
	{
		public virtual GameStates Type { get; protected set; }

		protected GameStateMachine _machineContext;
		protected GameStateFactory _factory;

		protected GameBaseState _currentSubState;
		protected GameBaseState _currentSuperState;

		public GameBaseState(GameStateMachine machineContext, GameStateFactory gameStateFactory)
		{
			_machineContext = machineContext;
			_factory = gameStateFactory;
		}

		public abstract void EnterState();
		public abstract void ExitState();

		public abstract void UpdateState();
	}
}
