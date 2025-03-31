using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;

namespace GameServer.Services
{
    public class ChatService : Singleton<ChatService>
    {
        public void Init()
        {
            ChatManager.Instance.Init();
        }
        public ChatService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ChatRequest>(OnChat);
        }

        private void OnChat(NetConnection<NetSession> sender, ChatRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnChat:: character: {0} Channel: {1} Message: {2}", character.Id, message.Message.Channel, message.Message.Message);
            if(message.Message.Channel == ChatChannel.Private)
            {
                var chatTo = SessionManager.Instance.GetSession(message.Message.ToId);
                if (chatTo == null)
                {
                    sender.Session.Response.Chat = new ChatResponse();
                    sender.Session.Response.Chat.Result = Result.Failed;
                    sender.Session.Response.Chat.Errormsg = "对方不在线";
                    sender.Session.Response.Chat.PrivateMessages.Add(message.Message);
                    sender.SendResponse();
                }
                else
                {
                    message.Message.FromId = character.Id;
                    message.Message.FromName = character.Name;
                    if(chatTo.Session.Response.Chat == null)
                    {
                        chatTo.Session.Response.Chat = new ChatResponse();
                    }
                    chatTo.Session.Response.Chat.Result = Result.Success;
                    chatTo.Session.Response.Chat.PrivateMessages.Add(message.Message);
                    chatTo.SendResponse();

                    if(sender.Session.Response.Chat == null)
                    {
                        sender.Session.Response.Chat = new ChatResponse();
                    }
                    sender.Session.Response.Chat.Result = Result.Success;
                    sender.Session.Response.Chat.PrivateMessages.Add(message.Message);
                    sender.SendResponse();
                }
            }
            else
            {
                sender.Session.Response.Chat = new ChatResponse();
                sender.Session.Response.Chat.Result = Result.Success;
                ChatManager.Instance.AddMessage(character, message.Message);
                sender.SendResponse();
            }
        }
    }
}
