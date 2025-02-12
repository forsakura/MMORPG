using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : UIWindow {
	public Text titleText;

    private void Start()
    {
        titleText = GetComponentInChildren<Text>();
    }
    public void SetTitle(string title)
	{
		titleText.text = title;
	}
}
