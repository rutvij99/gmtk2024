using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GameIdea2.Video
{
    public class PlayVideo : MonoBehaviour
    {
        private const string PAN = "pan_tut";
        private const string ZOOM_IN_OUT = "zoom_in_out";
        private const string SCALING = "Scaling_and_win";
        private const string MOVE_OBJECT = "move_object";

        [SerializeField] private TextMeshProUGUI descriptionBox;

        private VideoPlayer videoPlayer;

        private void Start()
        {
            videoPlayer = GetComponent<VideoPlayer>();
            PlayPanVideo();
        }

        public void PlayPanVideo()
        {
            PlaySelectedVideo(PAN);
            if (descriptionBox != null)
            {
                descriptionBox.text = "Hold middle mouse button to scroll.";
            }
        }

        public void PlayZoomInOutVideo()
        {
            PlaySelectedVideo(ZOOM_IN_OUT);
            if (descriptionBox != null)
            {
                descriptionBox.text = "Scroll up and down to zoom in and out.";
            }
        }
        
        public void PlayScalingVideo()
        {
            PlaySelectedVideo(SCALING);
            if (descriptionBox != null)
            {
                descriptionBox.text = "Hold right mouse button and drag to scale up or down.";
            }
        }
        
        public void PlayMoveObjectVideo()
        {
            PlaySelectedVideo(MOVE_OBJECT);
            if (descriptionBox != null)
            {
                descriptionBox.text = "Hold left mouse button to move and reach the target location to win.";
            }
        }
        
        private void PlaySelectedVideo(string videoName)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(this.transform.parent.GetComponent<RectTransform>());
            videoPlayer?.Stop();
            if (videoPlayer == null) videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.clip = Resources.Load<VideoClip>(videoName);
            
            videoPlayer?.Play();
        }
    }
}

