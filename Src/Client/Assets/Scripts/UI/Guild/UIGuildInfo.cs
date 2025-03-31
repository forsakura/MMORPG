using Common;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Guild
{
    public class UIGuildInfo : MonoBehaviour
    {
        public Text guildName;
        public Text ID;
        public Text notice;
        public Text memberCount;
        public Text leader;

        private NGuildInfo info;
        public NGuildInfo Info
        {
            get { return info; }
            set
            {
                info = value;
                this.RefreshUI();
            }
        }

        private void RefreshUI()
        {
            if(this.info == null)
            {
                this.guildName.text = "无";
                this.ID.text = "ID:0";
                this.notice.text = "";
                this.leader.text = "会长：无";
                this.memberCount.text = string.Format("成员数量：0/{0}", GameDefine.GuildMaxMemberCount);
            }
            else
            {
                this.guildName.text = this.info.GuildName;
                this.ID.text = "ID:" + this.info.Id.ToString();
                this.notice.text = this.info.Notice.ToString();
                this.leader.text = "会长" + this.info.LeaderName.ToString();
                this.memberCount.text = string.Format("成员数量：{0}/{1}", this.info.Members.Count, GameDefine.GuildMaxMemberCount);
            }
        }
    }
}