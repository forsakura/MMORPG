﻿using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using SkillBridge.Message;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.CharEquip
{
    public class UICharEquip : UIWindow
    {
        public Text title;
        public Text money;
        public UnityEngine.GameObject ItemPrefab;
        public UnityEngine.GameObject ItemEquipedPrefab;
        public Transform EquipListRoot;
        public Transform[] Slots;
        public ListView.ListView mainView;
        public ListView.ListView equipedView;
        public UIEquipItem selectedEquip;
        public UIEquipItem selectedEquiped;

        public void Start()
        {
            mainView.onItemSelected += OnEquipSelected;
            equipedView.onItemSelected += OnEquipedSelected;
            RefreshUI();
            EquipManager.Instance.OnEquipChange += RefreshUI;
        }

        private void OnDestroy()
        {
            EquipManager.Instance.OnEquipChange -= RefreshUI;
        }

        private void RefreshUI()
        {
            ClearAllEquipList();
            InitAllEquipList();
            ClearEquipedList();
            InitEquipedItems();
            money.text = User.Instance.currentCharacter.Gold.ToString();
        }

        /// <summary>
        /// 初始化装备中的装备
        /// </summary>
        /// <exception cref=""></exception>
        private void InitEquipedItems()
        {
            for(int i = 0; i < (int)EquipSlot.SlotMax; i++)
            {
                var equip = EquipManager.Instance.Equips[i];
                if (equip != null)
                {
                    var go = Instantiate(ItemEquipedPrefab, Slots[i]);
                    var ui = go.GetComponent<UIEquipItem>();
                    ui.SetEquip(equip.equipDefine, this, equip, true);
                    equipedView.AddItem(ui);
                }
            }
        }

        /// <summary>
        /// 清空装备中的装备
        /// </summary>
        /// <exception cref=""></exception>
        private void ClearEquipedList()
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                var uiEquipItem = Slots[i].GetComponentInChildren<UIEquipItem>();
                if (uiEquipItem != null)
                {
                    Destroy(uiEquipItem.gameObject);
                }
            }
            equipedView.RemoveAll();
        }

        /// <summary>
        /// 初始化装备列表
        /// </summary>
        /// <exception cref=""></exception>
        private void InitAllEquipList()
        {
            foreach (var kv in ItemManager.Instance.items)
            {
                if (kv.Value.itemDefine.Type == SkillBridge.Message.ItemType.Equip && kv.Value.itemDefine.LimitClass == User.Instance.currentCharacter.Class)
                {
                    if (EquipManager.Instance.Contains(kv.Value.equipDefine.ID)) continue;
                    var go = Instantiate(ItemPrefab, EquipListRoot);
                    var uiEquipItem =  go.GetComponent<UIEquipItem>();
                    uiEquipItem.SetEquip(kv.Value.equipDefine, this, kv.Value, false);
                    mainView.AddItem(uiEquipItem);
                }
            }
        }

        /// <summary>
        /// 清空装备列表
        /// </summary>
        private void ClearAllEquipList()
        {
            var res = EquipListRoot.GetComponentsInChildren<UIEquipItem>();
            foreach (var item in res)
            {
                Destroy(item.gameObject);
            }
            mainView.RemoveAll();
        }

        /// <summary>
        /// 装备
        /// </summary>
        /// <param name="item"></param>
        internal void DoEquip(Item item)
        {
            EquipManager.Instance.EquipItem(item);
        }

        /// <summary>
        /// 卸下
        /// </summary>
        /// <param name="item"></param>
        internal void UnEquip(Item item)
        {
            EquipManager.Instance.UnEquipItem(item);
        }
        void OnEquipSelected(ListView.ListView.ListViewItem item)
        {
            if(this.selectedEquip != null&& item == null)
                this.selectedEquip = null;
            else this.selectedEquip = item as UIEquipItem;
            foreach (var equip in mainView.items)
            {
                equip.Selected = equip == selectedEquip;
            }
        }

        void OnEquipedSelected(ListView.ListView.ListViewItem item)
        {
            if (this.selectedEquiped != null && item == null)
                this.selectedEquiped = null;
            else this.selectedEquiped = item as UIEquipItem;
            foreach (var item1 in equipedView.items)
            {
                item1.Selected = item1 == selectedEquiped;
            }
        }
    }
}