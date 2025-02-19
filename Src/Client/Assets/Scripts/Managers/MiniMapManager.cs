using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : Singleton<MiniMapManager> {
	public UIMiniMap minimap;
	Collider boxCollider;
	public Collider BoxCollider
	{
		get { return boxCollider; }
	}
	public Transform currentCharacterTransform
	{
		get
		{
			if (User.Instance.currentCharacterObject == null)
			{
				return null;				
			}
			return User.Instance.currentCharacterObject.transform;
		}
	}
	public Sprite LoadCurrentMiniMap()
	{
		return Resloader.Load<Sprite>("UI/MiniMap/" + User.Instance.curentMiniMap.MiniMap);
	}

	public void UpdateMiniMap(Collider collider)
	{
		boxCollider = collider;
		if (minimap != null)
		{
			minimap.UpdateMap();
		}
	}
}
