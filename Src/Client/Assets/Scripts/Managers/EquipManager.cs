using Assets.Scripts.Models;
using Assets.Scripts.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Managers
{
    internal class EquipManager : Singleton<EquipManager>
    {
        public delegate void OnEquipChangeHandler();

        public event OnEquipChangeHandler OnEquipChange;

        public Item[] Equips = new Item[(int)EquipSlot.SlotMax];

        byte[] Data;

        unsafe public void Init(byte[] data)
        {
            Data = data;
            ParseEquipData(data);
        }

        public bool Contains(int equipId)
        {
            for(int i = 0;  i < Equips.Length; i++)
            {
                if (Equips[i] != null && Equips[i].equipDefine.ID == equipId)
                    return true;
            }
            return false;
        }

        public Item GetEquip(EquipSlot slot)
        {
            return Equips[(int)slot];
        }

        unsafe public void ParseEquipData(byte[] data)
        {
            fixed(byte* pt = Data)
            {
                for(int i = 0; i < Equips.Length; i++)
                {
                    int itemId = *(int*)(pt + i * sizeof(int));
                    if (itemId > 0)
                        Equips[i] = ItemManager.Instance.items[itemId];
                    else
                        Equips[i] = null;
                }
            }
        }

        unsafe public byte[] GetEquipDate()
        {
            fixed (byte* pt = Data)
            {
                for(int i = 0;i < Equips.Length;i++)
                {
                    int* itemId = (int*)(pt + i * sizeof(int));
                    if (Equips[i] != null)
                        *itemId = Equips[i].equipDefine.ID;
                    else
                        *itemId = 0;
                }
            }
            return Data;
        }

        public void EquipItem(Item equip)
        {
            ItemService.Instance.SendItemEquip(equip, true);
        }

        public void UnEquipItem(Item equip)
        {
            ItemService.Instance.SendItemEquip(equip, false);
        }

        internal void OnEquip(Item pendingEquip)
        {
            if (Equips[(int)pendingEquip.equipDefine.Slot] != null && Equips[(int)pendingEquip.equipDefine.Slot].itemID == pendingEquip.itemID)
                return;
            Equips[(int)pendingEquip.equipDefine.Slot] = ItemManager.Instance.items[pendingEquip.itemID];
            if(OnEquipChange != null)
                OnEquipChange();
        }

        internal void OnUnEquip(EquipSlot slot)
        {
            if (Equips[(int)slot] != null)
            {
                Equips[(int)slot] = null;
                if(OnEquipChange != null) 
                    OnEquipChange();
            }
        }
    }
}
