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
        public ItemDefine define;
        public Item(NItemInfo info)
        {
            itemID = info.Id;
            itemCount = info.Count;
            define = DataManager.Instance.Items[itemID];
        }

        public override string ToString()
        {
            return string.Format("Item:: ID: {0}, Count: {1}", itemID, itemCount);
        }
    }
}