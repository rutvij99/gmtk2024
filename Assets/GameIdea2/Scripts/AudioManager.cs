using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameIdea2.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager Instance;

        public static AudioManager instance
        {
            get { return Instance; }
        }

        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private List<AudioMap> audioMapList;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }

        public void PlaySoundOfType(SoundTyes type)
        {
            if (sfxSource == null)
            {
                Debug.Log($"sfx source empty");
                return;
            }

            var clipFound = audioMapList.Find(i => i.soundType == type);
            if (clipFound != null)
            {
                sfxSource.PlayOneShot(clipFound.audioFile);
            }
        }

    }

    public enum SoundTyes
    {
        None,
        Boom,
        Success,
        UI
    }

    [Serializable]
    public class AudioMap
    {
        public SoundTyes soundType;
        public AudioClip audioFile;
    }
}