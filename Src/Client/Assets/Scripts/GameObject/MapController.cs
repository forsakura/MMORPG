using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {
	public Collider BoxCollider;
	// Use this for initialization
	void Start () {
		MiniMapManager.Instance.UpdateMiniMap(BoxCollider);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
