using DG.Tweening;
using UnityEngine;
using GravityWell.UI;


namespace GravityWell.MainMenu
{
    public class CreditsUI : MenuUI
    {
        [SerializeField] private float startYAnimationPosition;
        [SerializeField] private float endYAnimationPosition;
        [SerializeField] private float animationDuration;

        [SerializeField] private RectTransform creditsHolder;
        public override void Initialize(IMenuHandler handler)
        {
            Debug.Log($"CreditsUI Initializing -> isMain: {IsMain}");
            base.Initialize(handler);
        }

        public override void Enable()
        {
            if(SettingsUIHandler.Instance != null)
                SettingsUIHandler.Instance.ShowContextMenu(true);
            creditsHolder.DOKill();
            creditsHolder.anchoredPosition = new Vector2(0, startYAnimationPosition);
            ShowUI(true);
            creditsHolder.DOAnchorPosY(endYAnimationPosition, animationDuration);
        }

        public override void Disable()
        {
            if (SettingsUIHandler.Instance != null)
                SettingsUIHandler.Instance.ShowContextMenu(false);
            creditsHolder.DOKill();
            ShowUI(false);
        }
    }
}
