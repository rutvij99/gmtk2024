using System;
using UnityEngine;

namespace GravityWell.Core.Config
{
	public interface ISettingsProvider
	{
		event Action<IReadOnlyGameplaySettings> OnGamplaySettingsChanged;
		event Action<IReadOnlyAudioSettings> OnAudioSettingsChanged; 
		event Action<IReadOnlyDisplaySettings> OnDisplaySettingsChanged; 
		event Action<IReadOnlyGraphicsSettings> OnGraphicsSettingsChanged; 
		
		IReadOnlyAudioSettings AudioSettings { get; }
		IReadOnlyGameplaySettings GameplaySettings { get; }
		IReadOnlyDisplaySettings DisplaySettings { get; }
		IReadOnlyGraphicsSettings GraphicsSettings { get; }
	}

	public interface IReadOnlyGameplaySettings
	{
		float PanSensitivity { get; }
		float ZoomSensitivity { get; }
	}
	
	public interface IReadOnlyAudioSettings
	{
		float MasterVolume { get; }
		float Music { get; }
		float SFX { get; }
		bool Subtitles { get; }
	}
	
	public interface IReadOnlyDisplaySettings
	{
		float Brightness { get; }
		float Contrast { get; }
		bool Hdr { get; }
		
		FpsLimit FpsLimit { get; }
		VSync VSync { get; }
		
		bool FilmGrain { get; }
		bool Vignette { get; }
	}
	
	public interface IReadOnlyGraphicsSettings
	{
		float RenderScale { get; }
		GraphicsQualityType TextureQuality { get; }
		bool Fxaa { get; }
		MSAAType Msaa { get; }
	}
}
