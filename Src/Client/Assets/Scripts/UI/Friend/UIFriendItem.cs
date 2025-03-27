using Assets.Scripts.UI.ListView;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Friend
{
    public class UIFriendItem : ListView.ListView.ListViewItem
    {
        public Text NickName;
        public Text Level;
        public Text @class;
        public Text Status;

        public Image backGround;
        public Sprite normalSprite;
        public Sprite selectedSprite;

        public NFriendInfo NfriendInfo;

        bool isEquiped = false;

        private void Start()
        {
            backGround.overrideSprite = normalSprite;
        }

        public void SetInfo(NFriendInfo friendInfo)
        {
            this.NfriendInfo = friendInfo;
            if (NickName != null) NickName.text = NfriendInfo.friendInfo.Name;
            if (Level != null) Level.text = NfriendInfo.friendInfo.Level.ToString();
            if (@class != null) @class.text = NfriendInfo.friendInfo.Class.ToString();
            if (Status != null) Status.text = NfriendInfo.Status == 1 ? "在线" : "离线";
        }

        public override void OnSelected(bool selected)
        {
            base.OnSelected(selected);
            backGround.overrideSprite = selected ? selectedSprite : normalSprite;
        }

        public void OnItemSelected(ListView.ListView.ListViewItem item)
        {
            Selected = item == this ? true : false;
        }
    }
}