using Assets.Scripts.Services;
using SkillBridge.Message;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Guild
{
    public class UIGuildApplyListItem : ListView.ListView.ListViewItem
    {
        public Text Name;
        public Text Level;
        public Text Class;

        public NGuildApplyInfo member;

        public void SetInfo(NGuildApplyInfo member)
        {
            this.member = member;
            this.Name.text = this.member.Name;
            this.Level.text = this.member.Level.ToString();
            switch (this.member.Class)
            {
                case 1 :
                    this.Class.text = "战士";
                    break;
                case 2 :
                    this.Class.text = "法师";
                    break;
                case 3 :
                    this.Class.text = "射手";
                    break;
                default:
                    break;
            }
        }

        public void OnYes()
        {
            MessageBox.Show(string.Format("要同意{0}加入公会吗？", this.member.Name), "审批申请", MessageBoxType.Confirm, "同意", "取消").OnYes = () =>
            {
                GuildService.Instance.SendGuildJoinApply(true, member);
            };
        }

        public void OnNo()
        {
            MessageBox.Show(string.Format("要拒绝{0}加入公会吗？", this.member.Name), "审批申请", MessageBoxType.Confirm, "拒绝", "取消").OnYes = () =>
            {
                GuildService.Instance.SendGuildJoinApply(false, member);
            };
        }
    }
}