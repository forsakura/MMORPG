using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : Singleton<MiniMapManager> {

	public Sprite LoadCurrentMiniMap()
	{
		return Resloader.Load<Sprite>("UI/MiniMap/" + User.Instance.curentMiniMap.MiniMap);
	}
}
