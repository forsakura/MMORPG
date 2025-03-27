using Common;
using Common.Utils;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    internal class GuildManager : Singleton<GuildManager>
    {
        int createCost = 5000;
        public HashSet<string> GuildNames = new HashSet<string>();
        public Dictionary<int, Guild> IDGuilds = new Dictionary<int, Guild>();

        public void Init()
        {
            IDGuilds.Clear();
            foreach (var item in DBService.Instance.Entities.Guilds)
            {
                this.AddGuild(new Guild(item));
            }
        }

        void AddGuild(Guild guild)
        {
            this.IDGuilds.Add(guild.Id, guild);
            GuildNames.Add(guild.Name);
            guild.timestamp = TimeUtil.timestamp;
        }

        public bool CheckNameExisted(string guildName)
        {
            return GuildNames.Contains(guildName);
        }

        internal bool CreateGuild(string guildName, string guildNotice, Character character)
        {
            if(character.Gold < createCost)
                return false;
            character.Gold -= createCost;
            DateTime now = DateTime.Now;
            TGuild dbGuild = DBService.Instance.Entities.Guilds.Create();
            dbGuild.Name = guildName;
            dbGuild.Notice = guildNotice;
            dbGuild.LeaderID = character.Id;
            dbGuild.LeaderName = character.Name;
            dbGuild.CreateTime = now;
            DBService.Instance.Entities.Guilds.Add(dbGuild);

            Guild guild = new Guild(dbGuild);
            guild.AddMember(character.Id, character.Name, character.Data.Level, character.Data.Class, GuildTitle.President);
            character.Guild = guild;
            DBService.Instance.Save();
            character.Data.GuildId = dbGuild.Id;
            DBService.Instance.Save();
            this.AddGuild(guild);
            return true;
        }

        internal Guild GetGuild(int guildId)
        {
            Guild guild = null;
            IDGuilds.TryGetValue(guildId, out guild);
            return guild;
        }

        internal List<NGuildInfo> GetGuildInfos()
        {
            List<NGuildInfo> nGuildInfos = new List<NGuildInfo>();
            foreach (var item in this.IDGuilds)
            {
                nGuildInfos.Add(item.Value.GuildInfo(null));
            }
            return nGuildInfos;
        }
    }
}
