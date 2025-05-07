using GameServer.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Quest
{
    public class UIQuestRewardItem : MonoBehaviour
    {
        public Image image;
        public Text count;
        public void SetInfo(int itemId, int count)
        {
            image.overrideSprite = Resloader.Load<Sprite>(DataManager.Instance.Items[itemId].Icon);
            this.count.text = count.ToString();
        }
    }
}