using System;
using UnityEngine;

namespace GravityWell.Core.Config
{
	public interface ISettingsProvider
	{
		Resolution[] AvailableScreenResolutions { get; }
		event Action SettingsChangeConfirmed; 
		event Action<IReadOnlyGameplaySettings> GameplaySettingsChanged;
		event Action<IReadOnlyAudioSettings> AudioSettingsChanged; 
		event Action<IReadOnlyDisplaySettings> DisplaySettingsChanged; 
		event Action<IReadOnlyGraphicsSettings> GraphicsSettingsChanged; 
		
		IReadOnlyAudioSettings AudioSettings { get; }
		IReadOnlyGameplaySettings GameplaySettings { get; }
		IReadOnlyDisplaySettings DisplaySettings { get; }
		IReadOnlyGraphicsSettings GraphicsSettings { get; }
	}

	public interface IReadOnlyGameplaySettings
	{
		Language Language { get; }
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
		int Resolution { get; }
		FullscreenMode FullscreenMode { get; }
		// int OutputMonitor { get; }
		
		
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
		GraphicsPresets Preset { get; }

		float RenderScale { get; }
		GraphicsQualityType TextureQuality { get; }
		bool Fxaa { get; }
		MSAAType Msaa { get; }
	}
}
