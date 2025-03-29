using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.UI.Guild;
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
        public UnityAction OnGuildUpdate;
        public UnityAction<bool> OnGuildCreateResult;
        public UnityAction<List<NGuildInfo>> OnGuildListResult;

        public void Init()
        {
            MessageDistributer.Instance.Subscribe<GuildCreateResponse>(OnGuildCreate);
            MessageDistributer.Instance.Subscribe<GuildListResponse>(OnGuildList);
            MessageDistributer.Instance.Subscribe<GuildJoinRequest>(OnGuildJoinReq);
            MessageDistributer.Instance.Subscribe<GuildJoinResponse>(OnGuildJoinRes);
            MessageDistributer.Instance.Subscribe<GuildLeaveResponse>(OnGuildLeave);
            MessageDistributer.Instance.Subscribe<GuildResponse>(OnGuild);
            MessageDistributer.Instance.Subscribe<GuildSearchResponse>(OnGuildSearch);
            MessageDistributer.Instance.Subscribe<GuildAdminResponse>(OnGuildAdmin);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<GuildCreateResponse>(OnGuildCreate);
            MessageDistributer.Instance.Unsubscribe<GuildListResponse>(OnGuildList);
            MessageDistributer.Instance.Unsubscribe<GuildJoinRequest>(OnGuildJoinReq);
            MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(OnGuildJoinRes);
            MessageDistributer.Instance.Unsubscribe<GuildLeaveResponse>(OnGuildLeave);
            MessageDistributer.Instance.Unsubscribe<GuildResponse>(OnGuild);
            MessageDistributer.Instance.Unsubscribe<GuildSearchResponse>(OnGuildSearch);
            MessageDistributer.Instance.Unsubscribe<GuildAdminResponse>(OnGuildAdmin);
        }

        internal void SendGuildCreateRequest(string name, string notice)
        {
            Debug.LogFormat("SendGuildCreateRequest:: guildName: {0} guildNotice: {1}", name, notice);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildCreateReq = new GuildCreateRequest();
            message.Request.guildCreateReq.GuildName = name;
            message.Request.guildCreateReq.GuildNotice = notice;
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildCreate(object sender, GuildCreateResponse message)
        {
            Debug.LogFormat("OnGuildCreate:: Result: {0} Errormsg: {1}", message.Result, message.Errormsg);
            if(message.guildInfo != null)
            {
                Debug.LogFormat("OnGuildCreate:: guild: {0} leader : {1}", message.guildInfo.GuildName, message.guildInfo.LeaderName);
                this.OnGuildCreateResult(message.Result == Result.Success);
            }
            if(message.Result == Result.Success)
            {
                GuildManager.Instance.Init(message.guildInfo);
                MessageBox.Show(string.Format("公会创建成功\n[{0}]", message.guildInfo.GuildName), "公会");
            }
            else
            {
                MessageBox.Show(string.Format("公会创建失败\n[{0}]", message.Errormsg), "公会");
            }
        }

        internal void SendGuildJoinRequest(int id)
        {
            Debug.LogFormat("SendGuildJoinRequest::guildID: {0}", id);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinReq = new GuildJoinRequest();
            message.Request.guildJoinReq.Apply = new NGuildApplyInfo();
            message.Request.guildJoinReq.Apply.GuildId = id;
            NetClient.Instance.SendMessage(message);
        }
        private void OnGuildJoinReq(object sender, GuildJoinRequest message)
        {
            Debug.Log("OnGuildJoinReq");
            var box = MessageBox.Show(string.Format("玩家{0}请求加入公会，是否同意", message.Apply.Name), "公会申请", MessageBoxType.Confirm, "同意", "拒绝");
            box.OnYes = () =>
            {
                SendGuildJoinResponse(true, message);
            };
            box.OnNo = () =>
            {
                SendGuildJoinResponse(false, message);
            };
        }

        private void SendGuildJoinResponse(bool v, GuildJoinRequest message)
        {
            NetMessage netMessage = new NetMessage();
            netMessage.Request = new NetMessageRequest();
            netMessage.Request.guildJoinRes = new GuildJoinResponse();
            netMessage.Request.guildJoinRes.Result = Result.Success;
            netMessage.Request.guildJoinRes.Apply = message.Apply;
            netMessage.Request.guildJoinRes.Apply.Result = v ? ApplyResult.Acept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(netMessage);
        }

        private void OnGuildJoinRes(object sender, GuildJoinResponse message)
        {
            Debug.LogFormat("OnGuildJoinResponse::{0}", message.Result);
            if (message.Result == Result.Success)
            {
                MessageBox.Show("加入公会成功", "公会");
            }
            else
                MessageBox.Show(string.Format("加入公会失败\n{0}", message.Errormsg), "公会");
        }

        public void SendGuildLeaveRequest()
        {
            Debug.Log("SendGuildLeaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildLeave = new GuildLeaveRequest();
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildLeave(object sender, GuildLeaveResponse message)
        {
            if (message.Result == Result.Success)
            {
                GuildManager.Instance.Init(null);
                MessageBox.Show("离开公会成功", "公会");
            }
            else if (message.Result == Result.Failed)
            {
                MessageBox.Show(string.Format("离开公会失败\n{0}", message.Errormsg), "公会");
            }
        }

        private void OnGuild(object sender, GuildResponse message)
        {
            Debug.LogFormat("OnGuild:: {0} {1} : {2}", message.Result, message.Guild.Id, message.Guild.GuildName);
            GuildManager.Instance.Init(message.Guild);
            if (this.OnGuildUpdate != null)
                this.OnGuildUpdate();
        }

        internal void SendGuildListRequest()
        {
            Debug.LogFormat("SendGuildListRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildList = new GuildListRequest();
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildList(object sender, GuildListResponse message)
        {
            Debug.LogFormat("OnGuildList::Count: {0}", message.Guilds.Count);
            if (OnGuildListResult != null)
                OnGuildListResult(message.Guilds);
        }

        internal void SendGuildSearchRequest(int guildId)
        {
            Debug.LogFormat("SendGuildSearchRequest:: guild: {0}", guildId);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildSearch = new GuildSearchRequest();
            message.Request.guildSearch.guildId = guildId;
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildSearch(object sender, GuildSearchResponse message)
        {
            Debug.LogFormat("OnGuildSearch");
            if(message.Result == Result.Success)
            {
                if (OnGuildListResult != null)
                    OnGuildListResult(message.Guilds);
            }
            else if(message.Result == Result.Failed)
            {
                MessageBox.Show(string.Format("搜索失败\n{0}", message.Errormsg), "公会", MessageBoxType.Information);
            }
        }

        public void SendGuildJoinApply(bool accept, NGuildApplyInfo apply)
        {
            Debug.Log("SendGuildJoinApply");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinRes = new GuildJoinResponse();
            message.Request.guildJoinRes.Apply = apply;
            message.Request.guildJoinRes.Result = Result.Success;
            message.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Acept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(message);
        }

        internal void SendGuildAdminCommandRequest(GuildAdminCommand command, int characterId)
        {
            Debug.LogFormat("SendGuildAdminCommandRequest:: command: {0}", command);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildAdmin = new GuildAdminRequest();
            message.Request.guildAdmin.Command = command;
            message.Request.guildAdmin.Target = characterId;
            NetClient.Instance.SendMessage(message);
        }

        private void OnGuildAdmin(object sender, GuildAdminResponse message)
        {
            Debug.LogFormat("OnGuildAdmin:: {0} {1}", message.Command, message.Result);
            MessageBox.Show(string.Format("执行操作：{0} 结果： {1} {2}", message.Command.Command.ToString(), message.Result, message.Errormsg));
        }
    }
}
