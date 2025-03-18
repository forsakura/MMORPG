using Assets.Scripts.UI.ListView;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Guild
{
    public class UIGuildListItem : ListView.ListView.ListViewItem
    {
        public Text ID;
        public Text Name;
        public Text MembersCount;
        public Text Leader;
        public Image background;
        public Sprite originSprite;
        public Sprite selectedSprite;

        public NGuildInfo info;

        public override void OnSelected(bool selected)
        {
            base.OnSelected(selected);
            background.overrideSprite = selected ? selectedSprite : originSprite;
        }

        public void SetInfo(NGuildInfo info)
        {
            this.info = info;
            if (ID != null) ID.text = this.info.Id.ToString();
            if (Name != null) Name.text = this.info.GuildName.ToString();
            if (MembersCount != null) MembersCount.text = this.info.Members.Count.ToString();
            if (Leader != null) Leader.text = this.info.LeaderName.ToString();
        }
    }
}