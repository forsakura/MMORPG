using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Quest
{
    public class UIQuestSystemInfo : MonoBehaviour
    {
        public Text title;
        public Text questDiscribution;
        public Text overView;
        public Text[] questTarget;
        public Image[] questRewards;
        public Text questGoldCount;
        public Text questEXPCount;

        public void SetQuestInfo(Models.Quest quest)
        {
            title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.Name);
            questGoldCount.text = quest.Define.RewardGold.ToString();
            questEXPCount.text = quest.Define.RewardExp.ToString();
            if (overView != null) overView.text = quest.Define.Overview;
            if (this.questDiscribution != null)
            {
                if(quest.Info != null)
                {
                    this.questDiscribution.text = quest.Define.Dialog;
                }
                else
                {
                    if(quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
                    {
                        this.questDiscribution.text = quest.Define.DialogFinish;
                    }
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