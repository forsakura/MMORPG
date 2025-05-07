using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Quest
{
    public class UIQuestDialogInfo : MonoBehaviour
    {
        public Text title;
        public Text questDiscribution;
        public Text[] questTarget;
        public Image[] questRewards;
        public Text questGoldCount;
        public Text questEXPCount;
        public UnityEngine.GameObject uiQuestRewardItem;

        public void SetQuestInfo(Models.Quest quest)
        {
            title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.Name);
            questGoldCount.text = quest.Define.RewardGold.ToString();
            questEXPCount.text = quest.Define.RewardExp.ToString();

            if (quest.Define.RewardItem1 != 0)
            {
                var go = Instantiate(uiQuestRewardItem, questRewards[0].transform);
                var ui = go.GetComponent<UIQuestRewardItem>();
                ui.SetInfo(quest.Define.RewardItem1, quest.Define.RewardItem1Count);
            }
            if (quest.Define.RewardItem2 != 0)
            {
                var go = Instantiate(uiQuestRewardItem, questRewards[1].transform);
                var ui = go.GetComponent<UIQuestRewardItem>();
                ui.SetInfo(quest.Define.RewardItem2, quest.Define.RewardItem2Count);
            }
            if (quest.Define.RewardItem3 != 0)
            {
                var go = Instantiate(uiQuestRewardItem, questRewards[2].transform);
                var ui = go.GetComponent<UIQuestRewardItem>();
                ui.SetInfo(quest.Define.RewardItem3, quest.Define.RewardItem3Count);
            }
            if (quest.Info ==null)
            {
                questDiscribution.text = quest.Define.Dialog;
            }
            else
            {
                if(quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
                {
                    questDiscribution.text = quest.Define.DialogFinish;
                }
                else if(quest.Info.Status == SkillBridge.Message.QuestStatus.InProgress)
                {
                    questDiscribution.text = quest.Define.DialogAccept;
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