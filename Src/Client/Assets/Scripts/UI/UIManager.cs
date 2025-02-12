using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIManager : Singleton<UIManager>
    {
        class UIElement
        {
            public string resource;
            public bool cache;
            public GameObject instance;
        }

        Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();

        public UIManager()
        {
            UIResources.Add(typeof(UITest), new UIElement() { resource = "UI/UITest", cache = false, instance = null });
        }

        public T Show<T>()
        {
            Type type = typeof(T);
            if (UIResources.ContainsKey(type))
            {
                var res = UIResources[type];
                if (res.instance != null)
                {
                    res.instance.SetActive(true);
                }
                else
                {
                    UnityEngine.Object obj = Resloader.Load<UnityEngine.Object>(res.resource);
                    if (obj == null) return default(T);
                    res.instance = (GameObject)UnityEngine.Object.Instantiate(obj, UIMain.Instance.transform);
                }
                return res.instance.GetComponent<T>();
            }
            return default(T);
        }

        public void Close(Type type)
        {
            if (UIResources.ContainsKey(type))
            {
                var res = UIResources[type];
                if (res.cache)
                {
                    res.instance.SetActive(false);
                }
                else
                {
                    UnityEngine.Object.Destroy(res.instance);
                    res.instance = null;
                }
            }
        }
    }
}