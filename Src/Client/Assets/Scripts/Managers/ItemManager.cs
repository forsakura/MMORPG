using Assets.Scripts.Models;
using Common.Data;
using SkillBridge.Message;
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
