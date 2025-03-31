using Assets.Scripts.Managers;
using Network;
using SkillBridge.Message;
using System;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class ChatService : Singleton<ChatService>, IDisposable
    {
        public void Init()
        {
            MessageDistributer.Instance.Subscribe<ChatResponse>(OnChat);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ChatResponse>(OnChat);
        }

        internal void SendChatMessage(ChatChannel sendChannel, string text, string privateName, int privateID)
        {
            Debug.LogFormat("SendChatMessage:: targetId: {0} targetName: {1}", privateID, privateName);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.Chat = new ChatRequest();
            message.Request.Chat.Message = new ChatMessage();
            message.Request.Chat.Message.Channel = sendChannel;
            message.Request.Chat.Message.Message = text;
            message.Request.Chat.Message.ToId = privateID;
            message.Request.Chat.Message.ToName = privateName;
            NetClient.Instance.SendMessage(message);
        }

        private void OnChat(object sender, ChatResponse message)
        {
            Debug.LogFormat("OnChat:: Result: {0}", message.Result);
            if(message.Result == Result.Success)
            {
                ChatManager.Instance.AddMessages(ChatChannel.World, message.WorldMessages);
                ChatManager.Instance.AddMessages(ChatChannel.Local, message.LocalMessages);
                ChatManager.Instance.AddMessages(ChatChannel.System, message.SystemMessages);
                ChatManager.Instance.AddMessages(ChatChannel.Private, message.PrivateMessages);
                ChatManager.Instance.AddMessages(ChatChannel.Team, message.TeamMessages);
                ChatManager.Instance.AddMessages(ChatChannel.Guild, message.GuildMessages);
            }
            else
            {
                ChatManager.Instance.AddSystemMessage(message.Errormsg);
            }
        }
    }
}