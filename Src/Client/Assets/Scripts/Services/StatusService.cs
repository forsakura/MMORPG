using Assets.Scripts.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class StatusService : Singleton<StatusService>, IDisposable
    {
        public delegate bool StatusNotifyHandler(NStatus status);
        Dictionary<StatusType, StatusNotifyHandler> statusMap = new Dictionary<StatusType, StatusNotifyHandler>();

        public void Init()
        {

        }

        public void RegisterStatusNotify(StatusType type, StatusNotifyHandler statusNotifyHandler)
        {
            if(statusMap.ContainsKey(type))
                statusMap[type] += statusNotifyHandler;
            else 
                statusMap[type] = statusNotifyHandler;
        }

        public StatusService()
        {
            MessageDistributer.Instance.Subscribe<StatusNotify>(OnStatusNotify);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<StatusNotify>(OnStatusNotify);
        }

        private void OnStatusNotify(object sender, StatusNotify message)
        {
            foreach (var status in message.Status)
            {
                Notify(status);
            }
        }

        private void Notify(NStatus status)
        {
            Debug.LogFormat("StatusNotify: [{0}] [{1}] {2} : {3}", status.Type, status.Action, status.Id, status.Value);
            if(status.Type == StatusType.Money)
            {
                if(status.Action == StatusAction.Add)
                {
                    User.Instance.AddMoney(status.Value);
                }
                else if(status.Action == StatusAction.Delete)
                    User.Instance.AddMoney(-status.Value);
            }
            StatusNotifyHandler handler;
            if(statusMap.TryGetValue(status.Type, out handler))
            {
                handler(status);
            }
        }
    }
}