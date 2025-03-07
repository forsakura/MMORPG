using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Quest
{
    public class UIQuestDialog : UIWindow
    {
        public UIQuestInfo questInfo;
        public GameObject openBtns;
        public GameObject submitBtns;
        public Models.Quest quest;
        public void SetQuest(Models.Quest quest)
        {
            this.quest = quest;
            UpdatQuest();
            if(this.quest.Info ==null)
            {
                openBtns.SetActive(true);
                submitBtns.SetActive(false);
            }
            else
            {
                if(this.quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
                {
                    openBtns.SetActive(true);
                    submitBtns.SetActive(false);
                }
                else
                {
                    openBtns.SetActive(false);
                    submitBtns.SetActive(false);
                }
            }    
        }

        void UpdatQuest()
        {
            if(this.quest != null)
            {
                if(this.quest.Info != null)
                {
                    questInfo.SetQuestInfo(quest);
                }
            }
        }
    }
}