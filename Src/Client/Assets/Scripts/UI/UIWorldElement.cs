using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElement : MonoBehaviour {
	public Transform owner;
	public float height;
	// Use this for initialization
	void Start () {
		UpdateView();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateView();
	}

	void UpdateView()
	{
		if (owner != null)
		{
			transform.position = this.owner.position + Vector3.up * height;
		}
		if(Camera.main != null)
		{
			transform.forward = Camera.main.transform.forward;
		}
	}
}
