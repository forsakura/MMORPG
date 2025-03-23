using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    internal class FriendService : Singleton<FriendService>
    {
        public FriendService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddRequest>(OnFriendAddRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddResponse>(OnFriendAddResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendRemoveRequest>(OnFriendRemove);
        }

        public void Init()
        {

        }

        private void OnFriendAddRequest(NetConnection<NetSession> sender, FriendAddRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddRequest: FromId: {0} FromName: {1} ToId: {2} ToName: {3}", message.FromId, message.FromName, message.ToId, message.ToName);
            if(message.ToId == 0)
            {
                foreach (var item in CharacterManager.Instance.Characters)
                {
                    if (item.Value.Name == character.Name)
                    {
                        message.ToId = item.Key;
                        break;
                    }
                }
            }

            NetConnection<NetSession> friend = null;
            if(message.ToId > 0)
            {
                if (character.FriendManager.GetFriendInfo(message.ToId) != null)
                {
                    sender.Session.Response.friendAddRes = new FriendAddResponse();
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.Session.Response.friendAddRes.Errormsg = "已经是好友了";
                    sender.SendResponse();
                    return;
                }
                friend = SessionManager.Instance.GetSession(message.ToId);
            }
            if(friend == null)
            {
                sender.Session.Response.friendAddRes = new FriendAddResponse();
                sender.Session.Response.friendAddRes.Result = Result.Failed;
                sender.Session.Response.friendAddRes.Errormsg = "好友不存在或不在线";
                sender.SendResponse();
                return;
            }

            Log.InfoFormat("ForwardRequest:: FromId: {0} FromName: {1} ToId: {2} ToName: {3}", message.FromId, message.FromName, message.ToId, message.ToName);
            friend.Session.Response.friendAddReq = message;
            friend.SendResponse();
        }

        private void OnFriendAddResponse(NetConnection<NetSession> sender, FriendAddResponse message)
        {
            Character cha = sender.Session.Character;
            Log.InfoFormat("OnFriendAddResponse: FromId: {0} FromName: {1} ToId: {2} ToName: {3}", cha.Id, message.Request.ToName, message.Request.FromId, message.Request.FromName);
            sender.Session.Response.friendAddRes = message;
            if(message.Result == Result.Success)
            {
                var requester = SessionManager.Instance.GetSession(message.Request.FromId);
                if (requester == null)
                {
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.Session.Response.friendAddRes.Errormsg = "请求者已下线";
                }
                else
                {
                    cha.FriendManager.AddFriend(requester.Session.Character);
                    requester.Session.Character.FriendManager.AddFriend(cha);
                    DBService.Instance.Save();
                    requester.Session.Response.friendAddRes = message;
                    requester.Session.Response.friendAddRes.Result = Result.Success;
                    requester.Session.Response.friendAddRes.Errormsg = "添加好友成功";
                    requester.SendResponse();
                }
                sender.SendResponse();
            }
            else
            {
                var requester = SessionManager.Instance.GetSession(message.Request.FromId);
                if (requester != null)
                {
                    requester.Session.Response.friendAddRes = message;
                    requester.SendResponse();
                }
            }
        }

        private void OnFriendRemove(NetConnection<NetSession> sender, FriendRemoveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendRemove: characterId: {0} friend: {1}", character.Id, message.FriendId);
            sender.Session.Response.friendRemove = new FriendRemoveResponse();
            sender.Session.Response.friendRemove.Id = message.Id;

            if (character.FriendManager.RemoveFriendByFriendId(message.FriendId))
            {
                sender.Session.Response.friendRemove.Result = Result.Success;
                var friend = SessionManager.Instance.GetSession(message.FriendId);
                if (friend != null)
                {
                    friend.Session.Character.FriendManager.RemoveFriendByFriendId(character.Id);
                }
                else
                {
                    RemoveFriend(message.FriendId, character.Id);
                }
            }
            else
                sender.Session.Response.friendRemove.Result = Result.Failed;

            DBService.Instance.Save();
            sender.SendResponse();
        }

        private void RemoveFriend(int chaId, int friendId)
        {
            var removeItem = DBService.Instance.Entities.CharacterFriends.FirstOrDefault(v => v.CharacterID == chaId && v.FriendID == friendId);
            if (removeItem != null)
            {
                DBService.Instance.Entities.CharacterFriends.Remove(removeItem);
            }
        }
    }
}
