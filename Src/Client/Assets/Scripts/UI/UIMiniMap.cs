using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : MonoSingleton<UIMiniMap> {

	public Image mapImage;
	public Text mapName;
	private Collider mapBorder;
	public GameObject arraw;
	public Transform characterTransform;
	// Use this for initialization
	protected override void OnStart () {
		MiniMapManager.Instance.minimap = this;
		UpdateMap();
	}
	
	public void UpdateMap()
	{
		mapName.text = User.Instance.curentMiniMap.Name;
		mapImage.overrideSprite = MiniMapManager.Instance.LoadCurrentMiniMap();
		mapImage.SetNativeSize();
		mapImage.transform.position = Vector3.zero;
		mapBorder = MiniMapManager.Instance.BoxCollider;
		characterTransform = null;
    }

	// Update is called once per frame
	void Update () {
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
