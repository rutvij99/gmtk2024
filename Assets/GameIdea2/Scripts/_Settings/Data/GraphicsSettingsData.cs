using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.VisualScripting;
using UnityEngine;


namespace GravityWell.Core.Config
{
	[System.Serializable]
	public class GraphicsSettings : IReadOnlyGraphicsSettings
	{
		[SerializeField] private GraphicsPresets preset = GraphicsPresets.high;
		
		
		[SerializeField] [Range(0, 2f)] private float renderScale = 1f;
		[SerializeField] private GraphicsQualityType textureQuality = GraphicsQualityType.high;
		[SerializeField] private bool fxaa = true;
		[SerializeField] private MSAAType msaa = MSAAType.off;


		public GraphicsPresets Preset { get { return preset; } internal set { preset = value; } }
		
		
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
