using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : MonoSingleton<UIMiniMap> {

	public Image mapImage;
	public Text mapName;
	public Collider mapBorder;
	public GameObject arraw;
	public Transform characterTransform;
	// Use this for initialization
	protected override void OnStart () {
		mapName.text = User.Instance.curentMiniMap.Name;
		mapImage.overrideSprite = MiniMapManager.Instance.LoadCurrentMiniMap();
	}
	
	// Update is called once per frame
	void Update () {
		if (mapImage.overrideSprite != MiniMapManager.Instance.LoadCurrentMiniMap()) 
			mapImage.overrideSprite = MiniMapManager.Instance.LoadCurrentMiniMap();
		if (characterTransform == null)
        {
            characterTransform = MiniMapManager.Instance.currentCharacterTransform;
        }
		if (characterTransform == null || mapBorder == null) return;
		float realWeight = mapBorder.bounds.size.x;
		float realHeight = mapBorder.bounds.size.z;

		float realX = characterTransform.position.x - mapBorder.bounds.min.x;
		float realZ = characterTransform.position.z - mapBorder.bounds.min.z;

		float pivotX = realX / realWeight;
		float pivotZ = realZ / realHeight;

		mapImage.rectTransform.pivot = new Vector2 (pivotX, pivotZ);
		mapImage.rectTransform.localPosition = Vector3.zero;
		arraw.transform.eulerAngles = new Vector3(0, 0, -characterTransform.eulerAngles.y);
	}
}
