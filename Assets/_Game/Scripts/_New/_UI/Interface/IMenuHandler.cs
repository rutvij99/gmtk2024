using UnityEngine;
using UnityEngine.InputSystem;

namespace GravityWell.UI
{
	public interface IMenuHandler
	{
		public PlayerInput PlayerInput { get; }
		internal void ExitGame();
		void OpenMenu(MenuUI menu);
		void CloseMenu();
	}
}
