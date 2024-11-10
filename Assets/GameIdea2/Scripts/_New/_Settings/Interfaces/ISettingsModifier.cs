using System;
using UnityEngine;

namespace GravityWell.Core.Config
{
	public interface ISettingsModifier
	{
		void ModifyAudioSettings(Action<AudioSettings> modifyAction);
		void ModifyGameplaySettings(Action<GameplaySettings> modifyAction);
		void ModifyDisplaySettings(Action<DisplaySettings> modifyAction);
		void ModifyGraphicsSettings(Action<GraphicsSettings> modifyAction);

		void ApplySettings();
		void CancelChanges();
		void ResetToDefaults();
	}
}
