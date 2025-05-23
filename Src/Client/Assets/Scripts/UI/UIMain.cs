﻿using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.UI.CharEquip;
using Assets.Scripts.UI.Friend;
using Assets.Scripts.UI.Quest;
using Assets.Scripts.UI.Ride;
using Assets.Scripts.UI.Setting;
using Assets.Scripts.UI.Team;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIMain : MonoSingleton<UIMain>
    {
        public Text avaterName;
        public Text avaterLevel;

        public UITeamSystem UITeam;
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

        public void OnClickBag()
        {
            Debug.LogFormat("Open Bag");
            UIManager.Instance.Show<UI.UIBag.UIBag>();
        }

        public void OnClickChar()
        {
            UIManager.Instance.Show<UICharEquip>();
        }

        public void OnClickQuest()
        {
            Debug.Log("ClickQuest");
            UIManager.Instance.Show<UIQuestSystem>();
        }

        public void OnClickFriend()
        {
            UIManager.Instance.Show<UIFriend>();
        }

        public void OnClickGuild()
        {
            GuildManager.Instance.ShowGuild();
        }

        public void OnClickRide()
        {
            UIManager.Instance.Show<UIRide>();
        }

        public void OnClickSetting()
        {
            UIManager.Instance.Show<UISetting>();
        }

        public void OnClickSkill()
        {

        }

        public void ShowTeamUI(bool show)
        {
            UITeam.Show(show);
        }
    }
}