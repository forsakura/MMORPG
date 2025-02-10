using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class MapService : Singleton<MapService>
    {
        public MapService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(OnMapEntitySync);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapTeleportRequest>(OnMapTeleport);
        }

        public void Init()
        {
            MapManager.Instance.Init();
        }

        internal void SendEntityUpdate(NetConnection<NetSession> connection, NEntitySync entitySync)
        {
            connection.Session.Response.mapEntitySync = new MapEntitySyncResponse();
            connection.Session.Response.mapEntitySync.entitySyncs.Add(entitySync);
            connection.SendResponse();
        }

        private void OnMapEntitySync(NetConnection<NetSession> sender, MapEntitySyncRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("MapEntitySync:: characterID: {0}:{1} Entity.ID:{2} Evt:{3} Entity:{4}", character.Id, character.Info.Name, message.entitySync.Id, message.entitySync.Event, message.entitySync.Entity);
            MapManager.Instance[character.Info.mapId].UpdateEntity(message.entitySync);
        }

        private void OnMapTeleport(NetConnection<NetSession> sender, MapTeleportRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnMapTeleport: CharacterID:{0}:{1} TeleportID:{2}", character.Id, character.Data, message.teleporterId);

            if (!DataManager.Instance.Teleporters.ContainsKey(message.teleporterId))
            {
                Log.WarningFormat("Source TeleporterID:{0} not existed", message.teleporterId);
                return;
            }
            TeleporterDefine teleporterDefine = DataManager.Instance.Teleporters[message.teleporterId];
            if (teleporterDefine.LinkTo == 0 || !DataManager.Instance.Teleporters.ContainsKey(teleporterDefine.LinkTo))
            {
                Log.WarningFormat("Source TeleporterID [{0}] LinkTo [{1}] not existed", message.teleporterId, teleporterDefine.LinkTo);
            }

            TeleporterDefine target =  DataManager.Instance.Teleporters[teleporterDefine.LinkTo];

            MapManager.Instance[teleporterDefine.MapID].CharacterLeave(character);
            character.Position = target.Position;
            character.Direction = target.Direction;
            MapManager.Instance[target.MapID].CharacterEnter(sender, character);
        }
    }
}
