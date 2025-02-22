using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Quest
{
    public class UIQuestItem : MonoBehaviour
    {
        public Image backGround;
        public Text title;
        public Sprite selectedSprite;
        public Sprite normalSprite;

        bool selected;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                backGround.overrideSprite = selected ? selectedSprite : normalSprite;
            }
        }

        public QuestDefine questDefine;
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