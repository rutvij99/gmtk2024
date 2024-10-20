using UnityEngine;


namespace GravityWell.Core.Helpers
{
	public static class AutoSpawner
	{
		// private const string STATEMACHINE_PREFAB = "Core/GameStateMachine";
		private const string CONFIG_PREFAB = "Core/GameConfig";
		private const string LOADER_PREFAB = "Core/Loader";

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		private static void AutoSpawn()
		{
			Spawn(CONFIG_PREFAB);
			Spawn(LOADER_PREFAB);
			// Spawn(STATEMACHINE_PREFAB);
		}

		private static void Spawn(string prefabName)
		{
			var asset = Resources.Load<GameObject>(prefabName);
			if (asset == null) return;
			var obj = GameObject.Instantiate(asset);
			obj.name = $"[{asset.name}]";
		}
	}
}
