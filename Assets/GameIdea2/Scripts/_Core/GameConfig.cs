using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GravityWell.Helpers;
using UnityEngine;


namespace GravityWell.Core.Config
{
    [DefaultExecutionOrder(-100)]
    public class GameConfig : Singleton<GameConfig>
    {
        private SettingsHandler _settingsHandler;
        
        [SerializeField] private SettingsPreset _defaultSettingsPreset;
        [SerializeField] private List<GraphicsPresetSO> _graphicsPresets;
        
        
        public ISettingsProvider SettingsProvider => _settingsHandler;
        internal ISettingsModifier SettingsModifier => _settingsHandler;
        
        protected override void Awake()
        {
            base.Awake();
            if(Instance != this) return;
            DOTween.Init();
            _settingsHandler = new SettingsHandler(this);
            // _SettingsHandler.Testing();
            // _settingsHandler.LoadAllSettings();
        }


        internal SettingsPreset GetDefaultSettingsPreset() => _defaultSettingsPreset;

        internal GraphicsPresetSO GetGraphicsSO(GraphicsPresets preset)
        {
            return _graphicsPresets.FirstOrDefault(x => x.presetType == preset);
        }
    }
    
    
}
