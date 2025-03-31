using GameServer.Entities;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    public class Chat
    {
        Character owner;

        public int localIndex;
        public int worldIndex;
        public int systemIndex;
        public int teamIndex;
        public int guildIndex;

        public Chat(Character owner)
        {
            this.owner = owner;
        }

        public void PostProcess(NetMessageResponse message)
        {
            if (message.Chat == null)
            {
                message = new NetMessageResponse();
                message.Chat = new ChatResponse();
            }
            this.localIndex = ChatManager.Instance.GetLocalMessages(owner.Info.mapId, localIndex, message.Chat.LocalMessages);
            this.worldIndex = ChatManager.Instance.GetWorldMessages(worldIndex, message.Chat.WorldMessages);
            this.systemIndex = ChatManager.Instance.GetSystemMessages(systemIndex, message.Chat.SystemMessages);
            if (this.owner.team != null)
            {
                this.teamIndex = ChatManager.Instance.GetTeamMessages(owner.team.Id, teamIndex, message.Chat.TeamMessages);
            }
            if (this.owner.Guild != null)
            {
                this.guildIndex = ChatManager.Instance.GetGuildMessages(owner.Guild.Id, guildIndex, message.Chat.GuildMessages);
            }
        }
    }
}
