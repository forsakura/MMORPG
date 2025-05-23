﻿using Common;
using Common.Data;
using Common.Utils;
using GameServer.Core;
using GameServer.Managers;
using GameServer.Models;
using Network;
using SkillBridge.Message;

namespace GameServer.Entities
{
    /// <summary>
    /// Character
    /// 玩家角色类
    /// </summary>
    public class Character : CharacterBase, IPostResponser
    {
        public TCharacter Data;


        public ItemManager ItemManager;
        public StatusManager StatusManager;
        public QuestManager QuestManager;
        public FriendManager FriendManager;

        public Team team;
        public double TeamUpdateTS;

        public Guild Guild;
        public double GuildUpdateTS;

        public Chat Chat;

        public Character(CharacterType type,TCharacter cha):
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ),new Core.Vector3Int(100,0,0))
        {
            this.Data = cha;
            this.Id = cha.ID;
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID; 
            this.Info.EntityId = this.entityId;
            this.Info.Name = cha.Name;
            this.Info.Level = cha.Level;
            this.Info.ConfigId = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Gold = cha.Gold;
            this.Info.Ride = 0;
            this.Info.Entity = this.EntityData;
            this.Define = DataManager.Instance.Characters[this.Info.ConfigId];

            this.ItemManager = new ItemManager(this);
            ItemManager.GetItemsInfo(Info.Items);
            StatusManager = new StatusManager(this);
            Info.Bag = new NBagInfo();
            Info.Bag.Unlocked = this.Data.Bag.Unlocked;
            Info.Bag.Items = this.Data.Bag.Items;
            Info.Equips = this.Data.Equips;
            this.QuestManager = new QuestManager(this);
            this.QuestManager.GetQuestInfos(this.Info.Quests);
            this.FriendManager = new FriendManager(this);
            this.FriendManager.GetFriendInfos(this.Info.Friends);

            this.Guild = GuildManager.Instance.GetGuild(this.Data.GuildId);

            this.Chat = new Chat(this);
        }

        public long Gold
        {
            get { return this.Data.Gold; }
            set
            {
                if (this.Data.Gold == value)
                    return;
                StatusManager.AddGoldChange((int)(value - Data.Gold));
                this.Data.Gold = value;
            }
        }

        public int Ride
        {
            get
            {
                return Info.Ride;
            }
            set
            {
                if(this.Info.Ride == value)
                    return;
                this.Info.Ride = value;
            }
        }

        public void PostProcess(NetMessageResponse message)
        {
            Log.InfoFormat("PostProcess : characterId: {0} : {1}", this.Id, this.Info.Name);
            this.FriendManager.PostProcess(message);
            if(this.StatusManager.HasStatus)
            {
                this.StatusManager.PostProcess(message);
            }

            if (this.team != null)
            {
                Log.InfoFormat("PostProcess > Team: CharacterID:{0}:{1} {2}<{3}", this.Id, this.Info.Name, TeamUpdateTS, this.team.timestamp);
                if (TeamUpdateTS < this.team.timestamp)
                {
                    TeamUpdateTS = this.team.timestamp;
                    this.team.PostProcess(message);
                }
            }

            if (this.Guild != null)
            {
                Log.InfoFormat("PostProcess > Guild: CharacterID:{0}:{1} {2}<{3}", this.Id, this.Info.Name, GuildUpdateTS, this.Guild.timestamp);
                if(this.Info.Guild == null)
                {
                    this.Info.Guild = this.Guild.GuildInfo(this);
                    if (message.mapCharacterEnter != null)
                        GuildUpdateTS = Guild.timestamp;
                }
                if (GuildUpdateTS < this.Guild.timestamp && message.mapCharacterEnter == null)
                {
                    GuildUpdateTS = this.Guild.timestamp;
                    this.Guild.PostProcess(this, message);
                }
            }
            Chat.PostProcess(message);
        }

        /// <summary>
        /// 角色离开时调用
        /// </summary>
        public void Clear()
        {
            this.FriendManager.OfflineNotify();
        }

        public NCharacterInfo GetBasicInfo()
        {
            return new NCharacterInfo()
            {
                Id = this.Id,
                Name = this.Info.Name,
                Class = this.Info.Class,
                Level = this.Info.Level
            };
        }
    }
}
