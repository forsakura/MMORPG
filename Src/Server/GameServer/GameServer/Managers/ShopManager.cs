using Common;
using Common.Data;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    internal class ShopManager : Singleton<ShopManager>
    {
        internal Result BuyItem(NetConnection<NetSession> sender, int shopId, int shopItemId)
        {
            if(DataManager.Instance.Shops.ContainsKey(shopId))
            {
                ShopItemDefine shopItemDefine = null;
                if (DataManager.Instance.ShopItems[shopId].TryGetValue(shopItemId, out shopItemDefine))
                {
                    Log.InfoFormat("BuyItem:: character: {0} item : id: {1} count: {2} price: {3}", sender.Session.Character, shopItemDefine.itemID, shopItemDefine.Count, shopItemDefine.Price);
                    if(sender.Session.Character.Gold >= shopItemDefine.Price)
                    {
                        sender.Session.Character.ItemManager.Add(shopItemDefine.itemID, shopItemDefine.Count);
                        sender.Session.Character.Gold -= shopItemDefine.Price;
                        DBService.Instance.Save();
                        return Result.Success;
                    }
                }
            }
            return Result.Failed;
        }
    }
}
