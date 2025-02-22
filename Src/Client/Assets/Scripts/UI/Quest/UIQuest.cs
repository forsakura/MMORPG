using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Quest
{
    public class UIQuest : UIWindow
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

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}