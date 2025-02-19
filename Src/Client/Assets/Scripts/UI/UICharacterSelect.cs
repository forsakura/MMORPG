using Assets.Scripts.Models;
using Common;
using GameServer.Managers;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSelect : MonoBehaviour {
	public GameObject panelCreate;
	public GameObject panelSelect;
	public Image[] titles;
	public Text[] names;
	public Text description;
	public InputField charName;
	CharacterClass charClass;

	int selectCharacterIndex = -1;

	public Transform uiCharList;
	public GameObject uiCharInfo;
	List<GameObject> uiChars = new List<GameObject>();

	public UICharacterView characterView;
	// Use this for initialization
	void Start () {
		UserService.Instance.OnCharacterCreate += OnCharacterCreate;
		DataManager.Instance.Load();
		InitCharacterSelect(false);
		InitCharacters();
        for (int i = 0; i < names.Length; i++)
        {
			names[i].text = DataManager.Instance.Characters[i + 1].Name;
        }
		description.text = DataManager.Instance.Characters[characterView.CurrentCharacterIndex + 2].Description;
		charClass = DataManager.Instance.Characters[characterView.CurrentCharacterIndex + 2].Class;
    }


    // Update is called once per frame
    void Update () {
		
	}
	
	/// <summary>
	/// 当选择职业时，发生的事件，更新角色面板显示角色，更新标题显示职业，更新职业描述文本
	/// </summary>
	/// <param name="val"></param>
	public void OnSelectClass(int val)
	{
		this.charClass = (CharacterClass)(val + 1);
		characterView.CurrentCharacterIndex = (int)charClass - 1;

		for(int i = 0;i < names.Length;i++)
		{
			titles[i].gameObject.SetActive(i == val);
		}
		description.text = DataManager.Instance.Characters[val + 1].Description;
	}

	public void OnClickCreate()
	{
		if(string.IsNullOrEmpty(charName.text))
		{
			MessageBox.Show("请输入角色姓名");
			return;
		}
		UserService.Instance.SendCharacterCreate(charName.text, charClass);
	}

	public void OnClickPlay()
	{
		if(User.Instance.Info.Player.Characters.Count == 0)
		{
			MessageBox.Show("未创建角色", "提示", MessageBoxType.Information);
			return;
		}
		else if(selectCharacterIndex == -1)
		{
			MessageBox.Show("未选择角色", " 提示", MessageBoxType.Information);
			return;
		}
        else if (selectCharacterIndex != -1)
        {
			UserService.Instance.SendGameEnter(selectCharacterIndex);
        }
    }

	/// <summary>
	/// 当服务端返回响应时
	/// </summary>
	/// <param name="result"></param>
	/// <param name="message"></param>
	public void OnCharacterCreate(Result result, string message)
	{
		if (result == Result.Success)
		{
			InitCharacterSelect(true);
		}
		else
		{
			MessageBox.Show(message, "错误", MessageBoxType.Error);
		}
	}

	void InitCharacterSelect(bool init)
	{
		panelCreate.SetActive(false);
		panelSelect.SetActive(true);

		if(init)
		{
            foreach (var item in uiChars)
            {
				Destroy(item);
            }
			uiChars.Clear();
			for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
			{
				var go = Instantiate(uiCharInfo, uiCharList);
				int index = i;
				go.GetComponent<UICharInfo>().Info = User.Instance.Info.Player.Characters[index];
				go.GetComponent<Button>().onClick.AddListener(() =>
				{
					OnCharacterSelect(index);
				});

				uiChars.Add(go);
				go.SetActive(true);
			}
        }
	}

    private void OnCharacterSelect(int index)
    {
		selectCharacterIndex = index;
		var cha = User.Instance.Info.Player.Characters[index];
		Debug.LogFormat("Select Char : [{0} {1} {2}]", cha.Id, cha.Name, cha.Class);
		characterView.CurrentCharacterIndex = (int)cha.Class - 1;
		for (int i = 0;i < uiCharList.childCount; i++)
		{
			uiChars[i].GetComponent<UICharInfo>().Selected = i == index;
		}
    }

    void InitCharacters()
	{
		for(int i = 0; i < User.Instance.Info.Player.Characters.Count;i++)
		{
            var go = Instantiate(uiCharInfo, uiCharList);
            int index = i;
            go.GetComponent<UICharInfo>().Info = User.Instance.Info.Player.Characters[index];
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnCharacterSelect(index);
            });

            uiChars.Add(go);
            go.SetActive(true);
        }
	}
}
