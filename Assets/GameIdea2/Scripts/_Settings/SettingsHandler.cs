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
	public class SettingsHandler : ISettingsProvider
	{
		private GameConfig _config;
		private SettingsData _settingsData;
		
		private SettingsData _readOnlySettingsData;
		
		private string configFilePath = Path.Combine(Application.persistentDataPath, "GameData", "config.ini");

		
		internal SettingsHandler(GameConfig config)
		{
			_config = config;
		}
		
		#region Provider Setup
		public event Action<IReadOnlyGameplaySettings> OnGamplaySettingsChanged;
		public event Action<IReadOnlyAudioSettings> OnAudioSettingsChanged;
		public event Action<IReadOnlyDisplaySettings> OnDisplaySettingsChanged;
		public event Action<IReadOnlyGraphicsSettings> OnGraphicsSettingsChanged;
		
		public IReadOnlyGameplaySettings GameplaySettings => _settingsData.Gameplay;
		public IReadOnlyAudioSettings AudioSettings => _settingsData.Audio;
		public IReadOnlyDisplaySettings DisplaySettings => _settingsData.Display;
		public IReadOnlyGraphicsSettings GraphicsSettings => _settingsData.Graphics;
		#endregion
		
		internal void SaveAllSettings()
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

		internal List<string> LoadAllSettings()
		{
			if (!File.Exists(configFilePath))
			{
				Debug.LogWarning("Config file not found, performing first-time setup.");
				FirstTimeSetup();
				return null;
			}

			string[] iniLines = File.ReadAllLines(configFilePath);

			if (iniLines == null)
			{
				Debug.LogWarning("empty ini file, performing first-time setup.");
				FirstTimeSetup();
				return null;
			}
			
			_settingsData = new SettingsData
			{
				Gameplay = LoadSettings<GameplaySettings>(iniLines),
				Audio = LoadSettings<AudioSettings>(iniLines),
				Display = LoadSettings<DisplaySettings>(iniLines),
				Graphics = LoadSettings<GraphicsSettings>(iniLines)
			};

			return iniLines.ToList();
		}
		
		private void FirstTimeSetup()
		{
			ResetToDefaults();
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
					iniData.Add($"{field.Name}={field.GetValue(settings)}");
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
								object convertedValue = field.FieldType.IsEnum
									? Enum.Parse(field.FieldType, value)
									: Convert.ChangeType(value, field.FieldType);
								field.SetValue(settings, convertedValue);
								if (field.IsDefined(typeof(RangeAttribute), true))
								{
									var range = (RangeAttribute)field.GetCustomAttribute(typeof(RangeAttribute), true);
									if (field.FieldType == typeof(float))
									{
										float floatValue = (float)convertedValue;
										floatValue = Mathf.Clamp(floatValue, range.min, range.max);
										convertedValue = floatValue;
									}
									// Handle other types if necessary
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
			_settingsData = new SettingsData()
			{
				Audio = _config.GetDefaultSettingsPreset().Audio,
				Display = _config.GetDefaultSettingsPreset().Display,
				Gameplay = _config.GetDefaultSettingsPreset().Gameplay,
				Graphics = _config.GetGraphicsSO(_config.GetDefaultSettingsPreset().defaultGraphicsPreset).settings,
			};
			_settingsData.Audio.Music = 0f;

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
		
		public void ResetToDefaults()
		{
			_settingsData = new SettingsData()
			{
				Audio = _config.GetDefaultSettingsPreset().Audio,
				Display = _config.GetDefaultSettingsPreset().Display,
				Gameplay = _config.GetDefaultSettingsPreset().Gameplay,
				Graphics = _config.GetGraphicsSO(_config.GetDefaultSettingsPreset().defaultGraphicsPreset).settings,
			};
		}
	}
}