using UnityEngine;
using GravityWell.UI;


namespace GravityWell.MainMenu
{
    public class CreditsUI : MenuUI
    {
        [SerializeField] private float maxYAnimationPosition
        public override void Initialize(IMenuHandler handler)
        {
            Debug.Log($"CreditsUI Initializing -> isMain: {IsMain}");
            base.Initialize(handler);
        }

        public override void Enable()
        {
            
        }

        public override void Disable()
        {
            
        }
    }
}
