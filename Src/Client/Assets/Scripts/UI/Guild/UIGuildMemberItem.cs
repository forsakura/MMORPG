using Common.Utils;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Guild
{
    public class UIGuildMemberItem : ListView.ListView.ListViewItem
    {
        public Text Name;
        public Text Level;
        public Text Class;
        public Text Title;
        public Text JoinTime;
        public Text Status;
        public Image background;
        public Sprite originSprite;
        public Sprite selectedSprite;

        private void Start()
        {
            background.overrideSprite = originSprite;
        }

        public override void OnSelected(bool selected)
        {
            base.OnSelected(selected);
            background.overrideSprite = selected ? selectedSprite : originSprite;
        }

        public NGuildMember member;

        public void SetInfo(NGuildMember member)
        {
            this.member = member;
            if (Name != null) Name.text = this.member.Info.Name;
            if (Level != null) Level.text = this.member.Info.Level.ToString();
            if (Class != null) Class.text = this.member.Info.Class.ToString();
            if (Title != null) Title.text = this.member.Title.ToString();
            if (JoinTime != null) JoinTime.text = TimeUtil.GetTime(this.member.joinTime).ToString();
            if (Status != null) Status.text = this.member.Status == 1 ? "在线" : "离线";
        }
    }
}