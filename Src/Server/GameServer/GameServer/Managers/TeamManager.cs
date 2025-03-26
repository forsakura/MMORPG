using Common;
using GameServer.Entities;
using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    internal class TeamManager : Singleton<TeamManager>
    {
        private List<Team> Teams = new List<Team>();
        private Dictionary<int, Team> CharacterTeams = new Dictionary<int, Team>();
        public void Init()
        {

        }

        public Team GetTeamByCahracterId(int cahracterId)
        {
            Team team = null;
            CharacterTeams.TryGetValue(cahracterId, out team);
            return team;
        }

        internal void AddTeamMember(Character leader, Character member)
        {
            if(leader.team == null)
            {
                leader.team = CreateTeam(leader);
            }
            leader.team.AddMember(member);
        }

        private Team CreateTeam(Character leader)
        {
            Team team = null;
            for (int i = 0; i < this.Teams.Count; i++)
            {
                team = this.Teams[i];
                if (team.members.Count == 0)
                {
                    team.AddMember(leader);
                    return team;
                }
            }
            team = new Team(leader);
            this.Teams.Add(team);
            team.Id = this.Teams.Count;
            CharacterTeams[team.Id] = team;
            return team;
        }

        public bool RemoveTeamMember(Character member)
        {
            Team team = null;
            CharacterTeams.TryGetValue(member.team.Id, out team);
            team.Leave(member);
            if (team.members.Count == 0)
                CharacterTeams.Remove(team.Id);
            return true;
        }
    }
}
