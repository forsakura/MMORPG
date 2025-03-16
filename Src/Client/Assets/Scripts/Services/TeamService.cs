using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Entities;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class TeamService : Singleton<TeamService>, IDisposable
    {

        public void Init()
        {
            MessageDistributer.Instance.Subscribe<TeamInviteRequest>(OnTeamInviteReq);
            MessageDistributer.Instance.Subscribe<TeamInviteResponse>(OnTeamInviteRes);
            MessageDistributer.Instance.Subscribe<TeamInfoResponse>(OnTeamInfo);
            MessageDistributer.Instance.Subscribe<TeamLeaveResponse>(OnTeamLeave);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<TeamInviteRequest>(OnTeamInviteReq);
            MessageDistributer.Instance.Unsubscribe<TeamInviteResponse>(OnTeamInviteRes);
            MessageDistributer.Instance.Unsubscribe<TeamInfoResponse>(OnTeamInfo);
            MessageDistributer.Instance.Unsubscribe<TeamLeaveResponse>(OnTeamLeave);
        }

        /// <summary>
        /// 发送组队邀请
        /// </summary>
        /// <param name="friendId"></param>
        /// <param name="friendName"></param>
        public void SendTeamInviteRequest(int friendId, string friendName)
        {
            Debug.Log("SendTeamInviteRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamInviteReq = new TeamInviteRequest();
            message.Request.teamInviteReq.FromId = User.Instance.currentCharacter.Id;
            message.Request.teamInviteReq.FromName = User.Instance.currentCharacter.Name;
            message.Request.teamInviteReq.ToId = friendId;
            message.Request.teamInviteReq.ToName = friendName;
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// 收到组队邀请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnTeamInviteReq(object sender, TeamInviteRequest message)
        {
            var box = MessageBox.Show(string.Format("玩家[{0}]邀请你加入队伍，是否同意", message.FromName), "组队邀请", MessageBoxType.Confirm, "同意", "拒绝");
            box.OnYes = () =>
            {
                this.SendTeamInviteResponse(true, message);
            };
            box.OnNo = () =>
            {
                this.SendTeamInviteResponse(false, message);
            };
        }

        /// <summary>
        /// 发送请求组队响应
        /// </summary>
        /// <param name="accept"></param>
        /// <param name="request"></param>
        public void SendTeamInviteResponse(bool accept, TeamInviteRequest request)
        {
            Debug.Log("SendTeamInviteResponse");
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.teamInviteRes = new TeamInviteResponse();
            message.Response.teamInviteRes.Result = accept ? Result.Success : Result.Failed;
            message.Response.teamInviteRes.Errormsg = accept ? "组队成功" : "组队失败";
            message.Response.teamInviteRes.Request = request;
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// 收到组队邀请的回应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnTeamInviteRes(object sender, TeamInviteResponse message)
        {
            if (message.Result == Result.Success)
            {
                MessageBox.Show(message.Request.ToName + "加入您的队伍", "邀请组队成功");
            }
            else
            {
                MessageBox.Show(message.Errormsg, "邀请组队失败");
            }
        }

        private void OnTeamInfo(object sender, TeamInfoResponse message)
        {
            Debug.Log("OnTeamInfo");
            TeamManager.Instance.UpdateTeamInfo(message.Team);
        }

        /// <summary>
        /// 发送离开队伍请求
        /// </summary>
        /// <param name="id"></param>
        public void SendTeamLeaveRequest(int id)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Request.teamLeaveReq = new TeamLeaveRequest();
            message.Request.teamLeaveReq.TeamId = User.Instance.TeamInfo.TeamId;
            message.Request.teamLeaveReq.characterId = id;
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// 收到离开队伍响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnTeamLeave(object sender, TeamLeaveResponse message)
        {
            if (message.Result == Result.Success)
            {
                MessageBox.Show("离开成功", "离开队伍");
            }
            else if(message.Result == Result.Failed)
            {
                MessageBox.Show("离开失败", "离开队伍", MessageBoxType.Error);
            }
        }
    }
}