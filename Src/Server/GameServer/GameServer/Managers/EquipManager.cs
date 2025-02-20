using Common;
using GameServer.Entities;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    internal class EquipManager : Singleton<EquipManager>
    {
        internal Result EquipItem(NetConnection<NetSession> sender, int slot, int itemId, bool isEquip)
        {
            Character character = sender.Session.Character;
            if(character.ItemManager.items.ContainsKey(itemId))
            {
                UpdateEquip(character.Data.Equips, slot, itemId, isEquip);
                DBService.Instance.Save();
                return Result.Success;
            }
            return Result.Failed;
        }

        unsafe void UpdateEquip(byte[] equips, int slot, int itemId, bool isEquip)
        {
            fixed (byte* equip = equips)
            {
                int* slotId = (int*)(equip + slot * sizeof(int));
                if (isEquip)
                    *slotId = itemId;
                else
                    *slotId = 0;
            }
        }
    }
}
