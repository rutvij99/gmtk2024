using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GravityWell.Common.Helpers;
using UnityEngine;


namespace GravityWell.Core.Config
{
    [DefaultExecutionOrder(-100)]
    public class GameConfig : Singleton<GameConfig>
    {
        private SettingsDataHandler _settingsDataHandler;
        private GraphicsController _graphicsController;
        
        [SerializeField] private SettingsPreset _defaultSettingsPreset;
        [SerializeField] private List<GraphicsPresetSO> _graphicsPresets;
        
        public ISettingsProvider SettingsDataProvider => _settingsDataHandler;
        internal ISettingsModifier SettingsDataModifier => _settingsDataHandler;
        
        
        public static bool IsConfigReady { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            if(Instance != this) return;
            DOTween.Init();
            _settingsDataHandler = new SettingsDataHandler(this);
            _graphicsController = new GraphicsController(this, _settingsDataHandler);
            IsConfigReady = true;
            // _SettingsHandler.Testing();
        }


        internal SettingsPreset GetDefaultSettingsPreset() => _defaultSettingsPreset;

        internal GraphicsPresetSO GetGraphicsSO(GraphicsPresets preset)
        {
            return _graphicsPresets.FirstOrDefault(x => x.presetType == preset);
        }
        
        
        /// <summary>
        /// Method to call when exiting a game. Do All Game Quit realted things from here
        /// </summary>
        public static void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
    
    
}
