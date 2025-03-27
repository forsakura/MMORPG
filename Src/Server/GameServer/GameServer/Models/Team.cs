using Common;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Services;
using SkillBridge.Message;
using System.Collections.Generic;

namespace GameServer.Models
{
    public class Team
    {
        public int Id;
        public Character leader;
        public List<Character> members = new List<Character>();
        public int timestamp;

        public Team(Character leader)
        {
            this.leader = leader;
            members.Add(leader);
        }

        public void AddMember(Character member)
        {
            if (members.Count == 0)
                this.leader = member;
            members.Add(member);
            member.team = this;
            timestamp = Time.timestamp;
        }

        public void Leave(Character member)
        {
            Log.InfoFormat("Leave Team: {0} : {1}", member.Id, member.Info.Name);
            this.members.Remove(member);
            if(member == this.leader)
            {
                if(this.members.Count > 0)
                {
                    RemoveAllMembers();
                    //this.leader = this.members[0];
                }
                /*else
                {
                    this.leader = null;
                }*/
                this.leader = null;
                member.team = null;
            }
            timestamp = Time.timestamp;
        }

        /// <summary>
        /// 清空所有成员
        /// </summary>
        public void RemoveAllMembers()
        {
            Log.InfoFormat("RemoveAllMembers");
            for (int i = 0; i < this.members.Count; i++)
            {
                this.members[i].team = null;
                var memberSession = SessionManager.Instance.GetSession(members[i].Id);
                if (memberSession != null)
                {
                    memberSession.Session.Response.teamLeaveRes = new TeamLeaveResponse();
                    memberSession.Session.Response.teamLeaveRes.Result = Result.Success;
                    memberSession.SendResponse();
                }
            }
            this.members.Clear();
        }

        public void PostProcess(NetMessageResponse message)
        {
            if(message.teamInfoRes == null)
            {
                message.teamInfoRes = new TeamInfoResponse();
                message.teamInfoRes.Team = new NTeamInfo();
                message.teamInfoRes.Team.TeamId = this.Id;
                if(this.leader != null) message.teamInfoRes.Team.Leader = this.leader.Id;
                foreach (var item in members)
                {
                    message.teamInfoRes.Team.teamMembers.Add(item.GetBasicInfo());
                }
                message.teamInfoRes.Result = Result.Success;
            }
        }
    }
}
