using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
    {
        Dictionary<Transform, GameObject> elements = new Dictionary<Transform, GameObject>();

        public GameObject uiNameBarPrefab;

        public void AddCharacterElement(Transform owner, Character character)
        {
            GameObject gameObject = Instantiate(uiNameBarPrefab, transform);
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
    }
}