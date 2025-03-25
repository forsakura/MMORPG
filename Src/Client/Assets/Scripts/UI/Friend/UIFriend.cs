using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Assets.Scripts.UI.Friend
{
    public class UIFriend : UIWindow
    {
        public Button BtnAdd;
        public Button BtnChat;
        public Button BtnRemove;
        public UnityEngine.GameObject UIFriendPrefab;
        public ListView.ListView MainView;
        public Transform friendsListRoot;
        public UIFriendItem selectedItem;


        private void Start()
        {
            FriendService.Instance.OnFriendUpdate = RefreshUI;
            MainView.onItemSelected += OnFriendSelected;
            RefreshUI();
        }

        private void OnDestroy()
        {
            FriendService.Instance.OnFriendUpdate -= RefreshUI;
        }

        private void RefreshUI()
        {
            ClearFriendList();
            InitFriendList();
        }

        private void InitFriendList()
        {
            foreach (var item in FriendManager.Instance.allFriends)
            {
                UnityEngine.GameObject go = Instantiate(UIFriendPrefab, friendsListRoot);
                var ui = go.GetComponent<UIFriendItem>();
                ui.SetInfo(item);
                MainView.AddItem(ui);
            }
        }

        private void ClearFriendList()
        {
            MainView.RemoveAll();
        }

        private void OnFriendSelected(ListView.ListView.ListViewItem arg0)
        {
            selectedItem = arg0 as UIFriendItem;
        }

        public void OnClickFriendAdd()
        {
            InputBox.Show("请输入好友名称或ID", "添加好友", "添加", "取消").OnSubmit += OnFriendAddSubmit;
        }

        private bool OnFriendAddSubmit(string inputText, out string tips)
        {
            tips = "";
            int friendId = 0;
            string friendName = "";
            if(!int.TryParse(inputText, out friendId))
                friendName = inputText;
            if(friendId == User.Instance.currentCharacter.Id || friendName == User.Instance.currentCharacter.Name)
            {
                tips = "不能添加自己";
                return false;
            }

            FriendService.Instance.SendFriendAddRequest(friendId, friendName);
            return true;
        }

        public void Chat()
        {
            MessageBox.Show("暂未开放");
        }

        public void OnClickFriendRemove()
        {
            if (selectedItem == null)
            {
                MessageBox.Show("请选择要删除的好友");
                return;
            }
            MessageBox.Show(string.Format("确认删除好友[{0}]吗？", selectedItem.NfriendInfo.friendInfo.Name), "删除好友", MessageBoxType.Confirm, "删除", "取消").OnYes += () =>
                FriendService.Instance.SendFriendRemoveRequest(selectedItem.NfriendInfo.Id, selectedItem.NfriendInfo.friendInfo.Id);
        }

        public void OnClickFriendTeamInvite()
        {
            if (selectedItem == null)
            {
                MessageBox.Show("请选择要邀请的好友");
                return;
            }
            if(selectedItem.NfriendInfo.Status == 0)
            {
                MessageBox.Show("请选择在线的好友");
            }
            MessageBox.Show(string.Format("确定要邀请好友[{0}]加入队伍吗？", selectedItem.NfriendInfo.friendInfo.Name), "邀请好友组队", MessageBoxType.Confirm, "邀请", "取消").OnYes = () =>
            {
                TeamService.Instance.SendTeamInviteRequest(this.selectedItem.NfriendInfo.friendInfo.Id, this.selectedItem.NfriendInfo.friendInfo.Name);
            };
        }
    }
}