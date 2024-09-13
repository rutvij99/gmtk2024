using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace GravityWell.Core.Config
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum GraphicsPresets { low, medium, high, ultra }
	
	[JsonConverter(typeof(StringEnumConverter))]
	public enum FpsLimit { unlimited, _30, _60, _120, _240 }
	
	[JsonConverter(typeof(StringEnumConverter))]
	public enum VSync { off, on, half }
	
	[JsonConverter(typeof(StringEnumConverter))]
	public enum GraphicsQualityType { low, medium, high, ultra }
	
	[JsonConverter(typeof(StringEnumConverter))]
	public enum MSAAType { off, x2, x4, x8 }
}
