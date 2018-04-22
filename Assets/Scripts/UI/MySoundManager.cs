using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.SoundManagerNamespace
{
    public class MySoundManager : MonoBehaviour
    {
        public AudioSource[] soundAudioSources;
        public AudioSource[] musicAudioSources;
        public enum Sound
        {
            eMove,
            eGetCoin,
            eGetDiamond,
            eJump,
            eDamage,
            eGameOver,
            eGetLife,
            eGetClock,
            eGetFlag,
            eBossAlert
        }

        public enum Music
        {
            eMusicRetro
        }

        public void PlaySound(Sound eSound)
        {
            if (SoundManager.SoundVolume == 0)
                return;

            int index = (int)eSound;
            if (soundAudioSources.Length > index && soundAudioSources[index] && soundAudioSources[index].clip)
                soundAudioSources[index].PlayOneShotSoundManaged(soundAudioSources[index].clip);
            else
                Debug.Log(eSound.ToString() + " : Audio souces is not attached.");
        }

        public void PlayMusic(Music eMusic)
        {
            if (SoundManager.MusicVolume == 0)
                return;

            int index = (int)eMusic;
            if (musicAudioSources.Length > index && musicAudioSources[index] && musicAudioSources[index].clip)
                musicAudioSources[index].PlayLoopingMusicManaged(1.0f, 1.0f, true);
            else
                Debug.Log(eMusic.ToString() + " : Audio soures is not attached");

        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

