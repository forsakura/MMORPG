using Common.Data;
using GameServer.Managers;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Item
    {
        public int itemID;
        public int itemCount;
        public EquipDefine equipDefine;
        public ItemDefine itemDefine;
        public RideDefine rideDefine;
        public Item(NItemInfo info) : this(info.Id, info.Count)
        {
        }

        public Item(int itemID, int itemCount)
        {
            this.itemID = itemID;
            this.itemCount = itemCount;
            DataManager.Instance.Equips.TryGetValue(itemID, out equipDefine);
            DataManager.Instance.Items.TryGetValue(itemID, out itemDefine);
            DataManager.Instance.Rides.TryGetValue(itemID, out rideDefine);
        }

        public override string ToString()
        {
            return string.Format("Item:: ID: {0}, Count: {1}", itemID, itemCount);
        }
    }
}