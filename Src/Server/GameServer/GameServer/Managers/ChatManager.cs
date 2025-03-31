using Common;
using Common.Utils;
using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;

namespace GameServer.Managers
{
    public class ChatManager : Singleton<ChatManager>
    {
        public List<ChatMessage> WorldMessages = new List<ChatMessage>();
        public List<ChatMessage> SystemMessages = new List<ChatMessage>();
        public Dictionary<int, List<ChatMessage>> LocalMessages = new Dictionary<int, List<ChatMessage>>();
        public Dictionary<int, List<ChatMessage>> GuildMessages = new Dictionary<int, List<ChatMessage>>();
        public Dictionary<int, List<ChatMessage>> TeamMessages = new Dictionary<int, List<ChatMessage>>();

        public void Init()
        {

        }

        public void AddMessage(Character from, ChatMessage message)
        {
            message.FromId = from.Id;
            message.FromName = from.Name;
            message.Time = TimeUtil.timestamp;
            switch (message.Channel)
            {
                case ChatChannel.Local:
                    this.AddLocalMessage(from.Info.mapId, message);
                    break;
                case ChatChannel.World:
                    this.AddWorldMessage(message);
                    break;
                case ChatChannel.System:
                    this.AddSystemMessage(message);
                    break;
                case ChatChannel.Team:
                    this.AddTeamMessage(from.team.Id, message);
                    break;
                case ChatChannel.Guild:
                    this.AddGuildMessage(from.Guild.Id, message);
                    break;
                default:
                    break;
            }
        }

        private void AddLocalMessage(int mapId, ChatMessage message)
        {
            if (LocalMessages.TryGetValue(mapId, out List<ChatMessage> messages))
            {
                messages.Add(message);
            }
            else
            {
                LocalMessages[mapId] = new List<ChatMessage>() { message };
            }
        }

        private void AddWorldMessage(ChatMessage message)
        {
            WorldMessages.Add(message);
        }

        private void AddSystemMessage(ChatMessage message)
        {
            SystemMessages.Add(message);
        }

        private void AddTeamMessage(int id, ChatMessage message)
        {
            if(TeamMessages.TryGetValue(id, out List<ChatMessage> messages))
            {
                messages.Add(message);
            }
            else
            {
                TeamMessages[id] = new List<ChatMessage>() { message };
            }
        }

        private void AddGuildMessage(int id, ChatMessage message)
        {
            if (GuildMessages.TryGetValue(id, out List<ChatMessage> messages))
            {
                messages.Add(message);
            }
            else
            {
                GuildMessages[id] = new List<ChatMessage> { message };
            }
        }

        public int GetLocalMessages(int mapID, int index, List<ChatMessage> result)
        {
            if(!this.LocalMessages.TryGetValue(mapID, out List<ChatMessage> messages))
            {
                return 0;
            }
            return GetNewMessages(index, result, messages);
        }

        public int GetWorldMessages(int index, List<ChatMessage> result)
        {
            return GetNewMessages(index, result, this.WorldMessages);
        }

        public int GetSystemMessages(int index, List<ChatMessage> result)
        {
            return GetNewMessages(index, result, this.SystemMessages);
        }

        public int GetTeamMessages(int teamId, int index, List<ChatMessage> result)
        {
            if (!TeamMessages.TryGetValue(teamId, out List<ChatMessage> messages))
            {
                return 0;
            }
            return GetNewMessages(index, result, messages);
        }

        public int GetGuildMessages(int guildId, int index, List<ChatMessage> result)
        {
            if (!GuildMessages.TryGetValue(index, out List<ChatMessage> messages))
            {
                return 0;
            }
            return GetNewMessages(index, result, messages);
        }

        int GetNewMessages(int index, List<ChatMessage> result, List<ChatMessage> messages)
        {
            if(index == 0)
            {
                if(messages.Count > GameDefine.MaxChatRecordNums)
                {
                    index = messages.Count - GameDefine.MaxChatRecordNums;
                }
            }

            for(; index < messages.Count; index++)
            {
                result.Add(messages[index]);
            }
            return index;
        }
    }
}
