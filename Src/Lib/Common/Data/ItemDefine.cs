using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public enum ItemFunction
    {
        None,
        AddBuff,
        AddExp,
        AddMoney,
        AddItem,
        AddSkillPoint,
        RecoverHP,
        RecoverMP,
    }
    public class ItemDefine
    {
        public int itemID {  get; set; }
        public string itemName { get; set; }
        public  string itemDescription { get; set; }
        public ItemType itemType { get; set; }
        public string itemCategory { get; set; }
        public int itemLevel { get; set; }
        public string limitClass { get; set; }
        public bool canUse { get; set; }
        public float useCD { get; set; }
        public int price { get; set; }
        public int sellPrice { get; set; }
        public int stackLimit { get; set; }
        public string icon { get; set; }
        public ItemFunction function { get; set; }
        public int itemParam { get; set; }
        public List<int> itemParams { get; set; }
    }
}
