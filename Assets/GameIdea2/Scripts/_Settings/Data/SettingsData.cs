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

	[System.Serializable]
	public class GameplaySettings : IReadOnlyGameplaySettings
	{
		[SerializeField] [Range(0, 1f)] private float panSensitivity = 0.5f;
		[SerializeField] [Range(0, 1f)] private float zoomSensitivity = 0.5f;
		
		public float PanSensitivity { get { return panSensitivity; } internal set { panSensitivity = value; } }
		public float ZoomSensitivity { get { return zoomSensitivity; } internal set { zoomSensitivity = value; } }
		
		public GameplaySettings Clone()
		{
			return (GameplaySettings)this.MemberwiseClone();
		}
	}
	
	[System.Serializable]
	public class AudioSettings : IReadOnlyAudioSettings
	{
		[SerializeField] [Range(0,1f)] private float masterVolume = 1f;
		[SerializeField] [Range(0,1f)] private float music = 1f;
		[SerializeField] [Range(0,1f)] private float sfx = 1f;
		[SerializeField] private bool subtitles = false;
		
		public float MasterVolume { get { return masterVolume; } internal set { masterVolume = value; } }
		public float Music { get { return music; } internal set { music = value; } }
		public float SFX { get { return sfx; } internal set { sfx = value; } }
		public bool Subtitles { get { return subtitles; } internal set { subtitles = value; } }

		public AudioSettings Clone()
		{
			return (AudioSettings)this.MemberwiseClone();
		}
		
		public bool Equals(AudioSettings other)
		{
			if (other == null) return false;
			return MasterVolume == other.MasterVolume &&
			       Music == other.Music &&
			       SFX == other.SFX &&
			       Subtitles == other.Subtitles;
		}
		
		public override int GetHashCode()
		{
			// Combine hash codes of all properties
			return MasterVolume.GetHashCode() ^
			       Music.GetHashCode() ^
			       SFX.GetHashCode() ^
			       Subtitles.GetHashCode();
		}
	}
	
	[System.Serializable]
	public class DisplaySettings : IReadOnlyDisplaySettings
	{
		[SerializeField] [Range(0,1f)] private float brightness = 1f;
		[SerializeField] [Range(0,1f)] private float contrast = 1f;
		[SerializeField] private bool hdr = false;
		[SerializeField] private FpsLimit fpsLimit = FpsLimit.unlimited;
		[SerializeField] private VSync vsync = VSync.off;
		// Post Processing Settings
		[SerializeField] private bool filmGrain = false;
		[SerializeField] private bool vignette = false;
		
		
		public float Brightness { get { return brightness; } internal set { brightness = value; } }
		public float Contrast { get { return contrast; } internal set { contrast = value; } }
		public bool Hdr { get { return hdr; } internal set { hdr = value; } }
		
		public FpsLimit FpsLimit { get { return fpsLimit; } internal set { fpsLimit = value; } }
		public VSync VSync { get { return vsync; } internal set { vsync = value; } }
		
		public bool FilmGrain { get { return filmGrain; } internal set { filmGrain = value; } }
		public bool Vignette { get { return vignette; } internal set { vignette = value; } }

		public DisplaySettings Clone()
		{
			return (DisplaySettings)this.MemberwiseClone();
		}
	}
	
	[System.Serializable]
	public class GraphicsSettings : IReadOnlyGraphicsSettings
	{
		// [SerializeField] private Resolution resolution;
		[SerializeField] [Range(0, 2f)] private float renderScale = 1f;
		[SerializeField] private GraphicsQualityType textureQuality = GraphicsQualityType.high;
		[SerializeField] private bool fxaa = true;
		[SerializeField] private MSAAType msaa = MSAAType.off;


		public float RenderScale { get => renderScale; internal set => renderScale = value; }
		public GraphicsQualityType TextureQuality { get => textureQuality; internal set => textureQuality = value;  }
		public bool Fxaa { get => fxaa; internal set => fxaa = value; }
		public MSAAType Msaa { get => msaa; internal set => msaa = value; }
		
		

		public GraphicsSettings Clone()
		{
			return (GraphicsSettings)this.MemberwiseClone();
		}
	}
}
