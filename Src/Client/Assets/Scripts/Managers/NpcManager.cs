using Common.Data;
using GameServer.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : Singleton<NpcManager> {

	public delegate bool NPCActionHandler(NpcDefine npc);
	public Dictionary<NpcFunction, NPCActionHandler> NpcActions = new Dictionary<NpcFunction, NPCActionHandler>();
    internal NpcDefine GetDefine(int iD)
    {
		NpcDefine npcDefine = null;
		DataManager.Instance.Npcs.TryGetValue(iD, out npcDefine);
		return npcDefine;
    }

	public void RegisterNpcFunction(NpcFunction npcFunction, NPCActionHandler actionHandler)
	{
		if (NpcActions.ContainsKey(npcFunction))
		{
			NpcActions[npcFunction] += actionHandler;
		}
		else NpcActions[npcFunction] = actionHandler;
	}

	public bool Interactive(int npcID)
	{
		if (DataManager.Instance.Npcs.ContainsKey(npcID))
		{
			return Interactive(DataManager.Instance.Npcs[npcID]);
		}
		return false;
	}

	public bool Interactive(NpcDefine npcDefine)
	{
		if(npcDefine.Type == NpcType.Task)
		{
			return DoTaskInteractive(npcDefine);
		}
		else if(npcDefine.Type == NpcType.Functional)
		{
			return DoFunctionalactive(npcDefine);
		}
		return false;
	}

    private bool DoFunctionalactive(NpcDefine npcDefine)
    {
		MessageBox.Show("点击了NPC：" + npcDefine.Name, "NPC对话");
		return true;
    }

    private bool DoTaskInteractive(NpcDefine npcDefine)
    {
        if(npcDefine.Type != NpcType.Task) return false;
		if(!NpcActions.ContainsKey(npcDefine.Function)) return false;
		return NpcActions[npcDefine.Function](npcDefine);
    }
}
