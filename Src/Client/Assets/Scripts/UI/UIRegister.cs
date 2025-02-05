using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRegister : MonoBehaviour {
	public InputField userName;
	public InputField password;
	public InputField confirmPassword;
	public Button BtnRegister;
	public Button BtnCancel;

	public GameObject uiLogin;
	// Use this for initialization
	void Start () {
		UserService.Instance.OnRegister += OnRegister;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickRegister()
	{
		Debug.LogFormat("userName:{0} passWord:{1} passWordComfirm:{2}", userName.text, password.text, confirmPassword.text);
		if (string.IsNullOrEmpty(userName.text))
		{
			MessageBox.Show("未输入账号");
			return;
		}
		if (string.IsNullOrEmpty(password.text))
		{
			MessageBox.Show("未输入密码");
			return;
		}
		if(string.IsNullOrEmpty(confirmPassword.text))
		{
			MessageBox.Show("未输入确认密码");
			return;
		}
		UserService.Instance.SendRegister(userName.text, password.text);
	}

	public void OnRegister(Result result, string message)
	{
		if (result == Result.Success)
		{
			MessageBox.Show("注册成功，请登录。", "提示", MessageBoxType.Information).OnYes+=CloseRegister;
		}
		else
		{
			MessageBox.Show(message, "错误", MessageBoxType.Error);
		}
	}

	public void CloseRegister()
	{
		this.gameObject.SetActive(false);
		uiLogin.SetActive(true);
	}
}
