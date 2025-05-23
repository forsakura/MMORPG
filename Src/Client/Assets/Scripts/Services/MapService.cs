﻿using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Sound;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class MapService : Singleton<MapService>, IDisposable
    {
        public MapService() 
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(OnMapEntitySync);
        }


        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(OnMapCharacterLeave);
            MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(OnMapEntitySync);
        }

        public int currentMapId { get; set; }

        public void Init()
        {

        }
        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse message)
        {
            Debug.LogFormat("OnMapCharacterEnter:Map:{0} Count:{1}", message.mapId, message.Characters.Count);
            foreach (var cha in message.Characters)
            {
                if(User.Instance.currentCharacter == null || (cha.Type == CharacterType.Player && User.Instance.currentCharacter.Id == cha.Id))
                {
                    User.Instance.currentCharacter = cha;
                }
                CharacterManager.Instance.AddCharacter(cha);
            }
            if (currentMapId != message.mapId)
            {
                EnterMap(message.mapId);
                currentMapId = message.mapId;
            }
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse message)
        {
            Debug.LogFormat("OnMapCharacterLeave::EntityID:{0}", message.entityId);
            if (message.entityId != User.Instance.currentCharacter.EntityId)
            {
                CharacterManager.Instance.RemoveCharacter(message.entityId);
            }
            else CharacterManager.Instance.Clear();
        }

        private void EnterMap(int mapId)
        {
            if(DataManager.Instance.Maps.ContainsKey(mapId))
            {
                var map = DataManager.Instance.Maps[mapId];
                User.Instance.curentMiniMap = map;
                SceneManager.Instance.LoadScene(map.Resource);
                SoundManager.Instance.PlayMusic(map.Music);
            }
            else Debug.LogErrorFormat("EnterMap: Map {0} not existed", mapId);
        }

        internal void SendMapEntitySync(EntityEvent entityEvent, NEntity entityData, int param)
        {
            Debug.LogFormat("MapEntitySyncRequest::ID:{0} POS: {1} DIR: {2} SPD: {3}", entityData.Id, entityData.Position, entityData.Direction, entityData.Speed);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync()
            {
                Id = entityData.Id,
                Event = entityEvent,
                Entity = entityData,
                Param = param
            };
            NetClient.Instance.SendMessage(message);
        }

        private void OnMapEntitySync(object sender, MapEntitySyncResponse message)
        {
            foreach (var item in message.entitySyncs)
            {
                EntityManager.Instance.OnEntitySync(item);
            }
        }

        internal void SendMapTeleport(int iD)
        {
            Debug.LogFormat("MapTeleportRequest::TeleporterID:{0}", iD);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapTeleport = new MapTeleportRequest();
            message.Request.mapTeleport.teleporterId = iD;
            NetClient.Instance.SendMessage(message);
        }
    }
}