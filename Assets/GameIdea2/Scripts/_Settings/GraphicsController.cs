using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


namespace GravityWell.Core.Config
{
	public class GraphicsController
	{
		private GameConfig _config;
		private ISettingsProvider _settingsProvider;
		private UniversalRenderPipelineAsset _urpAsset;


		public GraphicsController(GameConfig config, ISettingsProvider settingsProvider)
		{
			_config = config;
			_settingsProvider = settingsProvider;
			
			_settingsProvider.SettingsChangeConfirmed += SettingsProviderOnSettingsChangeConfirmed;
			_settingsProvider.DisplaySettingsChanged += SettingsProviderOnDisplaySettingsChanged;
			_settingsProvider.GraphicsSettingsChanged += SettingsProviderOnGraphicsSettingsChanged;
			
			// _urpAsset = (UniversalRenderPipelineAsset)UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline;
			_urpAsset = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;
			SettingsProviderOnSettingsChangeConfirmed();
		}

		private void SettingsProviderOnSettingsChangeConfirmed()
		{
			SettingsProviderOnGraphicsSettingsChanged(_settingsProvider.GraphicsSettings);
			SettingsProviderOnDisplaySettingsChanged(_settingsProvider.DisplaySettings);
		}

		private void SettingsProviderOnGraphicsSettingsChanged(IReadOnlyGraphicsSettings settings)
		{
			ApplyGraphicsPreset(settings.Preset, false);
			
			ApplyRenderScale(settings.RenderScale);
			ApplyAntiAliasing(settings.Msaa, settings.Fxaa);
		}
		
		private void SettingsProviderOnDisplaySettingsChanged(IReadOnlyDisplaySettings settings)
		{
			ApplyResolutionAndFullscreen(settings.Resolution, settings.FullscreenMode);
			ApplyHdr(settings.Hdr);
			ApplyVSync(settings.VSync);
			ApplyFpsLimit(settings.FpsLimit);

			// ApplyBrightnessAndContrast(settings.Brightness, settings.Contrast);
			// ApplyPostProcessingEffects(settings.FilmGrain, settings.Vignette);
		}

		private void ApplyResolutionAndFullscreen(int resolutionIndex, FullscreenMode fullscreenMode)
		{
			if (resolutionIndex >= 0 && resolutionIndex < _settingsProvider.AvailableScreenResolutions.Length)
			{
				Resolution resolution = _settingsProvider.AvailableScreenResolutions[resolutionIndex];
				Screen.SetResolution(resolution.width, resolution.height, GetFullScreenMode(fullscreenMode));
				Debug.Log($"Resolution set to {resolution.width}x{resolution.height}, Fullscreen Mode: {fullscreenMode}");
			}
			else
			{
				Debug.LogWarning($"Invalid resolution index: {resolutionIndex}");
			}
		}

		private void ApplyFpsLimit(FpsLimit fpsLimit)
		{
			int targetFrameRate = GetFpsLimitValue(fpsLimit);
			Application.targetFrameRate = targetFrameRate;
			Debug.Log($"FPS Limit set to {targetFrameRate}");
		}
		
		private void ApplyVSync(VSync vSync)
		{
			QualitySettings.vSyncCount = GetVSyncCount(vSync);
			Debug.Log($"VSync set to {vSync}");
		}
		
		private void ApplyHdr(bool hdrEnabled)
		{
			// do this on every camera here or in their respective scenes
			if(Camera.main != null)
				Camera.main.allowHDR = hdrEnabled;
			Debug.Log($"HDR set to {hdrEnabled}");
		}
		
		
		
		private void ApplyGraphicsPreset(GraphicsPresets preset, bool applyExpensiveChanges)
		{
			string[] unityQualitySettingsNames = QualitySettings.names;
			string presetName = preset.ToString(); // Converts enum to string ("low", "medium", etc.)

			int qualityLevelIndex = Array.FindIndex(unityQualitySettingsNames, name => name.Equals(presetName, StringComparison.OrdinalIgnoreCase));

			if (qualityLevelIndex != -1)
			{
				Debug.Log($"[Graphics Controller] Quality settings set to {unityQualitySettingsNames[qualityLevelIndex]} (level {qualityLevelIndex})");
				QualitySettings.SetQualityLevel(qualityLevelIndex, applyExpensiveChanges);
			}
			else
			{
				Debug.LogWarning($"[Graphics Controller] No matching quality setting found for preset '{presetName}'.");
			}

			_urpAsset = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;
		}
		
		private void ApplyRenderScale(float renderScale)
		{
			if (_urpAsset == null) return;
			_urpAsset.renderScale = renderScale;
			
#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(_urpAsset);
#endif
		}
		
		private void ApplyAntiAliasing(MSAAType msaa, bool fxaa)
		{
			if (_urpAsset == null) return;
			_urpAsset.msaaSampleCount = GetMSAASampleCount(msaa);
			// Apply FXAA through camera or pp
#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(_urpAsset);
#endif
		}
		
		private void ApplyTextureQuality(GraphicsQualityType textureQuality)
		{
			QualitySettings.globalTextureMipmapLimit = GetTextureQualityLevel(textureQuality);
		}

		
		
		
		private int GetFpsLimitValue(FpsLimit fpsLimit)
		{
			return fpsLimit switch
			{
				FpsLimit._30 => 30,
				FpsLimit._60 => 60,
				FpsLimit._120 => 120,
				FpsLimit._240 => 240,
				_ => -1
			};
		}
		
		private int GetTextureQualityLevel(GraphicsQualityType quality)
		{
			return quality switch
			{
				GraphicsQualityType.ultra => 0, // Full resolution
				GraphicsQualityType.high => 1, // Half resolution
				GraphicsQualityType.medium => 2, // Quarter resolution
				GraphicsQualityType.low => 3, // Eighth resolution
				_ => 0
			};
		}
		
		private int GetMSAASampleCount(MSAAType msaa)
		{
			return msaa switch
			{
				MSAAType.off => 1,
				MSAAType.x2 => 2,
				MSAAType.x4 => 4,
				MSAAType.x8 => 8,
				_ => 1
			};
		}
		
		private int GetVSyncCount(VSync vSync)
		{
			return vSync switch
			{
				VSync.off => 0,
				VSync.on => 1,
				VSync.half => 2,
				_ => 0
			};
		}
		
		private UnityEngine.FullScreenMode GetFullScreenMode(FullscreenMode fullscreenMode)
		{
			return fullscreenMode switch
			{
				FullscreenMode.exclusiveFullscreen => UnityEngine.FullScreenMode.ExclusiveFullScreen,
				FullscreenMode.fullscreen => FullScreenMode.ExclusiveFullScreen,
				FullscreenMode.borderless => FullScreenMode.FullScreenWindow,
				FullscreenMode.windowed => FullScreenMode.Windowed,
				_ => FullScreenMode.FullScreenWindow
			};
		}
	}
}
