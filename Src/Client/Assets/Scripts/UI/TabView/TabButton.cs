using Assets.Scripts.UI.TabView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour {
	Sprite originSprite;
	public Sprite activeSprite;
	public TabView bagView;
	public int tabIndex;
	public bool selected;
	Image tabImage;

	// Use this for initialization
	void Start () {
		tabImage = GetComponent<Image>();
		originSprite = tabImage.sprite;
		GetComponent<Button>().onClick.AddListener(OnClick);
	}

	public void Selected(bool selected)
	{
		tabImage.overrideSprite = selected ? activeSprite : originSprite;
	}
	
	public void OnClick()
	{
		bagView.SelectTab(tabIndex);
	}
}
