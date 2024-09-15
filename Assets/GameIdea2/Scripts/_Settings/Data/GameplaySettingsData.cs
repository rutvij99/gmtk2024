using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.VisualScripting;
using UnityEngine;


namespace GravityWell.Core.Config
{
	[System.Serializable]
	public class GameplaySettings : IReadOnlyGameplaySettings
	{
		[SerializeField] private Languages language = Languages.English;
		[SerializeField] [Range(0, 1f)] private float panSensitivity = 0.5f;
		[SerializeField] [Range(0, 1f)] private float zoomSensitivity = 0.5f;
		
		public Languages Language { get { return language; } set { language = value; } }
		public float PanSensitivity { get { return panSensitivity; } internal set { panSensitivity = value; } }
		public float ZoomSensitivity { get { return zoomSensitivity; } internal set { zoomSensitivity = value; } }
		
		public GameplaySettings Clone()
		{
			return (GameplaySettings)this.MemberwiseClone();
		}
	}
}
