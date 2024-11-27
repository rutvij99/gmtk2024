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
                descriptionBox.text = "WASD or Press and Hold middle mouse button.";
            }
        }

        public void PlayZoomInOutVideo()
        {
            PlaySelectedVideo(ZOOM_IN_OUT);
            if (descriptionBox != null)
            {
                descriptionBox.text = "Use Scroll Wheel.";
            }
        }
        
        public void PlayScalingVideo()
        {
            PlaySelectedVideo(SCALING);
            if (descriptionBox != null)
            {
                descriptionBox.text = "Hold right mouse button and drag.";
            }
        }
        
        public void PlayMoveObjectVideo()
        {
            PlaySelectedVideo(MOVE_OBJECT);
            if (descriptionBox != null)
            {
                descriptionBox.text = "Hold left mouse button.";
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

