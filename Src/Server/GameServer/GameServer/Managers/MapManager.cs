using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Models;
using GameServer.Services;

namespace GameServer.Managers
{
    class MapManager : Singleton<MapManager>
    {
        Dictionary<int, Map> Maps = new Dictionary<int, Map>();

        public void Init()
        {
            foreach (var val in DataManager.Instance.Maps.Values)
            {
                Map map = new Map(val);
                Log.InfoFormat("MapManager.Init > MapID:{0} MapName:{1}", map.Define.ID, map.Define.Name);
                Maps[val.ID] = map;
            }
        }

        public Map this[int key]
        {
            get
            {
                return Maps[key];
            }
        }

        public void Updata()
        {
            foreach(var val in Maps.Values)
            {
                val.Update();
            }
        }
        /*Dictionary<int, Map> Maps = new Dictionary<int, Map>();

        public void Init()
        {
            foreach (var mapdefine in DataManager.Instance.Maps.Values)
            {
                Map map = new Map(mapdefine);
                Log.InfoFormat("MapManager.Init > Map:{0}:{1}", map.Define.ID, map.Define.Name);
                this.Maps[mapdefine.ID] = map;
            }
        }



        public Map this[int key]
        {
            get
            {
                return this.Maps[key];
            }
        }


        public void Update()
        {
            foreach(var map in this.Maps.Values)
            {
                map.Update();
            }
        }*/
    }
}
