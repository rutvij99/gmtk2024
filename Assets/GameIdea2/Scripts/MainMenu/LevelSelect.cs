using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectTemplate;
    [SerializeField] private Transform levelSelectHolder;
    [SerializeField] private List<Button> levels;
    
    private void Start()
    {
        var lastUnlocked = GameConfig.GetLastCompletedLevel();
        for (int i = 0; i < GameConfig.MAX_LEVELS; i++)
        {
            var obj = Instantiate(levelSelectTemplate, levelSelectHolder);
            var btn = obj.GetComponentInChildren<Button>();
            btn.GetComponentInChildren<TMP_Text>().text = $"Level {i + 1}";
            var level = i;
            if (i > lastUnlocked)
            {
                btn.interactable = false;
            }
            else
            {
                btn.onClick.AddListener(()=>
                {
                    this.GetComponent<MainMenu>().LoadLevel(level);
                    this.GetComponent<MainMenu>().ClickSFX();
                });
            }
            levels.Add(btn);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(levelSelectHolder.GetComponent<RectTransform>());
    }
}
