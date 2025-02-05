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
        Characters.Clear();
    }

    internal void AddCharacter(NCharacterInfo cha)
    {
        Debug.LogFormat("AddCharacter:chaID:{0} chaName:{1} Map:{2} Entity:{3}", cha.Id, cha.Name, cha.mapId, cha.EntityId);
        var character = new Character(cha);

        Characters[cha.Id] = character;
        if (OnCharacterEnter != null)
        {
            OnCharacterEnter(character);
        }
    }

    void RemoveCharacter(int characterId)
    {
        Debug.LogFormat("RemoveCharacter: {0}", characterId);
        Character cha = Characters[characterId];
        Characters.Remove(characterId);
        if(OnCharacterLeave != null)
        {
            OnCharacterLeave(cha);
        }
    }
}
