using Common;
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
        }

        public void Dispose()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Unsubscribe<ItemBuyRequest>(OnItemBuy);
        }

        private void OnItemBuy(NetConnection<NetSession> sender, ItemBuyRequest message)
        {
            Log.InfoFormat("ItemBuyRequest:: ShopID: {0} ShopItemID: {1}", message.shopId, message.shopItemId);
            var res = ShopManager.Instance.BuyItem(sender, message.shopId, message.shopItemId);
            sender.Session.Response.itemBuy = new ItemBuyResponse();
            sender.Session.Response.itemBuy.Result = res;
            sender.SendResponse();
        }
    }
}
