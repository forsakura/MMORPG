using Assets.Scripts.Models;
using GameServer.Managers;
using SkillBridge.Message;
using System;

namespace Assets.Scripts.Managers
{
    public class BagManager : Singleton<BagManager>
    {
        int unLocked;
        public BagItem[] items;
        NBagInfo info;
        unsafe public void Init(NBagInfo info)
        {
            this.info = info;
            unLocked = info.Unlocked;
            items = new BagItem[info.Unlocked];
            if (items == null && info.Items.Length >= unLocked)
            {
                Analyze(info.Items);
            }
            else
            {
                info.Items = new byte[sizeof(BagItem) * unLocked];
                Reset();
            }
        }

        /// <summary>
        /// 整理背包
        /// </summary>
        public void Reset()
        {
            int i = 0;
            foreach (var item in ItemManager.Instance.items)
            {
                if (item.Value.itemCount <= item.Value.define.stackLimit)
                {
                    items[i].itemID = (ushort)item.Key;
                    items[i].count = (ushort)item.Value.itemCount;
                }
                else
                {
                    int count = item.Value.itemCount;
                    while (count > item.Value.define.stackLimit)
                    {
                        items[i].itemID = (ushort)item.Key;
                        items[i].count = (ushort)item.Value.define.stackLimit;
                        i++;
                        count -= (ushort)item.Value.define.stackLimit;
                    }
                    items[i].itemID = (ushort)item.Key;
                    items[i].count = (ushort)count;
                }
                i++;
            }
        }

        /// <summary>
        /// 将字节数据转换为数组数据
        /// </summary>
        /// <param name="items"></param>
        unsafe private void Analyze(byte[] items)
        {
            fixed (byte* pt = items)
            {
                for (int i = 0; i < unLocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    this.items[i] = *item;
                }
            }
        }

        /// <summary>
        /// 将数组数据转换为字节数据
        /// </summary>
        /// <returns></returns>
        unsafe public NBagInfo GetBagInfo()
        {
            fixed (byte* pt = info.Items)
            {
                for (int i = 0; i < unLocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    *item = items[i];
                }
            }
            return info;
        }

        internal void AddItem(int id, int value)
        {
            ushort addCount = (ushort)value;
            for (int i = 0; i < items.Length; i++)
            {
                if(items[i].itemID == id)
                {
                    ushort canAdd = (ushort)(DataManager.Instance.Items[id].stackLimit - items[i].count);
                    if(canAdd >= addCount)
                    {
                        this.items[i].count += addCount;
                        addCount = 0;
                        return;
                    }
                    else
                    {
                        items[i].count += canAdd;
                        addCount -= canAdd;
                    }
                }
            }
            int j = 0;
            int countLimit = DataManager.Instance.Items[id].stackLimit;
            for (; j < items.Length && addCount > 0; j++)
            {
                if (items[j].itemID == 0)
                {
                    items[j].itemID = (ushort)id;
                    if (countLimit < addCount)
                    {
                        items[j].count = (ushort)countLimit;
                        addCount -= (ushort)countLimit;
                    }
                    else
                    {
                        items[j].count = addCount;
                        addCount = 0;
                    }    
                }
            }
        }

        internal void RemoveItem(int id, int value)
        {
            
        }
    }
}