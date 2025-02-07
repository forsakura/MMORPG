using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainCity : MonoBehaviour {
	public Text nameText;
	public Text levelText;
	public Button BtnBackToSelect;
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
		if (User.Instance.currentCharacter != null)
		{
			string name = string.Format("{0} [{1}]",User.Instance.currentCharacter.Name, User.Instance.currentCharacter.Id);
			string level = User.Instance.currentCharacter.Level.ToString();
			if (name != nameText.text)
			{
				nameText.text = name;
			}
			if(level != levelText.text)
			{
				levelText.text = level;
			}
		}
	}
	public void BackToSelect()
	{
		SceneManager.Instance.LoadScene("CharacterSelect");
		UserService.Instance.SendGameLeave(true);
	}
}
