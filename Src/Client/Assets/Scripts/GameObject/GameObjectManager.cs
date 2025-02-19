using Assets.Scripts.Models;
using Assets.Scripts.UI;
using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoSingleton<GameObjectManager> {

	Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();
	// Use this for initialization
	protected override void OnStart () {
		StartCoroutine(InitGameObjects());
		CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
		CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
	}


    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter -= OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave -= OnCharacterLeave;
    }

    // Update is called once per frame
    void Update () {
		
	}

    private IEnumerator InitGameObjects()
    {
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }

	void OnCharacterEnter(Character cha)
	{
		CreateCharacterObject(cha);
	}

    private void OnCharacterLeave(Character character)
    {
        if(!Characters.ContainsKey(character.entityId))
        {
            return;
        }
        if (Characters[character.entityId] != null)
        {
            Destroy(Characters[character.entityId]);
            Characters.Remove(character.entityId);
        }
    }

    private void CreateCharacterObject(Character cha)
    {
        if(!Characters.ContainsKey(cha.entityId) || Characters[cha.entityId] == null)
        {
            Object obj = Resloader.Load<Object>(cha.Define.Resource);
            if(obj == null)
            {
                Debug.LogErrorFormat("Character{0} Resource{1} not existed", cha.Define.TID, cha.Define.Resource);
                return;
            }
            GameObject go = (GameObject)Instantiate(obj, this.transform);
            go.name = "Character_" + cha.entityId + "_" + cha.Name;
            Characters[cha.entityId] = go;
            UIWorldElementManager.Instance.AddCharacterElement(go.transform, cha);
        }
        InitGameObject(Characters[cha.entityId], cha);
    }

    private void InitGameObject(GameObject gameObject, Character cha)
    {
        gameObject.transform.position = GameObjectTool.LogicToWorld(cha.position);
        gameObject.transform.forward = GameObjectTool.LogicToWorld(cha.direction);
        EntityController ec = gameObject.GetComponent<EntityController>();
        if(ec != null)
        {
            ec.entity = cha;
            ec.isPlayer = cha.IsCurrentPlayer;
        }
        PlayerInputController playerInputController = gameObject.GetComponent<PlayerInputController>();
        if (playerInputController != null)
        {
            if(cha.entityId == User.Instance.currentCharacter.Entity.Id)
            {
                User.Instance.currentCharacterObject = gameObject;
                MainPlayerCamera.Instance.player = gameObject;
                playerInputController.enabled = true;
                playerInputController.character = cha;
                playerInputController.entityController = ec;
            }
            else playerInputController.enabled = false;
        }
    }
}
