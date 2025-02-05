using Services;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UILogin : MonoBehaviour
	{

		public InputField userName;

		public InputField passWord;

		public Button BtnLogin;

		public Button BtnOpenRegister;
		// Use this for initialization
		void Start ()
		{
			UserService.Instance.OnLogin += OnLogin;
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		public void OnClickLogin()
		{
			if (string.IsNullOrEmpty(userName.text))
			{
				MessageBox.Show("未输入账号");
				return;
			}

			if (string.IsNullOrEmpty(passWord.text))
			{
				MessageBox.Show("未输入密码");
				return;
			}
			UserService.Instance.SendLogin(userName.text, passWord.text);
		}

		public void OnLogin(Result res, string val)
		{
			if (res == Result.Success)
			{
				MessageBox.Show("登录成功，请准备选择角色" + val, "提示", MessageBoxType.Information);
				SceneManager.Instance.LoadScene("CharacterSelect");
			}
			else
			{
				MessageBox.Show(val, "错误", MessageBoxType.Error);
			}
		}
	}
}
