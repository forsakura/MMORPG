using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIMain : MonoSingleton<UIMain>
    {
        public Text avaterName;
        public Text avaterLevel;
        // Use this for initialization
        protected override void OnStart()
        {
            UpdaterView();
        }

        // Update is called once per frame
        void Update()
        {
            UpdaterView();
        }

        void UpdaterView()
        {
            if (User.Instance.currentCharacter == null) return;
            string name = User.Instance.currentCharacter.Name;
            string level = User.Instance.currentCharacter.Level.ToString();
            if (name != avaterName.text)
            {
                avaterName.text = name;
            }
            if (level != avaterLevel.text)
            {
                avaterLevel.text = level;
            }
        }
        public void BackToSelect()
        {
            SceneManager.Instance.LoadScene("CharacterSelect");
            UserService.Instance.SendGameLeave(true);
        }
    }
}