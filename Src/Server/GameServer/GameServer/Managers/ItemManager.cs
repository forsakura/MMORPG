using GameServer.Entities;
using GameServer.Services;
using SkillBridge.Message;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer.Managers
{
    public class ItemManager
    {
        public Character owner;
        Dictionary<int, Item> items = new Dictionary<int, Item>();
        public ItemManager(Character character)
        {
            owner = character;

            foreach (var item in owner.Data.Items)
            {
                items.Add(item.ItemID, new Item(item));
            }
        }

        public bool Add(int itemID, int count)
        {
            Item itemTemp = null;
            if (items.TryGetValue(itemID, out itemTemp))
            {
                itemTemp.Add(count);
            }
            else
            {
                TCharacterItem item = new TCharacterItem();
                item.Owner = owner.Data;
                item.CharacterID = owner.Data.ID;
                item.ItemID = itemID;
                item.ItemCount = count;
                owner.Data.Items.Add(item);
                itemTemp = new Item(item);
                items[itemID] = itemTemp;
            }
            Debug.LogFormat("ItemManager::Add: ItemID: {0} ItemCount: {1}", itemID, count);
            DBService.Instance.Save(true);
            return true;
        }

        public bool Use(int itemID, int count)
        {
            Item itemTemp = null;
            if (items.TryGetValue(itemID, out itemTemp))
            {
                if (itemTemp.itemCount > count)
                {
                    //ToDo: 添加使用逻辑

                    itemTemp.Remove(count);
                    DBService.Instance.Save(true);
                    return true;
                }
            }
            return false;
        }

        public bool Remove(int itemID, int count)
        {
            Item itemTemp = null;
            if (items.TryGetValue(itemID, out itemTemp))
            {
                if (itemTemp.itemCount > count)
                {
                    itemTemp.Remove(count);
                    DBService.Instance.Save(true);
                    return true;
                }
            }
            return false;
        }

        public bool HasItem(int itemID)
        {
            Item itemTemp = null;
            if(items.TryGetValue(itemID,out itemTemp))
            {
                return itemTemp.itemCount > 0;
            }
            return false;
        }

        public Item GetItem(int itemID)
        {
            Item itemTemp = null;
            items.TryGetValue(itemID, out itemTemp);
            return itemTemp;
        }

        public void GetItemsInfo(List<NItemInfo> list)
        {
            foreach (var item in items)
            {
                list.Add(new NItemInfo() { Id = item.Value.itemID, Count = item.Value.itemCount });
            }
        }
    }
}
