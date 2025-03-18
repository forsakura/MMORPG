using Assets.Scripts.Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Services
{
    internal class GuildService : Singleton<GuildService>, IDisposable
    {
        public UnityAction<bool> OnGuildCreateResult;
        public UnityAction<List<NGuildInfo>> OnGuildListResult;

        public void Init()
        {
            MessageDistributer.Instance.Subscribe<GuildCreateResponse>(OnGuildCreate);
            MessageDistributer.Instance.Subscribe<GuildJoinResponse>(OnGuildJoinReq);
            MessageDistributer.Instance.Subscribe<GuildJoinResponse>(OnGuildJoinRes);
            MessageDistributer.Instance.Subscribe<GuildLeaveResponse>(OnGuildLeave);
            MessageDistributer.Instance.Subscribe<GuildResponse>(OnGuild);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<GuildCreateResponse>(OnGuildCreate);
            MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(OnGuildJoinReq);
            MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(OnGuildJoinRes);
            MessageDistributer.Instance.Unsubscribe<GuildLeaveResponse>(OnGuildLeave);
            MessageDistributer.Instance.Unsubscribe<GuildResponse>(OnGuild);
        }

        internal void SendGuildCreateRequest(string name, string notice)
        {
            Debug.LogFormat("SendGuildCreateRequest:: guildName: {0} guildNotice: {1}", name, notice);
            NetMessage message = new NetMessage();
            message.Request.guildCreateReq = new GuildCreateRequest();
            message.Request.guildCreateReq.GuildName = name;
            message.Request.guildCreateReq.GuildNotice = notice;
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildCreate(object sender, GuildCreateResponse message)
        {
            throw new NotImplementedException();
        }

        internal void SendGuildJoinRequest(int id)
        {
            Debug.LogFormat("SendGuildJoinRequest::guildID: {0}", id);
            NetMessage message = new NetMessage();
            message.Request.guildJoinReq = new GuildJoinRequest();
            message.Request.guildJoinReq.Apply = new NGuildApplyInfo();
            message.Request.guildJoinReq.Apply.GuildId = id;
            message.Request.guildJoinReq.Apply.Name = User.Instance.currentCharacter.Name;
            message.Request.guildJoinReq.Apply.Level = User.Instance.currentCharacter.Level;
            message.Request.guildJoinReq.Apply.Class = (int)User.Instance.currentCharacter.Class;
            message.Request.guildJoinReq.Apply.characterId = User.Instance.currentCharacter.Id;
            NetClient.Instance.SendMessage(message);
        }
        private void OnGuildJoinReq(object sender, GuildJoinResponse message)
        {
            var box = MessageBox.Show(string.Format("玩家{0}请求加入公会{1}，是否同意", message.Apply.characterId, User.Instance.currentCharacter.Guild.GuildName), "申请加入公会", MessageBoxType.Confirm, "同意", "拒绝");
            box.OnYes = () =>
            {
                SendGuildJoinResponse(true, message);
            };
            box.OnNo = () =>
            {
                SendGuildJoinResponse(false, message);
            };
        }

        private void SendGuildJoinResponse(bool v, GuildJoinResponse message)
        {
            NetMessage netMessage = new NetMessage();
            netMessage.Response.guildJoinRes = new GuildJoinResponse();
            netMessage.Response.guildJoinRes.Apply = message.Apply;
            netMessage.Response.guildJoinRes.Result = v ? Result.Success : Result.Failed;
            netMessage.Response.guildJoinRes.Errormsg = v ? "同意申请" : "拒绝申请";
            NetClient.Instance.SendMessage(netMessage);
        }

        private void OnGuildJoinRes(object sender, GuildJoinResponse message)
        {
            if (message.Result == Result.Success)
            {
                MessageBox.Show(string.Format("{0}同意了你的申请", message.Apply.GuildId), "申请答复", MessageBoxType.Information);
            }
            else
                MessageBox.Show(string.Format("{0}拒绝了你的申请", message.Apply.GuildId), "申请答复", MessageBoxType.Information);
        }

        private void OnGuildLeave(object sender, GuildLeaveResponse message)
        {
            throw new NotImplementedException();
        }

        private void OnGuild(object sender, GuildResponse message)
        {
            throw new NotImplementedException();
        }

        internal void SendGuildListRequest()
        {
            Debug.LogFormat("SendGuildListRequest");
            NetMessage message = new NetMessage();
            message.Request.guildJoinReq = new GuildJoinRequest();
            NetClient.Instance.SendMessage(message);
        }
    }
}
