using Common.Data;
using GameServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    public class Item
    {
        TCharacterItem tCharacterItem;
        public int itemID;
        public int itemCount;
        public int CharacterID;
        public Item(TCharacterItem item)
        {
            this.tCharacterItem = item;
            itemID = item.Id;
            itemCount = item.ItemCount;
        }

        public void Add(int Count = 1)
        {
            itemCount += Count;
            tCharacterItem.ItemCount = itemCount;
        }

        public void Remove(int Count = 1)
        {
            itemCount -= Count;
            tCharacterItem.ItemCount = itemCount;
        }

        public bool Use(int count = 1)
        {


            return false;
        }

        public override string ToString()
        {
            return string.Format("Item:: ID: {0} Count: {1}", itemID, itemCount);
        }
    }
}
