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
		unlimited,
		_30,
		_60,
		_120,
		_240
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
		off,
		x2,
		x4,
		x8
	}
}
