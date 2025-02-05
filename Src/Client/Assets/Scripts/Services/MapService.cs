using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class MapService : Singleton<MapService>
    {
        public MapService() 
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(OnMapCharacterLeave);
        }

        ~MapService()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(OnMapCharacterLeave);
        }

        public int currentMapId { get; private set; }

        public void Init()
        {

        }
        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse message)
        {
            Debug.LogFormat("OnMapCharacterEnter:Map:{0} Count:{1}", message.mapId, message.Characters.Count);
            foreach (var cha in message.Characters)
            {
                if(User.Instance.currentCharacter.Id == cha.Id)
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
            Debug.LogFormat("OnMapCharacterLeave::Map:{0} Count{1}");
        }

        private void EnterMap(int mapId)
        {
            if(DataManager.Instance.Maps.ContainsKey(mapId))
            {
                var map = DataManager.Instance.Maps[mapId];
                User.Instance.curentMiniMap = map;
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else Debug.LogErrorFormat("EnterMap: Map {0} not existed", mapId);
        }

        internal void SendMapEntitySync(EntityEvent entityEvent, NEntity entityData, int param)
        {
            Debug.LogFormat("MapEntitySync::");
        }
    }
}