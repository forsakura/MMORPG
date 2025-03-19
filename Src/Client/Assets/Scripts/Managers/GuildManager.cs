using Assets.Scripts.UI;
using Assets.Scripts.UI.Guild;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Managers
{
    internal class GuildManager : Singleton<GuildManager>
    {
        public NGuildInfo Info;

        public bool HasGuild { get { return Info != null; } }

        public void Init(NGuildInfo info)
        {
            this.Info = info;
        }

        public void ShowGuild()
        {
            if (this.HasGuild)
            {
                UIManager.Instance.Show<UIGuild>();
            }
            else
            {
                var guild = UIManager.Instance.Show<UIGuildPopNoGuild>();
                guild.onClose += PopNoGuild_OnClose;
            }
        }

        private void PopNoGuild_OnClose(UIWindow window, UIWindow.WindowResult result)
        {
            if(result == UIWindow.WindowResult.Yes)
            {
                UIManager.Instance.Show<UIGuildPopCreate>();
            }
            else if(result == UIWindow.WindowResult.No)
            {
                UIManager.Instance.Show<UIGuildList>();
            }
        }
    }
}
