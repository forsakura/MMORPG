using Assets.Scripts.Models;
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
        public Button navButton;
        int npc;

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

            if(quest.Info ==null)
            {
                this.npc = quest.Define.AcceptNPC;
            }
            else if(quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                this.npc = quest.Define.SubmitNPC;
            }
            this.navButton.gameObject.SetActive(this.npc > 0);
        }

        public void OnClickAbandon()
        {

        }

        public void OnClickNav()
        {
            Vector3 pos = NpcManager.Instance.GetNpcPosition(this.npc);
            User.Instance.currentCharacterObject.StartNav(pos);
            UIManager.Instance.Close(typeof(UIQuestStatus));
        }
    }
}