using Assets.Scripts.Models;
using Assets.Scripts.UI;
using SkillBridge.Message;

namespace Assets.Scripts.Managers
{
    internal class TeamManager : Singleton<TeamManager>
    {
        public void Init()
        {

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
    }
}
