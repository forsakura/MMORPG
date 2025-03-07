using Common.Data;
using GameServer.Managers;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Quest
    {
        public QuestDefine Define;
        public NQuestInfo Info;
        public Quest(NQuestInfo info)
        {
            Info = info;
            Define = DataManager.Instance.Quests[info.QuestId];
        }

        public Quest(QuestDefine define)
        {
            Define = define;
            Info = null;
        }

        public string GetTypeName()
        {
            return Define.Type.ToString();
        }
    }
}