using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using SkillBridge.Message;
using UnityEngine;

namespace Assets.Scripts.UI.Guild
{
    internal class UIGuild : UIWindow
    {
        public UIGuildInfo info;
        public ListView.ListView mainView;
        public UIGuildMemberItem selectedItem;
        public UnityEngine.GameObject uiPrefab;

        public UnityEngine.GameObject AdminBtns;
        public UnityEngine.GameObject LeaderBtns;

        private void Start()
        {
            this.mainView.onItemSelected += OnGuildSelectedItem;
            GuildService.Instance.OnGuildUpdate += GuildUpdate;
            this.GuildUpdate();
        }

        private void OnDestroy()
        {
            GuildService.Instance.OnGuildUpdate -= GuildUpdate;
        }

        private void GuildUpdate()
        {
            this.info.Info = GuildManager.Instance.Info;

            ClearList();
            InitItems();

            this.AdminBtns.SetActive(GuildManager.Instance.MyGuildMemberInfo.Title > SkillBridge.Message.GuildTitle.None);
            this.LeaderBtns.SetActive(GuildManager.Instance.MyGuildMemberInfo.Title == SkillBridge.Message.GuildTitle.President);
        }

        private void InitItems()
        {
            foreach (var item in GuildManager.Instance.Info.Members)
            {
                var go = Instantiate(uiPrefab, mainView.transform);
                var ui = go.GetComponent<UIGuildMemberItem>();
                ui.SetInfo(item);
                go.SetActive(true);
                mainView.AddItem(ui);
            }
        }

        private void ClearList()
        {
            mainView.RemoveAll();
        }

        private void OnGuildSelectedItem(ListView.ListView.ListViewItem arg0)
        {
            selectedItem = arg0 as UIGuildMemberItem;
            foreach (var item in this.mainView.items)
            {
                item.Selected = selectedItem == item;
            }
        }

        public void OnClickTransfer()
        {
            Debug.Log("ClickTeansfer");
            if (selectedItem == null)
            {
                MessageBox.Show("请选择要转让会长的玩家", "公会", MessageBoxType.Information);
                return;
            }

            MessageBox.Show(string.Format("确定将会长转让给{0}吗", selectedItem.member.Info.Name), "公会", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
            {
                GuildService.Instance.SendGuildAdminCommandRequest(GuildAdminCommand.Transfer, selectedItem.member.characterId);
            };
        }

        public void OnClickUp()
        {
            Debug.Log("ClickUp");
            if (selectedItem == null)
            {
                MessageBox.Show("请选择要提拔的玩家", "公会", MessageBoxType.Information);
                return;
            }

            if(selectedItem.member.Title != GuildTitle.None)
            {
                MessageBox.Show("对方身份已无法提拔");
                return;
            }

            MessageBox.Show(string.Format("确定提拔玩家{0}吗", selectedItem.member.Info.Name), "公会", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
            {
                GuildService.Instance.SendGuildAdminCommandRequest(GuildAdminCommand.Promote, selectedItem.member.characterId);
            };
        }

        public void OnClickDown()
        {
            Debug.Log("ClickDown");
            if (selectedItem == null)
            {
                MessageBox.Show("请选择要罢免的玩家", "公会", MessageBoxType.Information);
                return;
            }

            if(selectedItem.member.Title == GuildTitle.None)
            {
                MessageBox.Show("对方身份已无法罢免");
                return;
            }

            if(selectedItem.member.Title == GuildTitle.President)
            {
                MessageBox.Show("会长身份不可动摇");
                return;
            }

            MessageBox.Show(string.Format("确定罢免玩家{0}吗", selectedItem.member.Info.Name), "公会", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
            {
                GuildService.Instance.SendGuildAdminCommandRequest(GuildAdminCommand.Depost, selectedItem.member.characterId);
            };
        }

        public void OnClickApplyList()
        {
            Debug.Log("ClickApplyList");
            GuildManager.Instance.ShowApplyList();
        }

        public void OnClickKick()
        {
            Debug.Log("ClickKick");
            if(selectedItem == null)
            {
                MessageBox.Show("请选择要踢出公会的玩家", "公会", MessageBoxType.Information);
                return;
            }

            if(selectedItem.member.Title != GuildTitle.None)
            {
                MessageBox.Show("对方身份在公会中很重要，不可踢出公会");
                return;
            }

            MessageBox.Show(string.Format("确定踢出玩家{0}吗", selectedItem.member.Info.Name), "公会", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
            {
                GuildService.Instance.SendGuildAdminCommandRequest(GuildAdminCommand.Kickout, selectedItem.member.characterId);
            };
        }

        public void OnClickChat()
        {
            Debug.Log("ClickChat");
            if (selectedItem == null)
            {
                MessageBox.Show("请选择要聊天的玩家", "公会", MessageBoxType.Information);
                return;
            }

            ChatManager.Instance.StartPrivateChat(selectedItem.member.characterId, selectedItem.member.Info.Name);
            this.Close(WindowResult.None);
        }

        public void OnClickLeave()
        {
            MessageBox.Show(string.Format("确定要离开公会{0}吗？", GuildManager.Instance.Info.GuildName), "离开公会", MessageBoxType.Confirm, "离开", "取消").OnYes = () =>
            {
                GuildService.Instance.SendGuildLeaveRequest();
            };
        }

        public void OnSetNotice()
        {

        }
    }
}
