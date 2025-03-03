﻿using Assets.Scripts.Models;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Shop;
using Assets.Scripts.UI.TabView;
using Common.Data;
using GameServer.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow {
	public Text coinText;
	public GameObject uiShopItem;
	ShopDefine shopDefine;
	public Transform[] itemRoots;
	public Text title;

	UIShopItem currentShopItem;
	// Use this for initialization
	void Start () {
		StartCoroutine(InitItems());
	}
	

	IEnumerator InitItems()
	{
        foreach (var item in DataManager.Instance.ShopItems[shopDefine.ID])
        {
            if(item.Value.Status > 0)
			{
				var go = Instantiate(uiShopItem, itemRoots[0]);
				UIShopItem uIShopItem = go.GetComponent<UIShopItem>();
				uIShopItem.SetItem(item.Key, item.Value, this);
			}
        }
		yield return null;
    }

    private void Update()
    {
        if(coinText.text != User.Instance.currentCharacter.Gold.ToString()) coinText.text = User.Instance.currentCharacter.Gold.ToString();
    }

    public void SetShop(ShopDefine shop)
	{
		this.shopDefine = shop;
		title.text = shop.Name;
		coinText.text = User.Instance.currentCharacter.Gold.ToString();
	}

	public void OnClickBuy()
	{
		if (currentShopItem == null)
		{
			MessageBox.Show("请选择要购买的道具", "购买提示");
			return;
		}
		ShopManager.Instance.BuyItem(this.shopDefine.ID, currentShopItem.shopItemID);
	}

    internal void SelectItem(UIShopItem uIShopItem)
    {
		if (currentShopItem != null)
			currentShopItem.Selected = false;
		currentShopItem = uIShopItem;
    }
}
