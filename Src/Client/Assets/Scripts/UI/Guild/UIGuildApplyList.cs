using Assets.Scripts.Managers;
using Assets.Scripts.Services;
using System;
using UnityEngine;

namespace Assets.Scripts.UI.Guild
{
    public class UIGuildApplyList : UIWindow
    {

        public UnityEngine.GameObject uiPrefab;
        public ListView.ListView mainView;
        public Transform itemRoot;

        public UIGuildApplyListItem selectedItem;
        // Use this for initialization
        void Start()
        {
            GuildService.Instance.OnGuildUpdate += UpdateUI;
            GuildService.Instance.SendGuildListRequest();
            UpdateUI();

        }

        private void UpdateUI()
        {
            ClearList();
            InitItems();
        }

        private void InitItems()
        {
            foreach (var item in GuildManager.Instance.Info.Applies)
            {
                var go = Instantiate(uiPrefab, itemRoot);
                var ui = go.GetComponent<UIGuildApplyListItem>();
                ui.SetInfo(item);
                mainView.AddItem(ui);
            }
        }

        private void ClearList()
        {
            mainView.RemoveAll();
        }
    }
}