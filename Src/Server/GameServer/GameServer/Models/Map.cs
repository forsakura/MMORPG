using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

using Common;
using Common.Data;

using Network;
using GameServer.Managers;
using GameServer.Entities;
using GameServer.Services;

namespace GameServer.Models
{
    class Map
    {
        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        public int ID
        {
            get { return this.Define.ID; }
        }
        internal MapDefine Define;

        /// <summary>
        /// 地图中的角色，以CharacterID为Key
        /// </summary>
        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();


        /// <summary>
        /// 刷怪管理器
        /// </summary>
        //SpawnManager SpawnManager = new SpawnManager();

        //public MonsterManager MonsterManager = new MonsterManager();

        internal Map(MapDefine define)
        {
            this.Define = define;
            //this.SpawnManager.Init(this);
            //this.MonsterManager.Init(this);
        }

        internal void Update()
        {
            //SpawnManager.Update();
        }

        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterEnter::characterID:{0} MapID:{1}", character.Id, Define.ID);
            character.Info.mapId = this.ID;

            conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            conn.Session.Response.mapCharacterEnter.Characters.Add(character.Info);
            foreach (var item in MapCharacters)
            {
                conn.Session.Response.mapCharacterEnter.Characters.Add(item.Value.character.Info);
                if (item.Value.character != character)
                    this.AddCharacterEnterMap(item.Value.connection, character.Info);
            }
            MapCharacters[character.EntityData.Id] = new MapCharacter(conn, character);

            conn.SendResponse();
        }

        void AddCharacterEnterMap(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            if (conn.Session.Response.mapCharacterEnter == null)
            {
                conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
                conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            }
            conn.Session.Response.mapCharacterEnter.Characters.Add(character);
            conn.SendResponse();
        }


        internal void CharacterLeave(Character cha)
        {
            Log.InfoFormat("CharacterLeave: Map:{0} characterId:{1}", this.Define.ID, cha.EntityData.Id);
            foreach (var kv in this.MapCharacters)
            {
                this.SendCharacterLeaveMap(kv.Value.connection, cha);
            }
            this.MapCharacters.Remove(cha.EntityData.Id);
        }

        void SendCharacterLeaveMap(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("SendCharacterLeaveMap To {0}:{1} : Map:{2} Character:{3}:{4}", conn.Session.Character.EntityData.Id, conn.Session.Character.Info.Name, this.Define.ID, character.Id, character.Name);
            conn.Session.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            conn.Session.Response.mapCharacterLeave.entityId = character.EntityData.Id;
            conn.SendResponse();
        }


        /// <summary>
        /// 怪物进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void MonsterEnter(Monster monster)
        {
            Log.InfoFormat("MonsterEnter: Map:{0} monsterId:{1}", this.Define.ID, monster.Id);
            foreach (var kv in this.MapCharacters)
            {
                this.AddCharacterEnterMap(kv.Value.connection, monster.Info);
            }
        }

        internal void UpdateEntity(NEntitySync entitySync)
        {
            foreach (var kv in this.MapCharacters)
            {
                if (kv.Value.character.EntityData.Id == entitySync.Entity.Id)
                {
                    kv.Value.character.Position = entitySync.Entity.Position;
                    kv.Value.character.Direction = entitySync.Entity.Direction;
                    kv.Value.character.Speed = entitySync.Entity.Speed;
                }
                else MapService.Instance.SendEntityUpdate(kv.Value.connection, entitySync);
            }
        }
    }
}
