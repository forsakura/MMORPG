using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Item
    {
        public int itemID;
        public int itemCount;
        public Item(NItemInfo info)
        {
            this.itemID = info.Id;
            this.itemCount = info.Count;
        }

        public override string ToString()
        {
            return string.Format("Item:: ID: {0}, Count: {1}", itemID, itemCount);
        }
    }
}