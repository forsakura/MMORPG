using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    internal class ItemService : Singleton<ItemService>, IDisposable
    {
        public void Init()
        {

        }
        public ItemService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemBuyRequest>(OnItemBuy);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemEquipRequest>(OnItemEquip);
        }

        public void Dispose()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Unsubscribe<ItemBuyRequest>(OnItemBuy);
            MessageDistributer<NetConnection<NetSession>>.Instance.Unsubscribe<ItemEquipRequest>(OnItemEquip);
        }

        private void OnItemBuy(NetConnection<NetSession> sender, ItemBuyRequest message)
        {
            Log.InfoFormat("ItemBuyRequest:: ShopID: {0} ShopItemID: {1}", message.shopId, message.shopItemId);
            var res = ShopManager.Instance.BuyItem(sender, message.shopId, message.shopItemId);
            sender.Session.Response.itemBuy = new ItemBuyResponse();
            sender.Session.Response.itemBuy.Result = res;
            sender.SendResponse();
        }

        private void OnItemEquip(NetConnection<NetSession> sender, ItemEquipRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("ItemEquipRequest::Cahracter : {0} Slot : {1} Item : {2} Equip : {3}", character.Id, message.Slot, message.itemId, message.isEquip);
            var res = EquipManager.Instance.EquipItem(sender, message.Slot, message.itemId, message.isEquip);
            sender.Session.Response.itemEquip = new ItemEquipResponse();
            sender.Session.Response.itemEquip.Result = res;
            sender.SendResponse();
        }
    }
}
