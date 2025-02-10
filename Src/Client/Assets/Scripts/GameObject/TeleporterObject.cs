using Assets.Scripts.Services;
using Common.Data;
using GameServer.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour {

	public int ID;
	Mesh mesh;
	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().sharedMesh;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
		Debug.Log(other.gameObject.name);
		var pc = other.GetComponent<PlayerInputController>();
		if (pc != null && pc.isActiveAndEnabled)
		{
			TeleporterDefine teleporterDefine = DataManager.Instance.Teleporters[ID];
			if (teleporterDefine == null)
			{
				Debug.LogFormat("TeleporterObject: Character [{0}] Enter Teleporter [{1}], But TeleporterDefine not existed", pc.character.Info.Name, ID);
				return;
			}
			if(teleporterDefine.LinkTo > 0)
			{
				if(DataManager.Instance.Teleporters.ContainsKey(teleporterDefine.LinkTo))
				{
					MapService.Instance.SendMapTeleport(ID);
				}
				else
					Debug.LogFormat("Teleporter ID: {0} LinkID {1} error!", teleporterDefine.ID, teleporterDefine.LinkTo);
			}
		}
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
		Gizmos.color = Color.green;
		if (mesh != null)
		{
			Gizmos.DrawWireMesh(mesh, transform.position, transform.rotation, transform.localScale);
		}
		UnityEditor.Handles.color = Color.red;
		UnityEditor.Handles.ArrowHandleCap(0, transform.position, transform.rotation, 1f, EventType.Repaint);
    }
#endif
}
