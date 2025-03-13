using Common;
using Common.Data;
using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    internal class Spawner
    {
        public SpawnRuleDefine SpawnRuleDefine { get; set; }
        private SpawnPointDefine SpawnPointDefine = null;
        private Map map;
        private float spawnTime = 0;
        private float unspawnTime = 0;
        private bool spawned = false;
        public Spawner(SpawnRuleDefine spawnRuleDefine, Map map)
        {
            SpawnRuleDefine = spawnRuleDefine;
            this.map = map;
            if(DataManager.Instance.SpawnPoints.ContainsKey(map.Define.ID))
            {
                if (DataManager.Instance.SpawnPoints[map.Define.ID].ContainsKey(spawnRuleDefine.SpawnPoint))
                    this.SpawnPointDefine = DataManager.Instance.SpawnPoints[map.Define.ID][spawnRuleDefine.SpawnPoint];
                else
                    Log.ErrorFormat("SpawnRule[{0}] SpawnPoint[{1}] not existed", spawnRuleDefine.ID, spawnRuleDefine.SpawnPoint);
            }
        }

        public void Update()
        {
            if (CanSpawn())
                Spawn();
        }

        private bool CanSpawn()
        {
            if (spawned)
                return false;
            if (unspawnTime + SpawnRuleDefine.SpawnPeriod > Time.time)
                return false;

            return true;
        }

        private void Spawn()
        {
            spawned = true;
            Log.InfoFormat("Map[{0}] Spawn[{1}] Mon[{2}] At Point[{3}]", map.Define.ID, SpawnRuleDefine.ID, SpawnRuleDefine.SpawnMonID, SpawnPointDefine.ID);
            map.monsterManager.Create(SpawnRuleDefine.SpawnMonID, SpawnRuleDefine.SpawnLevel, SpawnPointDefine.Position, SpawnPointDefine.Direction);
        }

    }
}
