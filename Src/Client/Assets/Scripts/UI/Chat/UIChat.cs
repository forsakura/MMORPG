using Assets.Scripts.Managers;
using Candlelight.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Chat
{
    public class UIChat : MonoBehaviour
    {
        public InputField input;
        public HyperText hyperText;
        public Text chatTarget;
        public Dropdown channelSelect;
        public TabView.TabView channelTab;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            InputManager.Instance.isInputMode = input.isFocused;
        }
    }
}