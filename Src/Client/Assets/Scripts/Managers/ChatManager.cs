using Assets.Scripts.Models;
using Candlelight.UI;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class ChatManager : Singleton<ChatManager>
    {
        public CHAT_CHANNEL sendChannel;
        public Action OnChat {  get; set; }

        public int PrivateID;

        public string PrivateName;

        public enum CHAT_CHANNEL
        {
            ALL = 0,
            LOCAL = 1,
            WORLD = 2,
            TEAM = 3,
            GUILD = 4,
            PRIVATE = 5,
        }

        public CHAT_CHANNEL displayChannel;

        private ChatChannel[] ChannelFilter = new ChatChannel[6]
        {
            ChatChannel.Local | ChatChannel.World | ChatChannel.Team | ChatChannel.Guild | ChatChannel.Private,
            ChatChannel.Local,
            ChatChannel.World,
            ChatChannel.Team,
            ChatChannel.Guild,
            ChatChannel.Private
        };

        public List<ChatMessage> messages = new List<ChatMessage>();

        public string GetCurrentMessages()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var message in messages)
            {
                sb.AppendLine(FormMessage(message));
            }
            return sb.ToString();
        }

        private string FormMessage(ChatMessage message)
        {
            switch (message.Channel)
            {
                case ChatChannel.Local:
                    return string.Format("[本地]{0}[1}", FormatFromPlayer(message), message.Message);
                case ChatChannel.World:
                    return string.Format("<color = cyan>[世界]{0}[1}<color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.System:
                    return string.Format("<color = yellow>[系统]{0}<color>", message.Message);
                case ChatChannel.Private:
                    return string.Format("<color = magenta>[私聊]{0}[1}<color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.Team:
                    return string.Format("<color = green>[队伍]{0}[1}<color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.Guild:
                    return string.Format("<color = blue>[公会]{0}[1}<color>", FormatFromPlayer(message), message.Message);
                default:
                    break;
            }
            return "";
        }

        private string FormatFromPlayer(ChatMessage message)
        {
            if (message.FromId == User.Instance.currentCharacter.Id)
            {
                return "<a name = \"\" class = \"player\">[我]</a>";
            }
            else
                return string.Format("<a name = \"c:{0} : {1}\" class = \"player\">[{1}]</a>", message.FromId, message.FromName);
        }

        public void StartPrivateChat(int targetId, string targetName)
        {
            this.PrivateID = targetId;
            this.PrivateName = targetName;

            this.sendChannel = CHAT_CHANNEL.PRIVATE;
            if (OnChat != null)
                OnChat();
        }

        public void SendChat(string text)
        {
            
        }

        public bool SetSendChannel(CHAT_CHANNEL channel)
        {
            if(channel == CHAT_CHANNEL.TEAM)
            {
                if(User.Instance.TeamInfo == null)
                {
                    this.AddSystemMessage("你没有加入任何队伍");
                    return false;
                }
            }
            if(channel == CHAT_CHANNEL.GUILD)
            {
                if(User.Instance.currentCharacter.Guild == null)
                {
                    this.AddSystemMessage("你没有加入任何公会");
                    return false;
                }
            }
            this.sendChannel = channel;
            return true;
        }

        public void AddSystemMessage(string message, string from = "")
        {
            this.messages.Add(new ChatMessage()
            {
                Channel = ChatChannel.System,
                Message = message,
                FromName = from
            });
            if (this.OnChat != null)
                OnChat();
        }
    }
}