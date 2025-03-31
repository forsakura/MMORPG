using Assets.Scripts.Managers;
using Candlelight.UI;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Chat
{
    public class UIChat : MonoBehaviour
    {
        public InputField input;
        public HyperText textArea;
        public Text chatTarget;
        public Dropdown channelSelect;
        public TabView.TabView channelTab;
        // Use this for initialization
        void Start()
        {
            this.channelTab.OnTabSelected += OnDisplayChannelSelected;
            ChatManager.Instance.OnChat += RefreshUI;
        }

        private void OnDestroy()
        {
            ChatManager.Instance.OnChat -= RefreshUI;
        }

        // Update is called once per frame
        void Update()
        {
            InputManager.Instance.isInputMode = input.isFocused;
        }

        void OnDisplayChannelSelected(int index)
        {
            ChatManager.Instance.displayChannel = (ChatManager.CHAT_CHANNEL)index;
            RefreshUI();
        }

        public void RefreshUI()
        {
            this.textArea.text = ChatManager.Instance.GetCurrentMessages();
            this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
            if(ChatManager.Instance.sendChannel == ChatManager.CHAT_CHANNEL.PRIVATE)
            {
                this.chatTarget.gameObject.SetActive(true);
                if(ChatManager.Instance.PrivateID != 0)
                {
                    this.chatTarget.text = ChatManager.Instance.PrivateName + ":";
                }
                else
                {
                    this.chatTarget.text = "<无>:";
                }
            }
            else
            {
                this.chatTarget.gameObject.SetActive(false);
            }
        }

        public void OnClickChatLink(HyperText text, HyperText.LinkInfo link)
        {
            if (string.IsNullOrEmpty(link.Name))
                return;
            if(link.Name.StartsWith("c:"))
            {
                string[] strs = link.Name.Split(":".ToCharArray());
                UIChatPopMenu menu = UIManager.Instance.Show<UIChatPopMenu>();
                menu.targteId = int.Parse(strs[1]);
                menu.targetName = strs[2];
            }
        }

        public void OnClickSend()
        {

        }

        public void OnEndInput(string text)
        {
            if (!string.IsNullOrEmpty(text.Trim()))
            {
                this.SendChat(text);
            }
            this.textArea.text = "";
        }

        public void SendChat(string text)
        {
            ChatManager.Instance.SendChat(text);
        }

        public void OnSendChannelChanged(int index)
        {
            if (ChatManager.Instance.sendChannel == (ChatManager.CHAT_CHANNEL)(index + 1))
            {
                return;
            }
            if(!ChatManager.Instance.SetSendChannel((ChatManager.CHAT_CHANNEL)(index +1)))
            {
                this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
            }
            else
                this.RefreshUI();
        }
    }
}