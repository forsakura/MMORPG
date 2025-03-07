using Assets.Scripts.Managers;
using Common.Data;
using GameServer.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : Singleton<NpcManager> {
	public NpcDefine GetNPCDefine(int npcId)
	{
		return DataManager.Instance.Npcs[npcId];
	}

	public delegate bool NPCActionHandler(NpcDefine npcDefine);
	public Dictionary<NpcFunction, NPCActionHandler> NpcMaps = new Dictionary<NpcFunction, NPCActionHandler>();	

	public void RegisterNPCActionHandler(NpcFunction npcFunction, NPCActionHandler handler)
	{
		if (NpcMaps.ContainsKey(npcFunction))
		{
			NpcMaps[npcFunction] += handler;
		}
		else NpcMaps[npcFunction] = handler;
	}

	public bool Interactive(int npcID)
	{
		if(DataManager.Instance.Npcs.ContainsKey(npcID))
		{
			return Interactive(DataManager.Instance.Npcs[npcID]);
		}
		return false;
	}

	private bool Interactive(NpcDefine npcDefine)
	{
		if(DoTaskInteractive(npcDefine))
		{
			return true;
		}
		else if(npcDefine.Type == NpcType.Functional)
		{
			return DoFunctionalactive(npcDefine);
		}
		return false;
	}

    private bool DoTaskInteractive(NpcDefine npcDefine)
    {
		var status = QuestManager.Instance.GetNpcQuestStatus(npcDefine.ID);
		if (status == NpcQuestStatus.None)
			return false;
        return QuestManager.Instance.OpenNpcQuest(npcDefine.ID);
    }

    private bool DoFunctionalactive(NpcDefine npcDefine)
    {
        Debug.LogFormat("NPCManager:DoFunctionalInteractive::NPC: [{0} : {1}] : Task: {2} : {3}", npcDefine.ID, npcDefine.Name, npcDefine.Type, npcDefine.Function);
		if (npcDefine.Type != NpcType.Functional) return false;
		if (!NpcManager.Instance.NpcMaps.ContainsKey(npcDefine.Function)) return false;
		return NpcManager.Instance.NpcMaps[npcDefine.Function](npcDefine);
    }
}
