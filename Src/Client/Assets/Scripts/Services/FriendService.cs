using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Network;
using SkillBridge.Message;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Services
{
    internal class FriendService : Singleton<FriendService>, IDisposable
    {
        public UnityAction OnFriendUpdate;

        public void Init()
        {
            MessageDistributer.Instance.Subscribe<FriendAddRequest>(OnFriendAddRequset);
            MessageDistributer.Instance.Subscribe<FriendAddResponse>(OnFriendAddResponse);
            MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(OnFriendRemove);
            MessageDistributer.Instance.Subscribe<FriendListResponse>(OnFriendList);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(OnFriendAddRequset);
            MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(OnFriendAddResponse);
            MessageDistributer.Instance.Unsubscribe<FriendRemoveResponse>(OnFriendRemove);
            MessageDistributer.Instance.Unsubscribe<FriendListResponse>(OnFriendList);
        }

        internal void SendFriendAddRequest(int friendId, string friendName)
        {
            Debug.Log("SendFriendAdd");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddReq = new FriendAddRequest();
            message.Request.friendAddReq.FromId = User.Instance.currentCharacter.Id;
            message.Request.friendAddReq.FromName = User.Instance.currentCharacter.Name;
            message.Request.friendAddReq.ToId = friendId;
            message.Request.friendAddReq.ToName = friendName;
            NetClient.Instance.SendMessage(message);
        }

        private void OnFriendAddResponse(object sender, FriendAddResponse message)
        {
            if (message.Result == Result.Success)
            {
                MessageBox.Show(message.Errormsg, "添加好友成功");
            }
            else
                MessageBox.Show(message.Errormsg, "添加好友失败");
        }

        internal void SendFriendRemoveRequest(int friendId, int NFriendInfoID)
        {
            Debug.Log("SendFriendRemoveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendRemove = new FriendRemoveRequest();
            message.Request.friendRemove.Id = friendId;
            message.Request.friendRemove.FriendId = NFriendInfoID;
            NetClient.Instance.SendMessage(message);
        }

        private void OnFriendRemove(object sender, FriendRemoveResponse message)
        {
            if (message.Result == Result.Success)
            {
                MessageBox.Show("删除成功", "删除好友");
            }
            else
                MessageBox.Show("删除失败", "删除好友", MessageBoxType.Error);
        }

        private void OnFriendList(object sender, FriendListResponse message)
        {
            Debug.Log("OnFriendList");
            FriendManager.Instance.allFriends = message.Friends;
            if (this.OnFriendUpdate != null)
                OnFriendUpdate();
        }

        public void SendFriendAddReponse(bool accept, FriendAddRequest request)
        {
            Debug.Log("Send FriendAdd");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddRes = new FriendAddResponse();
            message.Request.friendAddRes.Result = accept ? Result.Success : Result.Failed;
            message.Request.friendAddRes.Errormsg = accept ? "对方同意" : "对方拒绝了你的请求";
            message.Request.friendAddRes.Request = request;
            NetClient.Instance.SendMessage(message);
        }

        private void OnFriendAddRequset(object sender, FriendAddRequest message)
        {
            var box = MessageBox.Show(string.Format("[{0}]请求加你为好友", message.FromName), "好友请求", MessageBoxType.Confirm, "接受", "拒绝");
            box.OnYes = () =>
            {
                this.SendFriendAddReponse(true, message);
            };
            box.OnNo = () =>
            {
                this.SendFriendAddReponse(false, message);
            };
        }
    }
}
