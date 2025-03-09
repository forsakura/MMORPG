using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Legacy
{
    public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
    {

        public UnityEngine.GameObject userNameBarPrefab;
        Dictionary<Transform, UnityEngine.GameObject> elements = new Dictionary<Transform, UnityEngine.GameObject>();
        public void AddCharacterNameBar(Transform owner, Character character)
        {
            UnityEngine.GameObject gameObject = Instantiate(userNameBarPrefab, transform);
            gameObject.name = "NameBar" + character.entityId;
            gameObject.GetComponent<UIWorldElement>().owner = owner;
            gameObject.GetComponent<UINameBar>().character = character;
            gameObject.SetActive(true);
            elements[owner] = gameObject;
        }

        public void RemoveCharacterNameBar(Transform owner)
        {
            if (elements.ContainsKey(owner))
            {
                Destroy(elements[owner]);
                elements.Remove(owner);
            }
        }
    }
}