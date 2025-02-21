using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Entities;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Services
{
    public class ItemService : Singleton<ItemService>, IDisposable
    {

        public ItemService()
        {
            MessageDistributer.Instance.Subscribe<ItemBuyResponse>(OnItemBuy);
            MessageDistributer.Instance.Subscribe<ItemEquipResponse>(OnItemEquip);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(OnItemBuy);
            MessageDistributer.Instance.Unsubscribe<ItemEquipResponse>(OnItemEquip);
        }

        private void OnItemBuy(object sender, ItemBuyResponse message)
        {
            MessageBox.Show("购买结果：" + message.Result + "\n" + message.Errormsg, "购买完成");
        }

        internal void SendBuyItem(int shopID, int shopItemID)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemBuy = new ItemBuyRequest();
            message.Request.itemBuy.shopId = shopID;
            message.Request.itemBuy.shopItemId = shopItemID;
            NetClient.Instance.SendMessage(message);
        }

        Item pendingEquip = null;
        bool isEquip;
        public bool SendItemEquip(Item item, bool isEquip)
        {
            if (pendingEquip != null)
                return false;
            Debug.Log("SendItemEquip");
            pendingEquip = item;
            this.isEquip = isEquip;
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemEquip = new ItemEquipRequest();
            message.Request.itemEquip.itemId = item.itemDefine.ID;
            message.Request.itemEquip.Slot = (int)item.equipDefine.Slot;
            message.Request.itemEquip.isEquip = isEquip;
            NetClient.Instance.SendMessage(message);
            return true;
        }

        private void OnItemEquip(object sender, ItemEquipResponse message)
        {
            if (message.Result == Result.Success)
            {
                if(pendingEquip != null)
                {
                    if(isEquip)
                        EquipManager.Instance.OnEquip(pendingEquip);
                    else
                        EquipManager.Instance.OnUnEquip(pendingEquip.equipDefine.Slot);
                    pendingEquip = null;
                }
            }
        }
    }
}