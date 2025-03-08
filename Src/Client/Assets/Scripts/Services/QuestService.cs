using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class QuestService : Singleton<QuestService>, IDisposable
    {
        public QuestService()
        {
            MessageDistributer.Instance.Subscribe<QuestAcceptResponse>(OnQuestAccept);
            MessageDistributer.Instance.Subscribe<QuestSubmitResponse>(OnQuestSubmit);
            MessageDistributer.Instance.Subscribe<QuestAbandonResponse>(OnQuestAbandon);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<QuestAcceptResponse>(OnQuestAccept);
            MessageDistributer.Instance.Unsubscribe<QuestSubmitResponse>(OnQuestSubmit);
            MessageDistributer.Instance.Unsubscribe<QuestAbandonResponse>(OnQuestAbandon);
        }


        public bool SendQuestAbandon(Quest quest)
        {
            Debug.LogFormat("SendQuestAbandon: {0}", quest.Define.ID);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.questAbandon = new QuestAbandonRequest();
            message.Request.questAbandon.QuestId = quest.Define.ID;
            NetClient.Instance.SendMessage(message);
            return true;
        }
        private void OnQuestAbandon(object sender, QuestAbandonResponse message)
        {
            Debug.LogFormat("QuestAbandon: {0} Err: {1}", message.Result, message.Errormsg);
            if (message.Result == Result.Success)
            {
                QuestManager.Instance.OnQuestAbandoned(message.Quest);
            }
            else
            {
                MessageBox.Show("任务拒绝失败", "错误", MessageBoxType.Error);
            }
        }

        public bool SendQuestSubmit(Quest quest)
        {
            Debug.LogFormat("SendQuestSubmit: {0}", quest.Define.ID);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.questSubmit = new QuestSubmitRequest();
            message.Request.questSubmit.QuestId = quest.Define.ID;
            NetClient.Instance.SendMessage(message);
            return true;

        }
        private void OnQuestSubmit(object sender, QuestSubmitResponse message)
        {
            Debug.LogFormat("QuestSubmit: {0} Err: {1}", message.Result, message.Errormsg);
            if (message.Result == Result.Success)
            {
                QuestManager.Instance.OnQuestSubmited(message.Quest);
            }
            else
            {
                MessageBox.Show("任务提交失败", "错误", MessageBoxType.Error);
            }
        }

        public bool SendQuestAccept(Quest quest)
        {
            Debug.LogFormat("SendQuestAccept:{0}", quest.Define.ID);

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.questAccept = new QuestAcceptRequest();
            message.Request.questAccept.QuestId = quest.Define.ID;
            NetClient.Instance.SendMessage(message);
            return true;
        }
        private void OnQuestAccept(object sender, QuestAcceptResponse message)
        {
            Debug.LogFormat("QuestAccept: {0} , Err: {1}", message.Result, message.Errormsg);
            if(message.Result == Result.Success)
            {
                QuestManager.Instance.OnQuestAccepted(message.Quest);
            }
            else
            {
                MessageBox.Show("任务接受失败", "错误", MessageBoxType.Error);
            }
        }
    }
}