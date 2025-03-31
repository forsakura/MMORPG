using Assets.Scripts.Managers;
using Entities;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterManager : Singleton<CharacterManager>, IDisposable
{
    public Dictionary<int, Character> Characters = new Dictionary<int, Character>();
    public UnityAction<Character> OnCharacterEnter;
    public UnityAction<Character> OnCharacterLeave;

    public CharacterManager()
    {

    }

    public void Init()
    {

    }
    public void Dispose()
    {
        
    }

    public void Clear()
    {
        foreach (var item in Characters)
        {
            RemoveCharacter(item.Key);
        }
        Characters.Clear();
    }

    internal void AddCharacter(NCharacterInfo cha)
    {
        Debug.LogFormat("AddCharacter:chaID:{0} chaName:{1} Map:{2} Entity:{3}", cha.Id, cha.Name, cha.mapId, cha.EntityId);
        var character = new Character(cha);
        EntityManager.Instance.AddEntity(character);
        Characters[cha.Entity.Id] = character;
        if (OnCharacterEnter != null)
        {
            OnCharacterEnter(character);
        }
    }

    public void RemoveCharacter(int characterId)
    {
        Debug.LogFormat("RemoveCharacter: {0}", characterId);
        if (Characters.ContainsKey(characterId))
        {
            EntityManager.Instance.RemoveEntity(Characters[characterId].Info.Entity);
            if(OnCharacterLeave != null)
            {
                OnCharacterLeave(Characters[characterId]);
            }
            else Characters.Remove(characterId);
        }
    }

    public Character GetCharacter(int characterId)
    {
        Character character = null;
        Characters.TryGetValue(characterId, out character);
        return character;
    }
}
