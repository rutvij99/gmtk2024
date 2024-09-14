using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.VisualScripting;
using UnityEngine;


namespace GravityWell.Core.Config
{
	[System.Serializable]
	public class DisplaySettings : IReadOnlyDisplaySettings
	{
		[SerializeField] private int resolution;
		// [SerializeField] private int outputMonitor;
		[SerializeField] private FullscreenMode fullscreenMode;
		[SerializeField] private FpsLimit fpsLimit = FpsLimit.unlimited;
		[SerializeField] private VSync vsync = VSync.off;
		
		
		[SerializeField] [Range(0,1f)] private float brightness = 1f;
		[SerializeField] [Range(0,1f)] private float contrast = 1f;
		[SerializeField] private bool hdr = false;
		
		// Post Processing Settings
		[SerializeField] private bool filmGrain = false;
		[SerializeField] private bool vignette = false;


		public int Resolution { get { return resolution; } internal set { resolution = value; } }
		// public int OutputMonitor { get { return outputMonitor; } internal set { outputMonitor = value; } }
		public FullscreenMode FullscreenMode { get { return fullscreenMode; } internal set { fullscreenMode = value; } }
		
		public FpsLimit FpsLimit { get { return fpsLimit; } internal set { fpsLimit = value; } }
		public VSync VSync { get { return vsync; } internal set { vsync = value; } }
		
		public float Brightness { get { return brightness; } internal set { brightness = value; } }
		public float Contrast { get { return contrast; } internal set { contrast = value; } }
		public bool Hdr { get { return hdr; } internal set { hdr = value; } }
		
		public bool FilmGrain { get { return filmGrain; } internal set { filmGrain = value; } }
		public bool Vignette { get { return vignette; } internal set { vignette = value; } }

		public DisplaySettings Clone()
		{
			return (DisplaySettings)this.MemberwiseClone();
		}
	}
}
