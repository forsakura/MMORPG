using Common.Data;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : Singleton<User> {
	NUserInfo userInfo;

	public NCharacterInfo currentCharacter;

	public MapDefine curentMiniMap {  get; set; }

	public GameObject currentCharacterObject {  get; set; }
	public NUserInfo Info 
	{
		get { return this.userInfo; }
	}

	public void SetUserInfo(NUserInfo info)
	{
		this.userInfo = info;
	}
}
