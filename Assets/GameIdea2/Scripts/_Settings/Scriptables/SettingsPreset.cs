using UnityEngine;
using UnityEngine.Serialization;


namespace GravityWell.Core.Config
{
	[CreateAssetMenu(fileName = "SettingsPreset", menuName = "GravityWell/Settings/SettingsPreset")]
	public class SettingsPreset : ScriptableObject
	{
		public AudioSettings Audio;
		public GameplaySettings Gameplay;
		public DisplaySettings Display;
		public GraphicsPresets defaultGraphicsPreset;
	}
}
