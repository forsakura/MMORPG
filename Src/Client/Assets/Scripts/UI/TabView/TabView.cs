using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Assets.Scripts.UI.TabView
{
    public class TabView : MonoBehaviour
    {
        public GameObject[] bagViews;
        public TabButton[] btnPages;
        public int index = -1;
        IEnumerator Start()
        {
            for (int i = 0; i < btnPages.Length; i++)
            {
                btnPages[i].bagView = this;
                btnPages[i].tabIndex = i;
            }
            yield return new WaitForEndOfFrame();
            SelectTab(0);
        }
        internal void SelectTab(int tabIndex)
        {
            if (index != tabIndex)
            {
                for (int i = 0; i < btnPages.Length; i++)
                {
                    btnPages[i].Selected(i == tabIndex);
                    bagViews[i].SetActive(i == tabIndex);
                }
            }
            index = tabIndex;
        }
    }
}