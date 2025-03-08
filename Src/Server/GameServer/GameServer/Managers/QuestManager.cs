using Common.Data;
using GameServer.Entities;
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
            throw new NotImplementedException();
        }

        internal Result SubmitQuest(NetConnection<NetSession> sender, int questId)
        {
            throw new NotImplementedException();
        }

        internal Result AbandonQuest(NetConnection<NetSession> sender, int questId)
        {
            throw new NotImplementedException();
        }
    }
}
