using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharInfo : MonoBehaviour {
	public Text levelText;
	public Text nameText;
	public Text ClassText;
    public Image background;
    public Sprite originSprite;
    public Sprite selectedSprite;

    private bool selected;

	public bool Selected
	{
		get 
		{
			return selected;
		}
		set
		{
			selected = value;
			UpdateSelected();
		}
	}

	private NCharacterInfo info;
	public NCharacterInfo Info
	{
		set
		{
			info = value;
			UpdateInfo();
		}
		get
		{
			return info;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateInfo()
	{
		levelText.text = "Level: " + info.Level;
		nameText.text = info.Name;
		ClassText.text = info.Class.ToString();
	}

	void UpdateSelected()
	{
        background.overrideSprite = selected ? selectedSprite : originSprite;
    }
}
