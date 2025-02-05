using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;
using GameServer.Entities;

namespace GameServer.Managers
{
    class CharacterManager : Singleton<CharacterManager>
    {
        //public Dictionary<int, Character> Characters = new Dictionary<int, Character>();
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

        public CharacterManager()
        {
        }

        public void Dispose()
        {
        }

        public void Init()
        {

        }

        public void Clear()
        {
            Characters.Clear();
        }

        public Character AddCharacter(TCharacter character)
        {
            Character cha = new Character(CharacterType.Player, character);
            cha.Info.EntityId = cha.entityId;
            Characters[cha.Id] = cha;
            //Characters.Add(character.ID, cha);
            return cha;
        }

        public void RemoveCharacter(int characterID)
        {
            if (Characters.ContainsKey(characterID))
            {
                var cha = Characters[characterID];
                Characters.Remove(characterID);
            }
        }

        public Character GetCharacter(int characterID)
        {
            Character character = null;
            Characters.TryGetValue(characterID, out character); 
            return character;
        }

        /*public void Clear()
        {
            this.Characters.Clear();
        }

        public Character AddCharacter(TCharacter cha)
        {
            Character character = new Character(CharacterType.Player, cha);
            //EntityManager.Instance.AddEntity(cha.MapID, character);
            character.Info.EntityId = character.entityId;
            this.Characters[character.Id] = character;
            return character;
        }


        public void RemoveCharacter(int characterId)
        {
            if (this.Characters.ContainsKey(characterId))
            {
                var cha = this.Characters[characterId];
                //EntityManager.Instance.RemoveEntity(cha.Data.MapID, cha);
                this.Characters.Remove(characterId);
            }
        }

        public Character GetCharacter(int characterId)
        {
            Character character = null;
            this.Characters.TryGetValue(characterId, out character);
            return character;
        }*/
    }
}
