using Common;
using GameServer.Entities;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    internal class QuestService : Singleton<QuestService>, IDisposable
    {
        public QuestService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<QuestAcceptRequest>(OnQuestAccept);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<QuestSubmitRequest>(OnQuestSubmit);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<QuestAbandonRequest>(OnQuestAbandon);
        }

        public void Init()
        {

        }

        public void Dispose()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Unsubscribe<QuestAcceptRequest>(OnQuestAccept);
            MessageDistributer<NetConnection<NetSession>>.Instance.Unsubscribe<QuestSubmitRequest>(OnQuestSubmit);
            MessageDistributer<NetConnection<NetSession>>.Instance.Unsubscribe<QuestAbandonRequest>(OnQuestAbandon);
        }

        private void OnQuestAbandon(NetConnection<NetSession> sender, QuestAbandonRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("QuestAbandon: Character: {0} Quest: {1}", character.Id, message.QuestId);
            Result result =  character.QuestManager.AbandonQuest(sender, message.QuestId);
            sender.Session.Response.questAbandon = new QuestAbandonResponse();
            sender.Session.Response.questAbandon.Result = result;
            sender.SendResponse();
        }

        private void OnQuestSubmit(NetConnection<NetSession> sender, QuestSubmitRequest message)
        {
            Character character = sender.Session.Character;

            Log.InfoFormat("QuestSubmit: Character: {0} Quest: {1}", character.Id, message.QuestId);

            sender.Session.Response.questSubmit = new QuestSubmitResponse();
            Result result = character.QuestManager.SubmitQuest(sender, message.QuestId);
            sender.Session.Response.questSubmit.Result = result;
            sender.SendResponse();
        }

        private void OnQuestAccept(NetConnection<NetSession> sender, QuestAcceptRequest message)
        {
            Character character = sender.Session.Character;

            Log.InfoFormat("QuestAccept: CharacterID: {0} Quest: {1} ", character.Id, message.QuestId);

            sender.Session.Response.questAccept = new QuestAcceptResponse();

            Result result = character.QuestManager.AcceptQuest(sender, message.QuestId);
            sender.Session.Response.questAccept.Result = result;
            sender.SendResponse();
        }
    }
}
