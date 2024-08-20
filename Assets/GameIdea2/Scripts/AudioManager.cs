using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
        [SerializeField] private AudioSource bsAudioSource;
        [SerializeField] private AudioSource backgroundAudioSource;
        [SerializeField] private AudioSource ambienceAudioSource;
        [SerializeField] private List<AudioMap> audioMapList;
        [SerializeField] private List<AudioClip> backgroundMusicList;
        [SerializeField] private List<AudioClip> ambienceMusicList;

        [SerializeField] private List<AudioClip> bsClipsList;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            StartCoroutine(BsClipsPlayRoutine());
        }

        private IEnumerator BsClipsPlayRoutine()
        {
            Debug.Log("BS1");
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(10, 15));
                Debug.Log("BS");
                bsAudioSource.PlayOneShot(bsClipsList[Random.Range(0, bsClipsList.Count)]);
            }
        }

        public void ChangeBackgroundMusic(int levelNum)
        {
            int audioSelectIndex = levelNum % backgroundMusicList.Count;
            int ambienceSelectIndex = levelNum % backgroundMusicList.Count;

            backgroundAudioSource.clip = backgroundMusicList[audioSelectIndex];
            ambienceAudioSource.clip = ambienceMusicList[ambienceSelectIndex];
        }

        public void ChangeBackgroundMusic()
        {
            int random = Random.Range(0, backgroundMusicList.Count);
            backgroundAudioSource.clip = backgroundMusicList[random];
            ambienceAudioSource.clip = ambienceMusicList[random];
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