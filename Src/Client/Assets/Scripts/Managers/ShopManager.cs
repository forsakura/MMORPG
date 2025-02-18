using Assets.Scripts.Services;
using Assets.Scripts.UI;
using Common.Data;
using GameServer.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    public void Init()
    {
        NpcManager.Instance.RegisterNPCActionHandler(Common.Data.NpcFunction.InvokeShop, OnOpenShop);
    }

    private bool OnOpenShop(NpcDefine npcDefine)
    {
        ShowShop(npcDefine.Param);
        return true;
    }

    private void ShowShop(int param)
    {
        ShopDefine shopDefine = null;
        if(DataManager.Instance.Shops.TryGetValue(param, out shopDefine))
        {
            var res = UIManager.Instance.Show<UIShop>();
            if (res != null)
            {
                res.SetShop(shopDefine);
            }
        }
    }

    internal bool BuyItem(int shopID, int shopItemID)
    {
        ItemService.Instance.SendBuyItem(shopID, shopItemID);
        return true;
    }
}
