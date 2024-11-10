using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.VisualScripting;
using UnityEngine;


namespace GravityWell.Core.Config
{
	[System.Serializable]
	public class AudioSettings : IReadOnlyAudioSettings
	{
		[SerializeField] [Range(0,1f)] private float masterVolume = 1f;
		[SerializeField] [Range(0,1f)] private float music = 1f;
		[SerializeField] [Range(0,1f)] private float sfx = 1f;
		[SerializeField] private bool subtitles = false;
		
		public float MasterVolume { get { return masterVolume; } internal set { masterVolume = value; } }
		public float Music { get { return music; } internal set { music = value; } }
		public float SFX { get { return sfx; } internal set { sfx = value; } }
		public bool Subtitles { get { return subtitles; } internal set { subtitles = value; } }

		public AudioSettings Clone()
		{
			return (AudioSettings)this.MemberwiseClone();
		}
		
		public bool Equals(AudioSettings other)
		{
			if (other == null) return false;
			return MasterVolume == other.MasterVolume &&
			       Music == other.Music &&
			       SFX == other.SFX &&
			       Subtitles == other.Subtitles;
		}
		
		public override int GetHashCode()
		{
			// Combine hash codes of all properties
			return MasterVolume.GetHashCode() ^
			       Music.GetHashCode() ^
			       SFX.GetHashCode() ^
			       Subtitles.GetHashCode();
		}
	}
}
