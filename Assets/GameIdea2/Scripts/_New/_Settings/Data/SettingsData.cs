using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.VisualScripting;
using UnityEngine;


namespace GravityWell.Core.Config
{
	[System.Serializable]
	public class SettingsData
	{
		public AudioSettings Audio;
		public GameplaySettings Gameplay;
		public DisplaySettings Display;
		public GraphicsSettings Graphics;

		public SettingsData Clone()
		{
			var audioClone = this.Audio.Clone();
			var gameplayClone = this.Gameplay.Clone();
			var displayClone = this.Display.Clone();
			var graphicsClone = this.Graphics.Clone();
			
			return new SettingsData()
			{
				Audio = audioClone,
				Gameplay = gameplayClone,
				Display = displayClone,
				Graphics = graphicsClone
			};
		}
	}
}
