using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Quest
{
    public class UIQuestInfo : MonoBehaviour
    {
        public Text title;
        public Text questDiscribution;
        public Text[] questTarget;
        public Image[] questRewards;
        public Text questGoldCount;
        public Text questEXPCount;
        
        public void SetQuestInfo(Models.Quest quest)
        {
            title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.Name);
            questGoldCount.text = quest.Define.RewardGold.ToString();
            questEXPCount.text = quest.Define.RewardExp.ToString();

            if(quest.Info ==null)
            {
                questDiscribution.text = quest.Define.Dialog;
            }
            else
            {
                if(quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
                {
                    questDiscribution.text = quest.Define.DialogFinish;
                }
            }

            foreach (var fitter in GetComponentsInChildren<ContentSizeFitter>())
            {
                fitter.SetLayoutVertical();
            }
        }

        public void OnClickAbandon()
        {

        }
    }
}