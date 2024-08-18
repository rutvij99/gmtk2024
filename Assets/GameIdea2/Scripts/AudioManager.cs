using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameIdea2.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;

        public static AudioManager Instance
        {
            get { return instance; }
        }

        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private List<AudioMap> audioMapList;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            instance = this;
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