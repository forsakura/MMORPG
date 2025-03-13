using Common.Data;
using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    public class SpawnManager
    {
        Map map;

        List<Spawner> rules = new List<Spawner>();    

        public void Init(Map map)
        {
            this.map = map;
            if (DataManager.Instance.SpawnRules.ContainsKey(map.Define.ID))
            {
                foreach (var define in DataManager.Instance.SpawnRules[map.Define.ID].Values)
                {
                    this.rules.Add(new Spawner(define, map));
                }
            }
        }

        public void Update()
        {
            if (this.rules.Count == 0)
                return;
            for(int i = 0; i < this.rules.Count; i++)
            {
                this.rules[i].Update();
            }
        }
    }
}
