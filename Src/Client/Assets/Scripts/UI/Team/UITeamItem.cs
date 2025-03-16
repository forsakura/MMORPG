using Assets.Scripts.Managers;
using Assets.Scripts.UI.ListView;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Team
{
    public class UITeamItem :  ListView.ListView.ListViewItem
    {
        public Image classImage;
        public Text levelText;
        public Text nameText;
        public Image leaderImage;
        public Image background;
        public Sprite originSprite;
        public Sprite selectedSprite;
        public NCharacterInfo Info;
        public int index;

        public override void OnSelected(bool selected)
        {
            base.OnSelected(selected);
            this.background.overrideSprite = selected ? selectedSprite : originSprite;
        }

        private void Start()
        {
            this.background.overrideSprite = originSprite;
        }

        public void SetInfo(int index, NCharacterInfo info, bool isLeader)
        {
            this.Info = info;
            this.index = index;
            if (levelText != null) this.levelText.text = Info.Level.ToString();
            if (nameText != null) this.nameText.text = Info.Name.ToString();
            if (leaderImage != null) this.leaderImage.gameObject.SetActive(isLeader);
            if (classImage != null) this.classImage.overrideSprite = SpriteManager.Instance.classIcons[(int)this.Info.Class - 1];
        }
    }
}