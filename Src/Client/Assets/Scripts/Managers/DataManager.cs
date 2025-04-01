using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Common.Data;

using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
namespace GameServer.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        public string DataPath;
        public Dictionary<int, MapDefine> Maps = null;
        public Dictionary<int, CharacterDefine> Characters = null;
        public Dictionary<int, TeleporterDefine> Teleporters = null;
		public Dictionary<int, NpcDefine> Npcs = null;
        public Dictionary<int, ItemDefine> Items = null;
        public Dictionary<int, ShopDefine> Shops = null;
        public Dictionary<int, Dictionary<int, ShopItemDefine>> ShopItems = null;
        public Dictionary<int, EquipDefine> Equips = null;
        public Dictionary<int, QuestDefine> Quests = null;
        public Dictionary<int, Dictionary<int, SpawnPointDefine>> SpawnPoints = null;
        public Dictionary<int, Dictionary<int, SpawnRuleDefine>> SpawnRules = null;
        public Dictionary<int, RideDefine> Rides = null;
        public DataManager()
        {
            this.DataPath = "Data/";
            Debug.LogFormat("DataManager > DataManager()");
        }

        public void Load()
        {
            string json = File.ReadAllText(this.DataPath + "MapDefine.txt");
            this.Maps = JsonConvert.DeserializeObject<Dictionary<int, MapDefine>>(json);

            json = File.ReadAllText(this.DataPath + "CharacterDefine.txt");
            this.Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);

            json = File.ReadAllText(this.DataPath + "TeleporterDefine.txt");
            this.Teleporters = JsonConvert.DeserializeObject<Dictionary<int, TeleporterDefine>>(json);

            json = File.ReadAllText(this.DataPath + "NpcDefine.txt");
            this.Npcs = JsonConvert.DeserializeObject<Dictionary<int, NpcDefine>>(json);

            json = File.ReadAllText(this.DataPath + "ItemDefine.txt");
            this.Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);

            json = File.ReadAllText(this.DataPath + "ShopDefine.txt");
            this.Shops = JsonConvert.DeserializeObject<Dictionary<int, ShopDefine>>(json);

            json = File.ReadAllText(this.DataPath + "ShopItemDefine.txt");
            this.ShopItems = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, ShopItemDefine>>>(json);

            json = File.ReadAllText(DataPath + "EquipDefine.txt");
            this.Equips = JsonConvert.DeserializeObject<Dictionary<int, EquipDefine>>(json);

            json = File.ReadAllText(DataPath + "QuestDefine.txt");
            Quests = JsonConvert.DeserializeObject<Dictionary<int, QuestDefine>>(json);

            json = File.ReadAllText(DataPath + "SpawnRuleDefine.txt");
            this.SpawnRules = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SpawnRuleDefine>>>(json);

            json = File.ReadAllText(DataPath + "SpawnPointDefine.txt");
            this.SpawnPoints = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SpawnPointDefine>>>(json);

            json = File.ReadAllText(DataPath + "RideDefine.txt");
            this.Rides = JsonConvert.DeserializeObject<Dictionary<int,  RideDefine>>(json);
        }

        public IEnumerator LoadData()
        {
            string json = File.ReadAllText(this.DataPath + "MapDefine.txt");
            this.Maps = JsonConvert.DeserializeObject<Dictionary<int, MapDefine>>(json);
            
            yield return null;

            json = File.ReadAllText(this.DataPath + "CharacterDefine.txt");
            this.Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);
            
            yield return null;

            json = File.ReadAllText(this.DataPath + "TeleporterDefine.txt");
            this.Teleporters = JsonConvert.DeserializeObject<Dictionary<int, TeleporterDefine>>(json);

            yield return null;

            json = File.ReadAllText(DataPath + "NpcDefine.txt");
            Npcs = JsonConvert.DeserializeObject<Dictionary<int, NpcDefine>>(json);

            yield return null;

            json = File.ReadAllText(DataPath + "ItemDefine.txt");
            Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);

            yield return null;

            json = File.ReadAllText(DataPath + "ShopDefine.txt");
            Shops = JsonConvert.DeserializeObject<Dictionary<int, ShopDefine>>(json);

            yield return null;

            json = File.ReadAllText(DataPath + "ShopItemDefine.txt");
            ShopItems = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, ShopItemDefine>>>(json);

            yield return null;

            json = File.ReadAllText(DataPath + "EquipDefine.txt");
            Equips = JsonConvert.DeserializeObject<Dictionary<int, EquipDefine>>(json);

            yield return null;

            json = File.ReadAllText(DataPath + "QuestDefine.txt");
            Quests = JsonConvert.DeserializeObject<Dictionary<int, QuestDefine>>(json);

            yield return null;

            json = File.ReadAllText(DataPath + "SpawnPointDefine.txt");
            this.SpawnPoints = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SpawnPointDefine>>>(json);

            yield return null;

            json = File.ReadAllText(DataPath + "SpawnRuleDefine.txt");
            this.SpawnRules = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, SpawnRuleDefine>>>(json);

            yield return null;

            json = File.ReadAllText(DataPath + "RideDefine.txt");
            this.Rides = JsonConvert.DeserializeObject<Dictionary<int, RideDefine>>(json);

            yield return null;
        }


#if UNITY_EDITOR
        public void SaveTeleporters()
        {
            string json = JsonConvert.SerializeObject(Teleporters, Formatting.Indented);
            File.WriteAllText(DataPath + "TeleporterDefine.txt", json);
        }
        public void SaveSpawnPoints()
        {
            string json = JsonConvert.SerializeObject(SpawnPoints, Formatting.Indented);
            File.WriteAllText(DataPath + "SpawnPointDefine.txt", json);
        }
#endif
    }
}