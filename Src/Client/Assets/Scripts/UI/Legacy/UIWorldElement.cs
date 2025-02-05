using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Legacy
{
    public class UIWorldElement : MonoBehaviour
    {
        public Transform owner;

        public float height;
        // Use this for initialization
        void Start()
        {
            height = 2f;
        }

        // Update is called once per frame
        void Update()
        {
            if (owner != null)
            {
                transform.position = owner.position + Vector3.up * height;
            }
            if (Camera.main != null)
            {
                transform.forward = Camera.main.transform.forward;
            }
        }
    }
}