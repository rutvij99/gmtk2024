using System;
using UnityEngine;

namespace GravityWell.Core.Config
{
	public interface ISettingsProvider
	{
		public event Action<IReadOnlyGameplaySettings> OnGamplaySettingsChanged;
		public event Action<IReadOnlyAudioSettings> OnAudioSettingsChanged; 
		public event Action<IReadOnlyDisplaySettings> OnDisplaySettingsChanged; 
		public event Action<IReadOnlyGraphicsSettings> OnGraphicsSettingsChanged; 
		
		IReadOnlyAudioSettings AudioSettings { get; }
		IReadOnlyGameplaySettings GameplaySettings { get; }
		IReadOnlyDisplaySettings DisplaySettings { get; }
		IReadOnlyGraphicsSettings GraphicsSettings { get; }
	}

	public interface IReadOnlyGameplaySettings
	{
		public float PanSensitivity { get; }
		public float ZoomSensitivity { get; }
	}
	
	public interface IReadOnlyAudioSettings
	{
		public float MasterVolume { get; }
		public float Music { get; }
		public float SFX { get; }
		public bool Subtitles { get; }
	}
	
	public interface IReadOnlyDisplaySettings
	{
		public float Brightness { get; }
		public float Contrast { get; }
		public bool Hdr { get; }
		
		public FpsLimit FpsLimit { get; }
		public VSync VSync { get; }
		
		public bool FilmGrain { get; }
		public bool Vignette { get; }
	}
	
	public interface IReadOnlyGraphicsSettings
	{
		public float RenderScale { get; }
		public GraphicsQualityType TextureQuality { get; }
		public bool Fxaa { get; }
		public MSAAType Msaa { get; }
	}
}
