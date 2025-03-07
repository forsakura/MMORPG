using Assets.Scripts.Managers;
using Assets.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Quest
{
    public class UIQuestSystem : UIWindow
    {
        public GameObject questObject;
        public ListView.ListView mainList;
        public ListView.ListView branchList;
        public Text title;
        public TabView.TabView tabs;
        public UIQuestInfo questInfo;
        // Use this for initialization
        void Start()
        {
            mainList.onItemSelected += OnQuestItemSelected;
            branchList.onItemSelected += OnQuestItemSelected;
            tabs.OnTabSelected += OnSelectTab;
            RefreshUI();
        }

        private void RefreshUI()
        {
            ClearAllQuestItems();
            InitAllQuestItems();
        }

        private void InitAllQuestItems()
        {
            foreach (var kv in QuestManager.Instance.allQuests)
            {
                if(showAvailableList)
                {
                    if (kv.Value.Info != null) continue;
                }
                else
                {
                    if(kv.Value.Info == null) continue;
                }

                GameObject go = Instantiate(questObject, kv.Value.Define.Type == Common.Data.QuestType.MAIN ? mainList.gameObject.transform : branchList.gameObject.transform);
                UIQuestItem ui = go.GetComponent<UIQuestItem>();
                ui.SetQuestInfo(kv.Value);
                if (kv.Value.Define.Type == Common.Data.QuestType.MAIN)
                    mainList.AddItem(ui as ListView.ListView.ListViewItem);
                else
                    branchList.AddItem(ui as ListView.ListView.ListViewItem);
            }
        }

        private void ClearAllQuestItems()
        {
            mainList.RemoveAll();
            branchList.RemoveAll();
        }

        bool showAvailableList = false;
        private void OnSelectTab(int arg0)
        {
            showAvailableList = arg0 == 1;
            RefreshUI();
        }

        private void OnQuestItemSelected(ListView.ListView.ListViewItem arg0)
        {
            UIQuestItem questItem = (UIQuestItem)arg0;
            questInfo.SetQuestInfo(questItem.quest);
        }
    }
}