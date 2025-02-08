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
            EntityManager.Instance.AddEntity(character.MapID, cha);
            cha.Info.Id = cha.Id;
            Characters[cha.Id] = cha;
            return cha;
        }

        public void RemoveCharacter(int characterID)
        {
            if (Characters.ContainsKey(characterID))
            {
                var cha = Characters[characterID];
                EntityManager.Instance.RemoveEntity(cha.Data.MapID, cha);
                Characters.Remove(characterID);
            }
        }

        public Character GetCharacter(int characterID)
        {
            Character character = null;
            Characters.TryGetValue(characterID, out character); 
            return character;
        }
    }
}
