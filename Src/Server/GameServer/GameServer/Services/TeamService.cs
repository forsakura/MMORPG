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
    internal class TeamService : Singleton<TeamService>
    {
        public void Init()
        {
            TeamManager.Instance.Init();
        }

        public TeamService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteRequest>(OnTeamInviteReq);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteResponse>(OnTeamInviteRes);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamLeaveRequest>(OnTeamLeaveReq);
        }

        private void OnTeamInviteReq(NetConnection<NetSession> sender, TeamInviteRequest message)
        {
            Character cha = sender.Session.Character;
            Log.InfoFormat("OnTeamInviteReq:: fromid: {0} fromanme: {1} toid: {2} toname: {3} teamid: {4}", message.TeamId, message.FromName, message.ToId, message.ToName, message.TeamId);
            // TODO: 执行数据校验

            var target = SessionManager.Instance.GetSession(message.ToId);
            if(target == null)
            {
                sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                sender.Session.Response.teamInviteRes.Result = Result.Failed;
                sender.Session.Response.teamInviteRes.Errormsg = "好友已下线";
                sender.SendResponse();
                return;
            }
            if(target.Session.Character.team != null)
            {
                sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                sender.Session.Response.teamInviteRes.Result = Result.Failed;
                sender.Session.Response.teamInviteRes.Errormsg = "好友已组队";
                sender.SendResponse();
                return;
            }

            Log.InfoFormat("ForwardTeamInviteRequest:Fromid: {0} fromName: {1} toId: {2} toName: {3}", message.FromId, message.FromName, message.ToId, message.ToName);
            target.Session.Response.teamInviteReq = message;
            target.SendResponse();
        }

        private void OnTeamInviteRes(NetConnection<NetSession> sender, TeamInviteResponse message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamInviteRes:: fromid: {0} fromName: {1} toiD: {2} toName: {3}", message.Request.ToId, message.Request.ToName, message.Request.FromId, message.Request.FromName);
            sender.Session.Response.teamInviteRes = message;
            if (message.Result == Result.Success)
            {
                var requester = SessionManager.Instance.GetSession(message.Request.FromId);
                if (requester == null)
                {
                    sender.Session.Response.teamInviteRes.Result = Result.Failed;
                    sender.Session.Response.teamInviteRes.Errormsg = "队伍已解散";
                    sender.SendResponse();
                    return;
                }
                else
                {
                    TeamManager.Instance.AddTeamMember(requester.Session.Character, character);
                    requester.Session.Response.teamInviteRes = message;
                    requester.SendResponse();
                }
            }
            sender.SendResponse();
        }

        private void OnTeamLeaveReq(NetConnection<NetSession> sender, TeamLeaveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamLeaveReq:: character: {0}", message.characterId);
            sender.Session.Response.teamLeaveRes = new TeamLeaveResponse();
            if(TeamManager.Instance.RemoveTeamMember(character))
            {
                sender.Session.Response.teamLeaveRes.characterId = character.Id;
                sender.Session.Response.teamLeaveRes.Result = Result.Success;
                sender.Session.Response.teamLeaveRes.Errormsg = "离队成功";
            }
            else
            {
                sender.Session.Response.teamLeaveRes.Result = Result.Failed;
                sender.Session.Response.teamLeaveRes.Errormsg = "离队失败";

            }
            sender.SendResponse();
        }
    }
}
