using Common.Data;
using GameServer.Entities;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class QuestManager
    {
        Character owner;
        public Dictionary<int, QuestDefine> allQuests = new Dictionary<int, QuestDefine>();
        public QuestManager(Character owner)
        {
            this.owner = owner;
        }

        internal Result AcceptQuest(NetConnection<NetSession> sender, int questId)
        {
            Character character = sender.Session.Character;
            QuestDefine quest;
            if(DataManager.Instance.Quests.TryGetValue(questId, out quest))
            {
                var dbquest = DBService.Instance.Entities.CharacterQuests.Create();
                dbquest.QuestID = quest.ID;
                if(quest.Target1 == TargetType.None)
                {
                    dbquest.Status = (int)QuestStatus.Complated;
                }
                else
                {
                    dbquest.Status = (int)QuestStatus.InProgress;
                }
                sender.Session.Response.questAccept.Quest = GetQuestInfo(dbquest);
                character.Data.Quests.Add(dbquest);
                DBService.Instance.Save();
                return Result.Success;
            }
            else
            {
                sender.Session.Response.questAccept.Errormsg = "任务不存在";
                return Result.Failed;
            }
        }

        internal Result SubmitQuest(NetConnection<NetSession> sender, int questId)
        {
            Character character = sender.Session.Character;
            QuestDefine quest;
            if(DataManager.Instance.Quests.TryGetValue(questId, out quest))
            {
                var dbquest = character.Data.Quests.Where(q=>q.QuestID == questId).FirstOrDefault();
                if (dbquest != null)
                {
                    if (dbquest.Status != (int)QuestStatus.Complated)
                    {
                        sender.Session.Response.questSubmit.Errormsg = "任务未完成";
                        return Result.Failed;
                    }
                    dbquest.Status = (int)QuestStatus.Finished;
                    sender.Session.Response.questSubmit.Quest = GetQuestInfo(dbquest);
                    DBService.Instance.Save();

                    if (quest.RewardGold > 0)
                    {
                        character.Gold += quest.RewardGold;
                    }
                    if (quest.RewardExp > 0)
                    {
                        //character.EXP += quest.RewardExp;
                    }
                    if(quest.RewardItem1 > 0)
                    {
                        character.ItemManager.Add(quest.RewardItem1, quest.RewardItem1Count);
                    }
                    if(quest.RewardItem2 > 0)
                    {
                        character.ItemManager.Add(quest.RewardItem2, quest.RewardItem2Count);
                    }
                    if (quest.RewardItem3 > 0)
                    {
                        character.ItemManager.Add(quest.RewardItem3, quest.RewardItem3Count);
                    }
                    DBService.Instance.Save();
                    return Result.Success;
                }
                sender.Session.Response.questSubmit.Errormsg = "任务不存在[2]";
                return Result.Failed;
            }
            else
            {
                sender.Session.Response.questSubmit.Errormsg = "任务不存在[1]";
                return Result.Failed;
            }
            
        }

        internal Result AbandonQuest(NetConnection<NetSession> sender, int questId)
        {
            return Result.Failed;
        }

        internal void GetQuestInfos(List<NQuestInfo> quests)
        {
            foreach (var item in owner.Data.Quests)
            {
                quests.Add(GetQuestInfo(item));
            }
        }

        NQuestInfo GetQuestInfo(TCharacterQuest quest)
        {
            NQuestInfo info = new NQuestInfo() { QuestId = quest.QuestID, Status = (QuestStatus)quest.Status };
            info.Targets = new int[3];
            info.Targets[0] = quest.Target1;
            info.Targets[1] = quest.Target2;
            info.Targets[2] = quest.Target3;
            return info;
        }
    }
}
