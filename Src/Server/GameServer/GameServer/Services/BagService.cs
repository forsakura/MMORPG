using Common;
using GameServer.Entities;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    internal class BagService : Singleton<BagService>
    {
        public BagService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<BagSaveRequest>(OnBagSave);
        }

        public void Init()
        {

        }

        private void OnBagSave(NetConnection<NetSession> sender, BagSaveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("BagSaveRequest:: characterID: {0}  items: {1}", character.Info.Id, message.BagInfo.Unlocked);
            if (message.BagInfo != null)
            {
                character.Info.Bag.Items = message.BagInfo.Items;
                DBService.Instance.Save();
            }
        }
    }
}
