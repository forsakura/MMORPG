using Assets.Scripts.Managers;
using Assets.Scripts.UI.Quest;
using Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
    {
        Dictionary<Transform, UnityEngine.GameObject> elements = new Dictionary<Transform, UnityEngine.GameObject>();

        public UnityEngine.GameObject uiNameBarPrefab;

        Dictionary<Transform, UnityEngine.GameObject> elementsQuest = new Dictionary<Transform, UnityEngine.GameObject>();

        public UnityEngine.GameObject uiQuestBarPrefab;

        public void AddCharacterElement(Transform owner, Character character)
        {
            UnityEngine.GameObject gameObject = Instantiate(uiNameBarPrefab, transform);
            gameObject.name = "UIWorldName" + character.Id;
            gameObject.GetComponent<UINameBar>().character = character;
            gameObject.GetComponent<UIWorldElement>().owner = owner;
            gameObject.SetActive(true);
            elements[owner] = gameObject;
        }

        public void RemoveCharacterElement(Transform owner)
        {
            if(elements.ContainsKey(owner))
            {
                Destroy(elements[owner]);
                elements.Remove(owner);
            }
        }

        public void AddNpcQuestStatus(Transform owner, NpcQuestStatus status)
        {
            if (elementsQuest.ContainsKey(owner))
            {
                elementsQuest[owner].GetComponent<UIQuestStatus>().SetQuestStatus(status);
            }
            else
            {
                UnityEngine.GameObject go = Instantiate(uiQuestBarPrefab, transform);
                go.name = "UIWorldName" + status;
                go.GetComponent<UIWorldElement>().owner = owner;
                go.GetComponent<UIQuestStatus>().SetQuestStatus(status);
                go.SetActive(true);
                elementsQuest[owner] = go;
            }
        }

        public void RemoveNpcQuestStatus(Transform owner)
        {
            if (elementsQuest.ContainsKey(owner))
            {
                Destroy(elementsQuest[owner]);
                elementsQuest.Remove(owner);
            }
        }
    }
}