using System;
using UnityEngine;

namespace GravityWell.UI
{
    public class GameplaySettingsUI : SettingsUI
    {
        [SerializeField] private CarouselSelector languageSelector;
        [SerializeField] private AdvancedSlider panSlider;
        [SerializeField] private AdvancedSlider zoomSlider;


        private void Awake()
        {
            languageSelector.OnStringValueChanged.AddListener(LanguageChanged);
            panSlider.onValueChanged.AddListener(SettingsUIHandler.Instance.OnPanChanged);
            zoomSlider.onValueChanged.AddListener(SettingsUIHandler.Instance.OnZoomChanged);
        }

        public override void Enable()
        {
            base.Enable();
            SettingsUIHandler.Instance.ShowSelectContext(false);
            UpdateCurrentUI();
        }

        public override void Disable()
        {
            base.Disable();
        }




        public void UpdateCurrentUI()
        {
            languageSelector.value = Core.Config.GameConfig.Instance.SettingsDataProvider.GameplaySettings.Language.ToString();
            panSlider.value = Core.Config.GameConfig.Instance.SettingsDataProvider.GameplaySettings.PanSensitivity;
            zoomSlider.value = Core.Config.GameConfig.Instance.SettingsDataProvider.GameplaySettings.ZoomSensitivity;
        }
        private void LanguageChanged(string language)
        {
            
        }
        
        
        private void PanValueChanged(float panSensitivity)
        {
            SettingsUIHandler.Instance.OnPanChanged(panSensitivity);
        }

        private void ZoomValueChanged(float zoomSensitivity)
        {
            SettingsUIHandler.Instance.OnZoomChanged(zoomSensitivity);

        }
    }
}
