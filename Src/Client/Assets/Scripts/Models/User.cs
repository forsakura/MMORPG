using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class User : Singleton<User>
    {
        NUserInfo userInfo;

        public NCharacterInfo currentCharacter;

        public MapDefine curentMiniMap { get; set; }

        public UnityEngine.GameObject currentCharacterObject { get; set; }
        public NUserInfo Info
        {
            get { return userInfo; }
        }

        public void SetUserInfo(NUserInfo info)
        {
            userInfo = info;
        }

        internal void AddMoney(int value)
        {
            currentCharacter.Gold += value;
        }
    }
}