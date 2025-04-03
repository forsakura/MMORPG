using Assets.Scripts.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIWindow : MonoBehaviour
    {
        public enum WindowResult
        {
            None,
            Yes,
            No
        }
        public delegate void CloseHander(UIWindow window, WindowResult result);
        public event CloseHander onClose;
        public virtual Type Type
        {
            get { return this.GetType(); }
        }

        public UnityEngine.GameObject Root;
        public void Close(WindowResult result = WindowResult.None)
        {
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Win_Close);
            UIManager.Instance.Close(Type);
            if (onClose != null)
            {
                onClose(this, result);
            }
            onClose = null;
        }

        public virtual void OnCloseClick()
        {
            Close();
        }
        public virtual void OnYesClick()
        {
            Close(WindowResult.Yes);
        }

        public virtual void OnNoClick()
        {
            Close(WindowResult.No);
        }
    }
}