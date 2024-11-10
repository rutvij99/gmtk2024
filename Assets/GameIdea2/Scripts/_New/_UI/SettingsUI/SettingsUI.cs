using UnityEngine;


namespace GravityWell.UI
{
    public class SettingsUI : MenuUI
    {
        public override void Enable()
        {
            if(SettingsUIHandler.Instance != null)
                SettingsUIHandler.Instance.ShowContextMenu(true);
            ShowUI(true);
            SelectFirstElement();
        }

        public override void Disable()
        {
            if(SettingsUIHandler.Instance != null)
                SettingsUIHandler.Instance.ShowContextMenu(false);
            ShowUI(false);
        }

        public override void GoBack()
        {

            if (SettingsUIHandler.Instance != null && SettingsUIHandler.Instance.IsConfirmationRequired)
            {
                // do confirmation stuff
                return;
            }
            base.GoBack();
        }
    }
}
