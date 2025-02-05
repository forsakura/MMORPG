using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Legacy
{
    public class UINameBar : MonoBehaviour
    {
        public Text avaterText;

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
            if (character != null)
            {
                string text = character.Info.Name + "Lv." + character.Info.Level.ToString();
                if (text != avaterText.text)
                {
                    avaterText.text = text;
                }
            }
        }
    }
}