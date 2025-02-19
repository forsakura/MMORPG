using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class StatusManager
    {
        Character owner;

        List<NStatus> Status {  get; set; }

        public bool HasStatus { get { return Status.Count > 0; } }

        public StatusManager(Character character)
        {
            owner = character;
            Status = new List<NStatus>();
        }

        public void AddStatus(StatusType type, int id, int value, StatusAction action)
        {
            Status.Add(new NStatus()
            {
                Id = id,
                Value = value,
                Action = action,
                Type = type
            });
        }

        public void AddGoldChange(int goldDelta)
        {
            if (goldDelta > 0)
            {
                Status.Add(new NStatus() { Type = StatusType.Money, Id = 0, Value = goldDelta, Action = StatusAction.Add });
            }
            else if (goldDelta < 0)
                Status.Add(new NStatus() { Type = StatusType.Money, Id = 0, Value = -goldDelta, Action = StatusAction.Delete });
        }

        public void AddItemChange(int ItemID, int count, StatusAction action)
        {
            Status.Add(new NStatus() { Type = StatusType.Item, Id = ItemID, Value = count, Action = action });
        }

        public void PostProcess(NetMessageResponse message)
        {
            if (message.statusNotify == null)
                message.statusNotify = new StatusNotify();
            foreach (var status in Status)
            {
                message.statusNotify.Status.Add(status);
            }
            Status.Clear();
        }
    }
}
