using UnityEngine;

namespace GravityWell.UI
{
	public interface IMenuHandler
	{
		internal void ExitGame();
		void OpenMenu(MenuUI menu);
		void CloseMenu();
	}
}
