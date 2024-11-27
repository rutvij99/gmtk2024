using DG.Tweening;
using UnityEngine;
using GravityWell.UI;
using UnityEngine.Video;


namespace GravityWell.MainMenu
{
    public class ControlsUI : MenuUI
    {
        public override void Initialize(IMenuHandler handler)
        {
            Debug.Log($"CreditsUI Initializing -> isMain: {IsMain}");
            base.Initialize(handler);
        }

        public override void Enable()
        {
            if (SettingsUIHandler.Instance != null)
            {
                SettingsUIHandler.Instance.ShowContextMenu(true);
                SettingsUIHandler.Instance.ShowSelectContext(false);
            }
            ShowUI(true);
            SelectFirstElement();
        }

        public override void Disable()
        {
            if (SettingsUIHandler.Instance != null)
                SettingsUIHandler.Instance.ShowContextMenu(false);
            
            ShowUI(false);
        }

        private VideoPlayer currentVideoPlayer;
        public void OnSelectionChanged(GameObject videoObj)
        {
            if(currentVideoPlayer != null)
                currentVideoPlayer.Stop();
            
            currentVideoPlayer = videoObj.GetComponent<VideoPlayer>();
            currentVideoPlayer.Play();
        }
    }
}
