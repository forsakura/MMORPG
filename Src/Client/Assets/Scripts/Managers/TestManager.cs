using Assets.Scripts.UI;
using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class TestManager : Singleton<TestManager>
    {
        public void Init()
        {
            NpcManager.Instance.RegisterNpcFunction(Common.Data.NpcFunction.InvokeShop, OnNPCInvokeShop);
            NpcManager.Instance.RegisterNpcFunction(Common.Data.NpcFunction.InvokeInsrance, OnNPCInvokeInsrance);
        }

        private bool OnNPCInvokeInsrance(NpcDefine npc)
        {
            Debug.LogFormat("TestManager.OnNPCInvokeInsrance:NPC:[{0} :{1}] Type: {2} Func: {3}", npc.ID, npc.Name, npc.Type, npc.Function);
            return true;
        }

        private bool OnNPCInvokeShop(NpcDefine npc)
        {
            Debug.LogFormat("TsetManager.OnNPCInvokeShop:NPC:[{0} :{1}] Type: {2} Func{3}", npc.ID, npc.Name, npc.Type, npc.Function);
            UITest test = UIManager.Instance.Show<UITest>();
            test.SetTitle(npc.Name);
            return true;
        }
    }
}