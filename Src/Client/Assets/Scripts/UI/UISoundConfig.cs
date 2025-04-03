using Assets.Scripts.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UISoundConfig : UIWindow
    {
        public Image MusicOff;
        public Image SoundOff;
        public Toggle ToggleMusic;
        public Toggle ToggleSound;
        public Slider SliderMusic;
        public Slider SliderSound;

        private void Start()
        {
            this.ToggleMusic.isOn = Config.MusicOn;
            this.ToggleSound.isOn = Config.SoundOn;
            this.SliderMusic.value = Config.MusicVolume;
            this.SliderSound.value = Config.SoundVolume;
        }

        public override void OnYesClick()
        {
            base.OnYesClick();
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
            PlayerPrefs.Save();
        }

        public void MusicToggle(bool on)
        {
            this.MusicOff.enabled = !on;
            Config.MusicOn = on;
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        }

        public void SoundToggle(bool on)
        {
            this.SoundOff.enabled = !on;
            Config.SoundOn = on;
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        }

        public void MusicVolume(int volume)
        {
            Config.MusicVolume = volume;
            PlaySound();
        }

        public void SoundVolume(int volume)
        {
            Config.SoundVolume = volume;
            PlaySound();
        }

        float lastPlay = 0;
        void PlaySound()
        {
            if(Time.realtimeSinceStartup - lastPlay > 0.1)
            {
                lastPlay = Time.realtimeSinceStartup;
                SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
            }
        }
    }
}