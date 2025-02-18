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
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(OnItemBuy);
        }

        private void OnItemBuy(object sender, ItemBuyResponse message)
        {
            Debug.LogFormat("ItemBuyResponse:: Result: {0}  Message: {1}", message.Result, message.Errormsg);
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
    }
}