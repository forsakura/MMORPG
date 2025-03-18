using Assets.Scripts.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Guild
{
    public class UIGuildPopCreate : UIWindow
    {
        public InputField InputName;
        public InputField InputNotice;

        private void Start()
        {
            GuildService.Instance.OnGuildCreateResult = OnGuildCreated;
        }

        private void OnDestroy()
        {
            GuildService.Instance.OnGuildCreateResult = null;
        }

        public override void OnYesClick()
        {
            base.OnYesClick();
            if(string.IsNullOrEmpty(InputName.text))
            {
                MessageBox.Show("请输入公会名称", "错误", MessageBoxType.Error);
                return;
            }
            if(InputName.text.Length < 2 || InputName.text.Length > 5)
            {
                MessageBox.Show("公会名称为2-5个汉字", "错误", MessageBoxType.Error);
                return;
            }
            if (string.IsNullOrEmpty(InputNotice.text))
            {
                MessageBox.Show("请输入公会宣言", "错误", MessageBoxType.Error);
                return;
            }
            if(InputNotice.text.Length < 6 || InputNotice.text.Length > 100)
            {
                MessageBox.Show("公会宣言为3-50个汉字", "错误", MessageBoxType.Error);
                return;
            }

            GuildService.Instance.SendGuildCreateRequest(InputName.text, InputNotice.text);
        }

        void OnGuildCreated(bool result)
        {
            if(result) 
                this.Close(WindowResult.Yes);
        }
    }
}