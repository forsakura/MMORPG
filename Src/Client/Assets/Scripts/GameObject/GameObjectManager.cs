using Assets.Scripts.UI;
using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour {

	Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();
	// Use this for initialization
	void Start () {
		StartCoroutine(InitGameObjects());
		CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
		CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
	}


    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter = null;
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

    private void OnCharacterLeave(Character arg0)
    {
        
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
        }

        InitGameObject(Characters[cha.entityId], cha);
    }

    private void InitGameObject(GameObject gameObject, Character cha)
    {
        gameObject.transform.position = GameObjectTool.LogicToWorld(cha.position);
        gameObject.transform.forward = GameObjectTool.LogicToWorld(cha.direction);
        EntityController ec = gameObject.GetComponent<EntityController>();
        Characters[cha.Info.Id] = gameObject;
        if(ec != null)
        {
            ec.entity = cha;
            ec.isPlayer = cha.IsCurrentPlayer;
        }
        PlayerInputController playerInputController = gameObject.GetComponent<PlayerInputController>();
        if (playerInputController != null)
        {
            if(cha.Info.Id == User.Instance.currentCharacter.Id)
            {
                User.Instance.currentCharacterObject = gameObject;
                MainPlayerCamera.Instance.player = gameObject;
                playerInputController.enabled = true;
                playerInputController.character = cha;
                playerInputController.entityController = ec;
            }
            else playerInputController.enabled = false;
        }
        UIWorldElementManager.Instance.AddCharacterElement(gameObject.transform, cha);
    }
}
