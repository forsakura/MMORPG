﻿using Assets.Scripts.UI.CharEquip;
using Assets.Scripts.UI.Chat;
using Assets.Scripts.UI.Friend;
using Assets.Scripts.UI.Guild;
using Assets.Scripts.UI.Quest;
using Assets.Scripts.UI.Ride;
using Assets.Scripts.UI.Setting;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.UI
{
    public class UIManager : Singleton<UIManager>
    {
        class UIElement
        {
            public string resource;
            public bool cache;
            public UnityEngine.GameObject instance;
        }

        Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();

        public UIManager()
        {
            UIResources.Add(typeof(UIBag.UIBag), new UIElement() { resource = "UI/UIBag", cache = false, instance = null });
            UIResources.Add(typeof(UIShop), new UIElement() { resource = "UI/UIShop", cache = false });
            UIResources.Add(typeof(UICharEquip), new UIElement() { resource = "UI/UICharEquip", cache = false });
            UIResources.Add(typeof(UIQuestSystem), new UIElement() { resource = "UI/UIQuest", cache = false });
            UIResources.Add(typeof(UIQuestDialog), new UIElement() { resource = "UI/UIQuestDialog", cache = false });
            UIResources.Add(typeof(UIFriend), new UIElement() { resource = "UI/UIFriend", cache = false });
            UIResources.Add(typeof(UIGuild), new UIElement() { resource = "UI/UIGuild", cache = false });
            UIResources.Add(typeof(UIGuildList), new UIElement() { resource = "UI/UIGuildList", cache = false });
            UIResources.Add(typeof(UIGuildPopNoGuild), new UIElement() { resource = "UI/UIGuildPopNoGuild", cache = false });
            UIResources.Add(typeof(UIGuildPopCreate), new UIElement() { resource = "UI/UIGuildPopCreate", cache = false });
            UIResources.Add(typeof(UIGuildApplyList), new UIElement() { resource = "UI/UIGuildApplyList", cache = false });
            UIResources.Add(typeof(UISetting), new UIElement() { resource = "UI/UISetting", cache = false });
            UIResources.Add(typeof(UIChatPopMenu), new UIElement() { resource = "UI/UIChatPopMenu", cache = false });
            UIResources.Add(typeof(UIRide), new UIElement() { resource = "UI/UIRide", cache = false });
            UIResources.Add(typeof(UISoundConfig), new UIElement() { resource = "UI/UISoundConfig", cache = false });
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
                    res.instance = (UnityEngine.GameObject)UnityEngine.Object.Instantiate(obj, UIMain.Instance.transform);
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