using Common.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Quest
{
    public class UIQuestItem : ListView.ListView.ListViewItem
    {
        public Image backGround;
        public Text title;
        public Sprite selectedSprite;
        public Sprite normalSprite;

        public override void OnSelected(bool selected)
        {
            backGround.overrideSprite = selected ? selectedSprite : normalSprite; 
        }

        public Models.Quest quest;
        
        public void SetQuestInfo(Models.Quest quest)
        {
            this.quest = quest;
            if (title != null)
            {
                title.text = quest.Define.Name;
            }
        }

    }
}