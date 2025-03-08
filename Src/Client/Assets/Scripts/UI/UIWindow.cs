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
        public void Close(WindowResult result = WindowResult.None)
        {
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
    }
}