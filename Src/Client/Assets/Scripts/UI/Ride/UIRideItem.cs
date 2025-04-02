using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Ride
{
    public class UIRideItem : ListView.ListView.ListViewItem 
    {
        public Image Icon;
        public Text Name;
        public Text Level;
        public Image background;
        public Sprite originSprite;
        public Sprite selectedSprite;

        public RideDefine ride;
        // Use this for initialization
        void Start()
        {
            background.overrideSprite = originSprite;
        }
        public void SetInfo(RideDefine ride)
        {
            this.ride = ride;
            Icon.overrideSprite = Resloader.Load<Sprite>(this.ride.Icon);
            Name.text = this.ride.Name;
            Level.text = this.ride.Level.ToString();
        }

        public override void OnSelected(bool selected)
        {
            base.OnSelected(selected);
            this.background.overrideSprite = selected ? selectedSprite : originSprite;
        }
    }
}