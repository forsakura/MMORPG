﻿using Assets.Scripts.Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Guild
{
    public class UIGuildList : UIWindow
    {
        public UIGuildInfo UIInfo;
        public UnityEngine.GameObject uiPrefab;
        public UIGuildListItem selectedItem;
        public ListView.ListView mainView;


        // Use this for initialization
        void Start()
        {
            this.mainView.onItemSelected += OnGuildItemSelected;
            this.UIInfo.Info = null;
            GuildService.Instance.OnGuildListResult += UpdateGuildList;

            GuildService.Instance.SendGuildListRequest();
        }


        private void OnDestroy()
        {
            GuildService.Instance.OnGuildListResult -= UpdateGuildList;
        }

        void UpdateGuildList(List<NGuildInfo> list)
        {
            ClearList();
            InitItems(list);
        }

        private void ClearList()
        {
            mainView.RemoveAll();
        }

        private void InitItems(List<NGuildInfo> list)
        {
            foreach (var item in list)
            {
                var go = Instantiate(uiPrefab, this.mainView.transform);
                var ui = go.GetComponent<UIGuildListItem>();
                ui.SetInfo(item);
                this.mainView.AddItem(ui);
            }
        }

        private void OnGuildItemSelected(ListView.ListView.ListViewItem item)
        {
            this.selectedItem = item as UIGuildListItem;
            foreach (var item1 in mainView.items)
            {
                item1.Selected = item1 == this.selectedItem;
            }
            this.UIInfo.Info = this.selectedItem.info;
        }

        public void OnClickJoin()
        {
            if (this.selectedItem == null)
            {
                MessageBox.Show("请选择要加入的公会");
                return;
            }
            MessageBox.Show(string.Format("确定要加入公会{0}吗？", this.selectedItem.info.GuildName), "申请加入公会", MessageBoxType.Confirm, "加入", "取消").OnYes = () =>
            {
                GuildService.Instance.SendGuildJoinRequest(this.selectedItem.info.Id);
            };
        }

        public void OnClickSearch()
        {
            InputBox.Show("请输入公会ID", "公会", "搜索", "取消").OnSubmit += UIGuildList_OnSubmit;
        }

        private bool UIGuildList_OnSubmit(string inputText, out string tips)
        {
            tips = "";
            int guildId = 0;
            if(!int.TryParse(inputText, out guildId))
            {
                tips = "请输入公会ID";
                return false;
            }
            GuildService.Instance.SendGuildSearchRequest(guildId);
            return true;
        }
    }
}