using Common.Data;
using GameServer.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Shop
{
    public class UIShopItem : MonoBehaviour, ISelectHandler
    {
        UIShop shop;
        public Transform slot;
        public Text title;
        public Text shopItemCount;
        public Text shopItemPrice;
        public Text shopItemClass;

        public Image bg;
        public Image icon;
        public Sprite normalSprite;
        public Sprite selectedSprite;
        bool selected;
        public bool Selected
        {  
            get { return selected; }
            set 
            { 
                selected = value;
                bg.overrideSprite = selected ? selectedSprite : normalSprite;
            }
        }

        public int shopItemID;
        ItemDefine itemDefine;
        ShopItemDefine shopItemDefine { get; set; }

        internal void SetItem(int id, ShopItemDefine value, UIShop shop)
        {
            this.shop = shop;
            shopItemID = id;
            shopItemDefine = value;
            itemDefine = DataManager.Instance.Items[shopItemDefine.itemID];

            title.text = itemDefine.Name;
            shopItemCount.text = shopItemDefine.Count.ToString();
            shopItemPrice.text = shopItemDefine.Price.ToString();
            if(itemDefine.LimitClass != SkillBridge.Message.CharacterClass.None) shopItemClass.text = itemDefine.LimitClass.ToString();
            icon.overrideSprite = Resloader.Load<Sprite>(itemDefine.Icon);
        }

        public void OnSelect(BaseEventData eventData)
        {
            Selected = true;
            shop.SelectItem(this);
        }
    }
}