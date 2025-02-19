using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.UIBag
{
    public class UIBag : UIWindow
    {
        public Text money;
        public Transform[] pages;
        public GameObject bagItem;
        List<Image> slots;
        // Use this for initialization
        void Start()
        {
            if(slots == null)
            {
                slots = new List<Image>();
                for(int page = 0; page < pages.Length;page++)
                {
                    slots.AddRange(pages[page].transform.GetChild(0).transform.GetChild(0).GetComponentsInChildren<Image>(true));
                }
            }
            StartCoroutine(InitBags());
        }

        IEnumerator InitBags()
        {
            for(int i = 0; i < BagManager.Instance.items.Length; i++)
            {
                var item = BagManager.Instance.items[i];
                if(item.itemID > 0)
                {
                    GameObject go = Instantiate(bagItem, slots[i].transform);
                    var ui = go.GetComponent<UIIconItem>();
                    var def = ItemManager.Instance.items[item.itemID].define;
                    ui.SetMain(def.icon, item.count.ToString());
                }
            }
            for(int j = BagManager.Instance.items.Length; j < slots.Count; j++)
            {
                slots[j].color = Color.gray;
            }
            yield return null;
        }

        private void Update()
        {
            SetMoney();
        }

        public void SetMoney()
        {
            if(money.text != User.Instance.currentCharacter.Gold.ToString()) money.text = User.Instance.currentCharacter.Gold.ToString();
        }

        public void OnReset()
        {
            BagManager.Instance.Reset();
        }
    }
}