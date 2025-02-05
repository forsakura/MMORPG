using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : MonoBehaviour {

	public Image mapImage;
	public Text mapName;
	public Collider mapBorder;
	public GameObject arraw;
	public Transform characterTransform;
	// Use this for initialization
	void Start () {
		characterTransform = User.Instance.currentCharacterObject.transform;
		mapName.text = User.Instance.curentMiniMap.Name;
		mapImage.overrideSprite = MiniMapManager.Instance.LoadCurrentMiniMap();
	}
	
	// Update is called once per frame
	void Update () {
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
