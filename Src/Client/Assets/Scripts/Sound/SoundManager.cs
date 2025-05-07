using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Sound
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        public AudioMixer audioMixer;
        public AudioSource musicAudioSource;
        public AudioSource soundAudioSource;

        public const string MusicPath = "Music/";
        public const string SoundPath = "Sound/";

        public bool musicOn;
        public bool MusicOn
        {
            get
            {
                return musicOn;
            }
            set
            {
                musicOn = value;
                this.MusicMute(!musicOn);
            }
        }

        public bool soundOn;
        public bool SoundOn
        {
            get
            {
                return soundOn;
            }
            set
            {
                soundOn = value;
                this.SoundMute(!soundOn);
            }
        }

        public int musicVolume;
        public int MusicVolume
        {
            get
            {
                return musicVolume;
            }
            set
            {
                musicVolume = value;
                if (musicOn) this.SetVolume("Music", musicVolume);
            }
        }

        public int soundVolume;
        public int SoundVolume
        {
            get
            {
                return soundVolume;
            }
            set
            {
                soundVolume = value;
                if (soundOn) this.SetVolume("SFX", soundVolume);
            }
        }

        private void Start()
        {
            this.MusicVolume = Config.MusicVolume;
            this.SoundVolume = Config.SoundVolume;
            this.MusicOn = Config.MusicOn;
            this.SoundOn = Config.SoundOn;
        }

        public void MusicMute(bool mute)
        {
            this.SetVolume("Music", mute ? 0 : musicVolume);
        }

        public void SoundMute(bool mute)
        {
            this.SetVolume("SFX", mute ? 0 : soundVolume);
        }

        public void SetVolume(string name, int value)
        {
            float volume = value * 0.5f - 50f;
            this.audioMixer.SetFloat(name, volume);
        }

        public void PlayMusic(string name)
        {
            AudioClip clip = Resloader.Load<AudioClip>(MusicPath + name);
            if(clip == null)
            {
                return;
            }
            if(musicAudioSource.isPlaying)
            {
                musicAudioSource.Stop();
            }
            musicAudioSource.clip = clip;
            musicAudioSource.Play();
        }

        public void PlaySound(string name)
        {
            AudioClip clip = Resloader.Load<AudioClip>(SoundPath + name);
            if (clip == null)
            {
                return;
            }
            soundAudioSource.PlayOneShot(clip);
        }
    }
}