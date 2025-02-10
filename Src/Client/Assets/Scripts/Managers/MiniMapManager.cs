using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : Singleton<MiniMapManager> {
	
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
}
