using GameServer.Entities;
using GameServer.Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class MonsterManager
    {
        Map map;

        public Dictionary<int, Monster> monsters = new Dictionary<int, Monster>();

        public void Init(Map map)
        {
            this.map = map;
        }

        internal Monster Create(int monsterId, int Level, NVector3 position, NVector3 rotation)
        {
            Monster monster = new Monster(monsterId, Level, position, rotation);
            EntityManager.Instance.AddEntity(map.ID, monster);
            monster.Info.Id = monster.entityId;
            monster.Info.mapId = map.ID;
            monsters[monster.Id] = monster;

            this.map.MonsterEnter(monster);
            return monster;
        }
    }
}
