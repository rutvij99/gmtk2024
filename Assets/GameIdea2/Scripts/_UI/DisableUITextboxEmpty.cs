using System;
using System.Collections.Generic;
using Michsky.MUIP;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace GameIdea2.Audio._UI
{
    public class DisableUITextboxEmpty : MonoBehaviour
    {
        [SerializeField] private List<TMPro.TMP_InputField> inputFields;
        [SerializeField] private List<ButtonManager> targetButtons;

        private void Start()
        {
            foreach (var inputField in inputFields)
            {
                inputField.onValueChanged.AddListener(UpdateFields);
            }
        }

        private void UpdateFields(string str)
        {
            bool isInteractble = true;
            foreach (var inputField in inputFields)
            {
                if (String.IsNullOrEmpty(inputField.text))
                {
                    isInteractble = false;
                    break;
                }

            }
            
            foreach (var bttn in targetButtons)
            {
                bttn.Interactable(isInteractble);
            }
        }
    }
}