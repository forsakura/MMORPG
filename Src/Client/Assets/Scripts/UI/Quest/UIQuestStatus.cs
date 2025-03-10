using Assets.Scripts.Managers;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Quest
{
    public class UIQuestStatus : MonoBehaviour
    {
        public Image[] questImages;
        public NpcQuestStatus questStatus;
        internal void SetQuestStatus(NpcQuestStatus status)
        {
            questStatus = status;
            for (int i = 0; i < 4; i++)
            {
                if(questImages[i] != null)
                {
                    questImages[i].gameObject.SetActive(i == (int)status);
                }
            }
        }
    }
}