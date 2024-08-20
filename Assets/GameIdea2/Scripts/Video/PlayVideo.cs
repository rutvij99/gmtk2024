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

        private VideoPlayer videoPlayer;

        private void Start()
        {
            videoPlayer = GetComponent<VideoPlayer>();
            PlayPanVideo();
        }

        public void PlayPanVideo()
        {
            PlaySelectedVideo(PAN);
        }

        public void PlayZoomInOutVideo()
        {
            PlaySelectedVideo(ZOOM_IN_OUT);
        }
        
        public void PlayScalingVideo()
        {
            PlaySelectedVideo(SCALING);
        }
        
        public void PlayMoveObjectVideo()
        {
            PlaySelectedVideo(MOVE_OBJECT);
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

