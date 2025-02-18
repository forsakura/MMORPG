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

    private bool DoTaskInteractive(NpcDefine npcDefine)
    {
        Debug.LogFormat("NPCManager:DoTaskInteractive::NPC: [{0} : {1}] : Task: {2} : {3}", npcDefine.ID, npcDefine.Name, npcDefine.Type, npcDefine.Function);
        if (npcDefine.Type != NpcType.Task) return false;
        if(!NpcMaps.ContainsKey(npcDefine.Function)) return false;
        return NpcMaps[npcDefine.Function](npcDefine);
    }

    private bool DoFunctionalactive(NpcDefine npcDefine)
    {
        Debug.LogFormat("NPCManager:DoFunctionalInteractive::NPC: [{0} : {1}] : Task: {2} : {3}", npcDefine.ID, npcDefine.Name, npcDefine.Type, npcDefine.Function);
        MessageBox.Show("触发任务NPC:" + npcDefine.Name, "NPC对话");
        throw new NotImplementedException();
    }
}
