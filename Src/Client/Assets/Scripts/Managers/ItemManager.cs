using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Services;
using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager> {

	public Dictionary<int, Item> items = new Dictionary<int, Item>();
	public void Init(List<NItemInfo> list)
	{
		items.Clear();
		foreach (NItemInfo info in list)
		{
			Item item = new Item(info);
			items.Add(item.itemID, item);
			Debug.LogFormat("ItemManager: Init{0}", item);
		}
		StatusService.Instance.RegisterStatusNotify(StatusType.Item, OnItemNotify);
	}

    private bool OnItemNotify(NStatus status)
    {
        if(status.Action == StatusAction.Add)
			AddItem(status.Id, status.Value);
		else if(status.Action == StatusAction.Delete)
			RemoveItem(status.Id, status.Value);
		return true;
    }

    private void AddItem(int id, int value)
    {
		Item item = null;
		if (items.TryGetValue(id, out item))
			item.itemCount += value;
		else
		{
			item = new Item(id, value);
			items.Add(id, item);
		}
		BagManager.Instance.AddItem(id, value);
    }

    private void RemoveItem(int id, int value)
    {
		if (!items.ContainsKey(id))
			return;
		Item item = items[id];
		if (item.itemCount < value) return;
		item.itemCount -= value;
		BagManager.Instance.RemoveItem(id, value);
    }

    public ItemDefine GetItem(int itemID)
	{

		return null;
	}

	public bool UseItem(int itemID)
	{

		return false;
	}

	public bool UseItem(ItemDefine item)
	{

		return false;
	}
}
