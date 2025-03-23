using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.UIElements;

namespace Assets.Scripts.UI.Guild
{
    internal class UIGuild : UIWindow
    {
        public UIGuildInfo info;
        public ListView.ListView mainView;
        public UIGuildItem selectedItem;
        public UnityEngine.GameObject uiPrefab;
        public Button btnTransfer;
        public Button btnUp;
        public Button btnDown;
        public Button btnApplyList;
        public Button btnKick;
        public Button btnChat;
        public Button btnLeave;

        private void Start()
        {
            this.mainView.onItemSelected += OnGuildSelectedItem;
            GuildService.Instance.OnGuildUpdate += GuildUpdate;
            this.GuildUpdate();
        }

        private void OnDestroy()
        {
            GuildService.Instance.OnGuildUpdate -= GuildUpdate;
        }

        private void GuildUpdate()
        {
            this.info.Info = GuildManager.Instance.Info;

            ClearList();
            InitItems();
        }

        private void InitItems()
        {
            foreach (var item in GuildManager.Instance.Info.Members)
            {
                var go = Instantiate(uiPrefab, mainView.transform);
                var ui = go.GetComponent<UIGuildItem>();
                ui.SetInfo(item);
                go.SetActive(true);
                mainView.AddItem(ui);
            }
        }

        private void ClearList()
        {
            mainView.RemoveAll();
        }

        private void OnGuildSelectedItem(ListView.ListView.ListViewItem arg0)
        {
            selectedItem = arg0 as UIGuildItem;
        }

        public void OnClickTransfer()
        {
            Debug.Log("ClickTeansfer");
        }

        public void OnClickUp()
        {
            Debug.Log("ClickUp");
        }

        public void OnClickDown()
        {
            Debug.Log("ClickDown");
        }

        public void OnClickApplyList()
        {
            Debug.Log("ClickApplyList");
        }

        public void OnClickKick()
        {
            Debug.Log("ClickKick");
        }

        public void OnClickChat()
        {
            Debug.Log("ClickChat");
        }

        public void OnClickLeave()
        {
            Debug.Log("ClickLeave");
        }
    }
}
