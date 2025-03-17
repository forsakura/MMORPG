using Assets.Scripts.Models;
using Assets.Scripts.Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Team
{
    public class UITeamSystem : MonoBehaviour
    {
        public Text titleText;
        public List<UITeamItem> Members;
        public ListView.ListView list;
        // Use this for initialization
        void Start()
        {
            if(User.Instance.TeamInfo == null)
                this.gameObject.SetActive(false);
            for (int i = 0; i < Members.Count; i++)
            {
                this.list.AddItem(Members[i]);
            }
        }

        private void OnEnable()
        {
            UpdateTeamUI();
        }


        public void Show(bool show)
        {
            this.gameObject.SetActive (show);
            if (show)
                UpdateTeamUI();
        }

        private void UpdateTeamUI()
        {
            if (User.Instance.TeamInfo == null)
                return;
            this.titleText.text = string.Format("我的队伍({0}/5)", User.Instance.TeamInfo.teamMembers.Count);
            for (int i = 0; i < Members.Count; i++)
            {
                if (i < User.Instance.TeamInfo.teamMembers.Count)
                {
                    this.Members[i].SetInfo(i, User.Instance.TeamInfo.teamMembers[i], User.Instance.TeamInfo.Leader == User.Instance.TeamInfo.teamMembers[i].Id);
                    this.Members[i].gameObject.SetActive(true);
                }
                else
                    this.Members[i].gameObject.SetActive(false);
            }
        }

        public void OnClickTeamLeave()
        {
            MessageBox.Show("确定离开队伍吗？", "离开队伍", MessageBoxType.Confirm, "离开", "取消").OnYes = () =>
            {
                TeamService.Instance.SendTeamLeaveRequest(User.Instance.currentCharacter.Id);
            };
        }
    }
}