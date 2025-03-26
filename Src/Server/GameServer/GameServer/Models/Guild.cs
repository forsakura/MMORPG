using Common;
using Common.Utils;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    public class Guild
    {
        public TGuild Data;
        public Character Leader;
        public List<NGuildMember> members = new List<NGuildMember>();
        public double timestamp;
        public int Id { get { return this.Data.Id; } }
        public string Name { get { return this.Data.Name; } }

        public Guild(TGuild guild)
        {
            this.Data = guild;
        }

        public void AddMember(int characterId, string name, int level, int @class, GuildTitle title)
        {
            DateTime now = DateTime.Now;
            TGuildMember dbMember = new TGuildMember();
            dbMember.CharacterId = characterId;
            dbMember.Name = name;
            dbMember.Level = level;
            dbMember.Class = @class;
            dbMember.Title = (int)title;
            dbMember.JoinTime = now;
            dbMember.LastTime = now;
            this.Data.Members.Add(dbMember);
            timestamp = TimeUtil.timestamp;
        }

        public NGuildInfo GuildInfo(Character character)
        {
            NGuildInfo info = new NGuildInfo();
            info.Id = this.Id;
            info.GuildName = this.Name;
            info.Notice = this.Data.Notice;
            info.LeaderId = this.Data.LeaderID;
            info.LeaderName = this.Data.LeaderName;
            info.createTime = (long)TimeUtil.GetTimestamp(this.Data.CreateTime);
            info.memberCount = this.Data.Members.Count;

            if (character != null)
            {
                info.Members.AddRange(GetMemberInfos());
                if (character.Id == this.Data.LeaderID)
                {
                    info.Applies.AddRange(GetApplyInfos());
                }
            }
            return info;
        }

        private List<NGuildApplyInfo> GetApplyInfos()
        {
            List<NGuildApplyInfo> nGuildApplyInfos = new List<NGuildApplyInfo>();
            foreach (var item in this.Data.Applies)
            {
                NGuildApplyInfo info = new NGuildApplyInfo();
                info.Name = item.Name;
                info.characterId = item.CharacterId;
                info.GuildId = item.GuildId;
                info.Level = item.Level;
                info.Class = item.Class;
                info.Result = (ApplyResult)item.Result;
                nGuildApplyInfos.Add(info);
            }
            return nGuildApplyInfos;
        }

        private List<NGuildMember> GetMemberInfos()
        {
            List<NGuildMember> nGuildMembers = new List<NGuildMember>();
            foreach (var item in this.Data.Members)
            {
                NGuildMember member = new NGuildMember();
                member.characterId = item.CharacterId;
                member.Id = item.Id;
                member.joinTime = (long)TimeUtil.GetTimestamp(item.JoinTime);
                member.lastTime = (long)TimeUtil.GetTimestamp(item.LastTime);
                member.Title = (GuildTitle)item.Title;

                var character = CharacterManager.Instance.GetCharacter(item.CharacterId);
                if (character != null)
                {
                    member.Status = 1;
                    member.Info = character.GetBasicInfo();
                    item.Level = character.Data.Level;
                    item.Name = character.Name;
                    item.LastTime = DateTime.Now;
                    if(item.CharacterId == this.Data.LeaderID)
                        this.Leader = character;
                }
                else
                {
                    member.Status = 0;
                    member.Info = this.GetMemberInfo(item);
                    if (item.CharacterId == this.Leader.Id)
                        this.Leader = null;
                }
                nGuildMembers.Add(member);
            }
            return nGuildMembers;
        }

        private NCharacterInfo GetMemberInfo(TGuildMember member)
        {
            return new NCharacterInfo()
            {
                Id = member.Id,
                Name = member.Name,
                Class = (CharacterClass)member.Class,
                Level = member.Level
            };
        }

        internal void PostProcess(Character character, NetMessageResponse message)
        {
            if (message.Guild == null)
            {
                message.Guild = new GuildResponse();
                message.Guild.Result = Result.Success;
                message.Guild.Guild = this.GuildInfo(character);
            }
        }

        internal bool JoinApply(NGuildApplyInfo apply)
        {
            var oldApply = this.Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId);
            if (oldApply != null)
            {
                return false;
            }

            var dbapply = DBService.Instance.Entities.GuildApplies.Create();
            dbapply.Name = apply.Name;
            dbapply.CharacterId = apply.characterId;
            dbapply.Level = apply.Level;
            dbapply.Class = apply.Class;
            dbapply.ApplyTime = DateTime.Now;
            dbapply.GuildId = apply.GuildId;

            DBService.Instance.Entities.GuildApplies.Add(dbapply);
            this.Data.Applies.Add(dbapply);

            DBService.Instance.Save();

            this.timestamp = Time.timestamp;
            return true;
        }

        internal bool JoinAppove(NGuildApplyInfo apply)
        {
            var oldApply = this.Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId && v.Result == 0);
            if(oldApply == null)
            {
                return false;
            }

            oldApply.Result = (int)apply.Result;

            if (apply.Result == ApplyResult.Acept)
                this.AddMember(apply.characterId, apply.Name, apply.Level, apply.Class, GuildTitle.None);

            DBService.Instance.Save();
            this.timestamp = Time.timestamp;
            return true;
        }

        internal void Leave(Character charcater)
        {
            
        }
    }
}
