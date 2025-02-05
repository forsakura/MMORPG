using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UINameBar : MonoBehaviour
    {
        public Text uiText;

        public Character character;
        // Use this for initialization
        void Start()
        {
            UpdateView();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateView();
        }
        void UpdateView()
        {
            string text = string.Format("{0} [{1}]",character.Name, character.Info.Level);
            if (uiText.text != text)
            {
                uiText.text = text;
            }
        }
    }
}