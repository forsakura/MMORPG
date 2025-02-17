using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIconItem : MonoBehaviour {
	public Image mainImage;
	public Image secondIamge;
	public Text mainText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetMain(string iconName, string Text)
	{
		mainImage.overrideSprite = Resloader.Load<Sprite>(iconName);
		mainText.text = Text;
	}

}
