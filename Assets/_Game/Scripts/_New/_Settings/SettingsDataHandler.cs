using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;


namespace GravityWell.Core.Config
{
	[System.Serializable]
	internal class SettingsDataHandler : ISettingsProvider , ISettingsModifier
	{
		private GameConfig _config;
		private SettingsData _settingsData;
		
		private SettingsData _readOnlySettingsData;

		public Resolution[] AvailableScreenResolutions => Screen.resolutions;
		
		private string configFilePath = Path.Combine(Application.persistentDataPath, "GameData", "config.ini");
		

		internal SettingsDataHandler(GameConfig config)
		{
			_config = config;
			LoadAllSettings();
			GameplaySettingsChanged?.Invoke(_settingsData.Gameplay);
			AudioSettingsChanged?.Invoke(_settingsData.Audio);
			DisplaySettingsChanged?.Invoke(_settingsData.Display);
			GraphicsSettingsChanged?.Invoke(_settingsData.Graphics);
			SettingsChangeConfirmed?.Invoke();
		}
		
		#region Provider Setup

		public event Action SettingsChangeConfirmed;
		public event Action<IReadOnlyGameplaySettings> GameplaySettingsChanged;
		public event Action<IReadOnlyAudioSettings> AudioSettingsChanged;
		public event Action<IReadOnlyDisplaySettings> DisplaySettingsChanged;
		public event Action<IReadOnlyGraphicsSettings> GraphicsSettingsChanged;
		
		public IReadOnlyGameplaySettings GameplaySettings => _settingsData.Gameplay;
		public IReadOnlyAudioSettings AudioSettings => _settingsData.Audio;
		public IReadOnlyDisplaySettings DisplaySettings => _settingsData.Display;
		public IReadOnlyGraphicsSettings GraphicsSettings => _settingsData.Graphics;
		#endregion
		
		#region Mofidier Setup
		public void ModifyAudioSettings(Action<AudioSettings> modifyAction)
		{
			if (modifyAction == null) return;
			var originalSettings = _settingsData.Audio.Clone();
			modifyAction(_settingsData.Audio);
			if (!originalSettings.Equals(_settingsData.Audio))
			{
				AudioSettingsChanged?.Invoke(_settingsData.Audio);
			}
		}
		public void ModifyGameplaySettings(Action<GameplaySettings> modifyAction)
		{
			if (modifyAction == null) return;
			var originalSettings = _settingsData.Gameplay.Clone();
			modifyAction(_settingsData.Gameplay);
			if (!originalSettings.Equals(_settingsData.Gameplay))
			{
				GameplaySettingsChanged?.Invoke(_settingsData.Gameplay);
			}
		}

		public void ModifyDisplaySettings(Action<DisplaySettings> modifyAction)
		{
			if (modifyAction == null) return;
			var originalSettings = _settingsData.Display.Clone();
			modifyAction(_settingsData.Display);
			if (!originalSettings.Equals(_settingsData.Display))
			{
				DisplaySettingsChanged?.Invoke(_settingsData.Display);
			}
		}

		public void ModifyGraphicsSettings(Action<GraphicsSettings> modifyAction)
		{
			if (modifyAction == null) return;
			var originalSettings = _settingsData.Graphics.Clone();
			modifyAction(_settingsData.Graphics);
			if (!originalSettings.Equals(_settingsData.Graphics))
			{
				GraphicsSettingsChanged?.Invoke(_settingsData.Graphics);
			}
		}
		
		public void ApplySettings()
		{
			if (_settingsData == null) return;
			SaveAllSettings();
			GameplaySettingsChanged?.Invoke(_settingsData.Gameplay);
			AudioSettingsChanged?.Invoke(_settingsData.Audio);
			DisplaySettingsChanged?.Invoke(_settingsData.Display);
			GraphicsSettingsChanged?.Invoke(_settingsData.Graphics);
		}

		public void CancelChanges()
		{
			LoadAllSettings();
			GameplaySettingsChanged?.Invoke(_settingsData.Gameplay);
			AudioSettingsChanged?.Invoke(_settingsData.Audio);
			DisplaySettingsChanged?.Invoke(_settingsData.Display);
			GraphicsSettingsChanged?.Invoke(_settingsData.Graphics);
		}
		
		public void ResetToDefaults()
		{
			_settingsData = new SettingsData()
			{
				Audio = _config.GetDefaultSettingsPreset().Audio.Clone(),
				Display = _config.GetDefaultSettingsPreset().Display.Clone(),
				Gameplay = _config.GetDefaultSettingsPreset().Gameplay.Clone(),
				Graphics = _config.GetGraphicsSO(_config.GetDefaultSettingsPreset().defaultGraphicsPreset).settings.Clone(),
			};
		}
		#endregion

		
		private void SaveAllSettings()
		{
			List<string> iniData = new List<string>();
			iniData.AddRange(SaveSettings<GameplaySettings>(_settingsData.Gameplay));
			iniData.AddRange(SaveSettings<AudioSettings>(_settingsData.Audio));
			iniData.AddRange(SaveSettings<DisplaySettings>(_settingsData.Display));
			iniData.AddRange(SaveSettings<GraphicsSettings>(_settingsData.Graphics));
			
			string directory = Path.GetDirectoryName(configFilePath);
			if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

			File.WriteAllLines(configFilePath, iniData);
			Debug.Log("Settings saved to: " + configFilePath);
		}

		private void LoadAllSettings()
		{
			if (!File.Exists(configFilePath))
			{
				Debug.LogWarning("Config file not found, performing first-time setup.");
				FirstTimeSetup();
				return;
			}

			string[] iniLines = File.ReadAllLines(configFilePath);

			if (iniLines == null)
			{
				Debug.LogWarning("empty ini file, performing first-time setup.");
				FirstTimeSetup();
				return;
			}
			
			_settingsData = new SettingsData
			{
				Gameplay = LoadSettings<GameplaySettings>(iniLines),
				Audio = LoadSettings<AudioSettings>(iniLines),
				Display = LoadSettings<DisplaySettings>(iniLines),
				Graphics = LoadSettings<GraphicsSettings>(iniLines)
			};
		}
		
		private void FirstTimeSetup()
		{
			ResetToDefaults();
			_settingsData.Display.Resolution = GetCurrentResolutionIndex();
			SaveAllSettings();
		}
		
		private static List<string> SaveSettings<T>(T settings)
		{
			Type type = typeof(T);
			if (settings == null) return null;
			List<string> iniData = new List<string> { $"[{type.Name}]" };

			foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if (!field.FieldType.IsClass)
				{
					object value = field.GetValue(settings);
					// iniData.Add($"{field.Name}={value}");
            
					if (field.FieldType.IsEnum)
					{
						if(field.Name == "fpsLimit")
							Debug.Log($"Setting {field.Name}: {value} -> {(int)value}");
						iniData.Add($"{field.Name}={(int)value}");
					}
					else if (field.FieldType == typeof(bool))
					{
						iniData.Add($"{field.Name}={((bool)value ? 1 : 0)}");
					}
					else
					{
						iniData.Add($"{field.Name}={value}");
					}
				}
			}
			return iniData;
		}
		
		private static T LoadSettings<T>(string[] iniLines) where T : new()
		{
			T settings = new T();
			Type type = typeof(T);
			string currentSection = "";

			foreach (string line in iniLines)
			{
				if (string.IsNullOrWhiteSpace(line)) continue;

				if (line.StartsWith("[") && line.EndsWith("]"))
				{
					currentSection = line.Trim('[', ']');
				}
				else if (currentSection == type.Name)
				{
					string[] keyValue = line.Split('=');
					if (keyValue.Length == 2)
					{
						string key = keyValue[0].Trim();
						string value = keyValue[1].Trim();

						FieldInfo field = type.GetField(key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
						if (field != null)
						{
							try
							{
								// Load enum from int
								if (field.FieldType.IsEnum)
								{
									object enumValue = Enum.ToObject(field.FieldType, int.Parse(value));
									field.SetValue(settings, enumValue);
								}
								// Load boolean from 0 or 1
								else if (field.FieldType == typeof(bool))
								{
									bool boolValue = value == "1";
									field.SetValue(settings, boolValue);
								}
								else if (field.FieldType == typeof(int) || field.FieldType == typeof(float))
								{
									var range = (RangeAttribute)field.GetCustomAttribute(typeof(RangeAttribute), true);
									if (field.FieldType == typeof(float))
									{
										float floatValue = (float)field.GetValue(settings);
										floatValue = Mathf.Clamp(floatValue, range.min, range.max);
										field.SetValue(settings, floatValue);
									}
								}
								else
								{
									object convertedValue = Convert.ChangeType(value, field.FieldType);
									field.SetValue(settings, convertedValue);
								}
							}
							catch (Exception ex)
							{
								Debug.LogError($"Error parsing field '{key}': {ex.Message}");
							}
						}
					}
				}
			}

			return settings;
		}
		
		internal void Testing()
		{
			for (var index = 0; index < AvailableScreenResolutions.Length; index++)
			{
				var resolution = AvailableScreenResolutions[index];
				Debug.Log($"r: {index} {resolution.width}x{resolution.height}");
			}
			Debug.Log($"r: current res ->s {GetCurrentResolutionIndex()} {Screen.currentResolution.width}x{Screen.currentResolution.height}");

			string[] names = QualitySettings.names;
			for (int i = 0; i < names.Length; i++)
			{
				Debug.Log($"r: quality settings: {i} -> {names[i]}");
			}
			Debug.Log($"current quality settings -> {QualitySettings.GetQualityLevel()}");

			_settingsData = new SettingsData()
			{
				Audio = _config.GetDefaultSettingsPreset().Audio,
				Display = _config.GetDefaultSettingsPreset().Display,
				Gameplay = _config.GetDefaultSettingsPreset().Gameplay,
				Graphics = _config.GetGraphicsSO(_config.GetDefaultSettingsPreset().defaultGraphicsPreset).settings,
			};
			_settingsData.Display.Resolution = GetCurrentResolutionIndex();

			
			ModifyAudioSettings(audioSettings => audioSettings.MasterVolume += 0.1f);

			ModifyGraphicsSettings(graphicsSettings =>
			{
				graphicsSettings.RenderScale = 1.5f;
				graphicsSettings.TextureQuality = GraphicsQualityType.ultra;
			});
			
			ModifyAudioSettings(audioSettings => audioSettings.Subtitles = !audioSettings.Subtitles);

			List<string> iniData = new List<string>();
			iniData.AddRange(SaveSettings<GameplaySettings>(_settingsData.Gameplay));
			iniData.AddRange(SaveSettings<AudioSettings>(_settingsData.Audio));
			iniData.AddRange(SaveSettings<DisplaySettings>(_settingsData.Display));
			iniData.AddRange(SaveSettings<GraphicsSettings>(_settingsData.Graphics));

			var stringData = "";
			foreach (var data in iniData) stringData += $"{data}\n";
			Debug.Log($"r: {stringData}");
			
			
			_settingsData.Audio.Music = 1f;
			var iniDataArray = iniData.ToArray();
			var loadedSettings = new SettingsData
			{
				Gameplay = LoadSettings<GameplaySettings>(iniDataArray),
				Audio = LoadSettings<AudioSettings>(iniDataArray),
				Display = LoadSettings<DisplaySettings>(iniDataArray),
				Graphics = LoadSettings<GraphicsSettings>(iniDataArray)
			};
			Debug.Log($"r: reloaded -> \n{JsonConvert.SerializeObject(loadedSettings, Formatting.Indented)}");
		}
		
		private int GetCurrentResolutionIndex()
		{
			var resolutions = Screen.resolutions;
			Resolution currentResolution = Screen.currentResolution;
			for (int i = 0; i < resolutions.Length; i++)
			{
				if (resolutions[i].width == currentResolution.width &&
				    resolutions[i].height == currentResolution.height &&
				    resolutions[i].refreshRate == currentResolution.refreshRate)
				{
					return i;
				}
			}

			// If no exact match is found, return a default index (e.g., 0)
			return 0;
		}

	}
}