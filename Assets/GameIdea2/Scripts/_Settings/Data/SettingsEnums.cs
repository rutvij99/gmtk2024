using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace GravityWell.Core.Config
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum GraphicsPresets
	{
		low = 0,
		medium = 1,
		high = 2,
		ultra = 3
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum FpsLimit
	{
		unlimited = -1,
		_30 = 30,
		_60 = 60,
		_120 = 120,
		_240 = 240
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum VSync
	{
		off = 0,
		on = 1,
		half = 2
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum GraphicsQualityType
	{
		low = 0,
		medium = 1,
		high = 2,
		ultra = 3
	}

	[JsonConverter(typeof(StringEnumConverter))]
	public enum MSAAType
	{
		off = 0,
		x2 = 2,
		x4 = 4,
		x8 = 8
	}
	
	[JsonConverter(typeof(StringEnumConverter))]
	public enum FullscreenMode
	{
		exclusiveFullscreen = 0,
		fullscreen = 1,
		windowed = 2,
		borderless = 3,
	}
	
	
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Languages
	{
		English = 0,
	}
}
