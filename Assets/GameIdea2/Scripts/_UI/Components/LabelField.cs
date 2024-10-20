using System;
using GravityWell.Core.Config;
using TMPro;
using UnityEngine;

namespace GravityWell.UI
{
    public class LabelField : MonoBehaviour
    {
        
        [SerializeField] private bool useKey = false;
        [SerializeField] private string contentKey = "";
        [SerializeField] private string content = "";
        [SerializeField] private bool flashContent = false;
       
        private Language _currentLanguage;
        private TMP_Text labelText;
        
        public string Value => labelText.text;

        private void Awake()
        {
            _currentLanguage = Core.Config.GameConfig.Instance.SettingsDataProvider.GameplaySettings.Language;
            Core.Config.GameConfig.Instance.SettingsDataProvider.GameplaySettingsChanged += OnGameplaySettingsChanged;
            
            labelText = GetComponentInChildren<TMP_Text>();
            if (useKey && !string.IsNullOrEmpty(contentKey))
            {
                SetTextByKey(content);
            }
            else if (!string.IsNullOrEmpty(content))
            {
                SetText(content);
            }
        }

        private void OnGameplaySettingsChanged(IReadOnlyGameplaySettings obj)
        {
            // handle change
        }

        private void Start()
        {
            
        }
        
        public void SetTextByKey(string key)
        {
            // find content with key and then set key
            labelText.text = key;
        }
        
        public void SetText(string text)
        {
            // find localized content
            labelText.text = text;
        }
    }
}
