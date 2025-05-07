using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Common.Data;
using GameServer.Managers;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.CharEquip
{
    public class UIEquipItem : ListView.ListView.ListViewItem
    {
        UICharEquip charEquip;
        public Image backgroud;
        public Text title;
        public Text level;
        public Text LimitClass;
        public Text category;
        public Image equipIcon;
        public Sprite normalBg;
        public Sprite selectedBg;
        public int index { get; set; }
        Item item;
        bool isEquiped = false;
        public override void OnSelected(bool selected)
        {
            base.OnSelected(selected);
            backgroud.overrideSprite = selected ? selectedBg : normalBg;
            if (selected)
            {
                if (isEquiped)
                    UnEquip();
                else
                    DoEquip();
            }
        }

        public void SetEquip(EquipDefine define, UICharEquip owner, Item item, bool isEquiped)
        {
            this.item = item;
            this.isEquiped = isEquiped;
            this.charEquip = owner;
            if (title  != null) title.text = define.Name;
            if(category != null) category.text = define.Category.ToString();

            ItemDefine itemDefine = DataManager.Instance.Items[define.ID];
            if(level != null) level.text = itemDefine.Level.ToString();
            if(LimitClass != null) LimitClass.text = itemDefine.LimitClass.ToString();
            if(equipIcon != null)equipIcon.overrideSprite = Resloader.Load<Sprite>(itemDefine.Icon);
        }

        private void DoEquip()
        {
            var msg = MessageBox.Show(string.Format("要装备【{0}】吗？", this.item.itemDefine.Name), "确认", MessageBoxType.Confirm);
            msg.OnYes = () =>
            {
                var oldEquip = EquipManager.Instance.GetEquip(item.equipDefine.Slot);
                if (oldEquip != null)
                {
                    var newMsg = MessageBox.Show(string.Format("要替换掉{[0]}吗？", oldEquip.equipDefine.Name), "确认", MessageBoxType.Confirm);
                    newMsg.OnYes = () =>
                    {
                        this.charEquip.DoEquip(item);
                    };
                }
                else
                    charEquip.DoEquip(item);
            };
            msg.OnNo = () =>
            {
                owner.SelectedItem = null;
            };
        }
        private void UnEquip()
        {
            var msg = MessageBox.Show(string.Format("要取下装备[{0}]吗？", this.item.equipDefine.Name), "确认", MessageBoxType.Confirm);
            msg.OnYes = () =>
            {
                charEquip.UnEquip(item);
            };
            msg.OnNo = () =>
            {
                owner.SelectedItem = null;
            };
        }
    }
}