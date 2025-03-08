using Assets.Scripts.Models;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Quest;
using Common.Data;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public enum NpcQuestStatus
    {
        None = 0,
        complete,
        Available,
        Incomplete
    }
    public class QuestManager : Singleton<QuestManager>
    {

        public List<NQuestInfo> questInfos;
        public Dictionary<int, Quest> allQuests = new Dictionary<int, Quest>();
        public Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>> npcQuests = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();

        public void Init(List<NQuestInfo> quests)
        {
            this.questInfos = quests;
            allQuests.Clear();
            this.npcQuests.Clear();
            InitQuests();
        }

        private void InitQuests()
        {
            foreach (var item in questInfos)
            {
                Quest quest = new Quest(item);
                AddNpcQuest(quest.Define.AcceptNPC, quest);
                AddNpcQuest(quest.Define.SubmitNPC, quest);
                allQuests[quest.Info.QuestId] = quest;
            }
            foreach (var kv in DataManager.Instance.Quests)
            {
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.currentCharacter.Class)
                    continue;
                if (kv.Value.LimitLevel > User.Instance.currentCharacter.Level)
                    continue;
                if (allQuests.ContainsKey(kv.Key))
                    continue;

                if(kv.Value.PreQuset > 0)
                {
                    Quest preQuest;
                    if (allQuests.TryGetValue(kv.Value.PreQuset, out preQuest))
                    {
                        if (preQuest.Info == null)
                            continue;
                        if (preQuest.Info.Status != QuestStatus.Finished)
                            continue;
                    }
                    else
                        continue;
                }
                Quest quest = new Quest(kv.Value);
                AddNpcQuest(quest.Define.AcceptNPC, quest);
                allQuests[quest.Define.ID] = quest;
            }
        }

        private void AddNpcQuest(int npcId, Quest quest)
        {
            if(npcQuests.ContainsKey(npcId))
                npcQuests[npcId] = new Dictionary<NpcQuestStatus, List<Quest>>();
            List<Quest> availables;
            List<Quest> completes;
            List<Quest> incompletes;

            if (!npcQuests[npcId].TryGetValue(NpcQuestStatus.Available, out availables))
            {
                availables = new List<Quest>();
                npcQuests[npcId][NpcQuestStatus.Available] = availables;
            }
            if (!npcQuests[npcId].TryGetValue(NpcQuestStatus.complete, out completes))
            {
                completes = new List<Quest>();
                npcQuests[npcId][NpcQuestStatus.complete] = completes;
            }
            if (!npcQuests[npcId].TryGetValue(NpcQuestStatus.Incomplete, out incompletes))
            {
                incompletes = new List<Quest>();
                npcQuests[npcId][NpcQuestStatus.Incomplete] = incompletes;
            }

            if(quest.Info == null)
            {
                if(npcId == quest.Define.AcceptNPC && !npcQuests[npcId][NpcQuestStatus.Available].Contains(quest))
                {
                    npcQuests[npcId][NpcQuestStatus.Available].Add(quest);
                }
                else
                {
                    if(quest.Define.AcceptNPC == npcId && quest.Info.Status == QuestStatus.Complated)
                    {
                        if (!npcQuests[npcId][NpcQuestStatus.complete].Contains(quest))
                            npcQuests[npcId][NpcQuestStatus.complete].Add(quest);
                    }
                    if(quest.Define.AcceptNPC == npcId && quest.Info.Status == QuestStatus.InProgress)
                    {
                        if (!npcQuests[npcId][NpcQuestStatus.Incomplete].Contains(quest))
                            npcQuests[npcId][NpcQuestStatus.Incomplete].Add(quest);
                    }
                }
            }
        }

        public NpcQuestStatus GetNpcQuestStatus(int npcID)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (npcQuests.TryGetValue(npcID, out status))
            {
                if (status[NpcQuestStatus.complete].Count > 0)
                    return NpcQuestStatus.complete;
                if (status[NpcQuestStatus.Available].Count > 0)
                    return NpcQuestStatus.Available;
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                    return NpcQuestStatus.Incomplete;
            }
            return NpcQuestStatus.None;
        }

        public bool OpenNpcQuest(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if(npcQuests.TryGetValue(npcId, out status))
            {
                if (npcQuests[npcId][NpcQuestStatus.complete].Count > 0)
                    return ShowQuestDialog(npcQuests[npcId][NpcQuestStatus.complete].First());
                if (npcQuests[npcId][NpcQuestStatus.Available].Count > 0)
                    return ShowQuestDialog(npcQuests[npcId][NpcQuestStatus.Available].First());
                if (npcQuests[npcId][NpcQuestStatus.Incomplete].Count > 0)
                    return ShowQuestDialog(npcQuests[npcId][NpcQuestStatus.Incomplete].First());
            }
            return false;
        }
        bool ShowQuestDialog(Quest quest)
        {
            if(quest.Info == null || quest.Info.Status == QuestStatus.Complated)
            {
                UIQuestDialog dialog = UIManager.Instance.Show<UIQuestDialog>();
                dialog.SetQuest(quest);
                dialog.onClose += OnQuestDialogClose;
                return true;
            }
            if(quest.Info != null || quest.Info.Status == QuestStatus.Complated)
            {
                if (!string.IsNullOrEmpty(quest.Define.DialogIncomplete))
                    MessageBox.Show(quest.Define.DialogIncomplete);
            }
            return true;
        }

        void OnQuestDialogClose(UIWindow sender, UIWindow.WindowResult result)
        {
            UIQuestDialog uIQuestDialog = sender as UIQuestDialog;
            if(result == UIWindow.WindowResult.Yes)
            {

                MessageBox.Show(uIQuestDialog.quest.Define.DialogAccept);
            }
            else if(result == UIWindow.WindowResult.No)
            {
                MessageBox.Show(uIQuestDialog.quest.Define.DialogDeny);
            }
        }

        public void OnQuestAccepted(Quest quest)
        {

        }
    }
}