using Assets.Scripts.Models;
using Assets.Scripts.UI;
using Common;
using SkillBridge.Message;

namespace Assets.Scripts.Managers
{
    internal class TeamManager : Singleton<TeamManager>
    {
        public void Init()
        {

        }

        public bool IsFull 
        { 
            get
            {
                return User.Instance.TeamInfo.teamMembers.Count == GameDefine.TeamMaxMemberCount;
            } 
        }

        public void UpdateTeamInfo(NTeamInfo team)
        {
            User.Instance.TeamInfo = team;
            ShowTeamUI(team != null);
        }

        public void ShowTeamUI(bool show)
        {
            if (UIMain.Instance != null)
            {
                UIMain.Instance.ShowTeamUI(show);
            }
        }

        public bool HasTeamMember(int  memberId)
        {
            foreach (var teamMember in User.Instance.TeamInfo.teamMembers)
            {
                if(teamMember.Id == memberId)
                    return true;
            }
            return false;
        }
    }
}
