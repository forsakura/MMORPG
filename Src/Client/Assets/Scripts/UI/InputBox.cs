using UnityEngine;

namespace Assets.Scripts.UI
{
    class InputBox
    {
        static Object cacheObject = null;
        public static UIInputBox Show(string message, string title = "", string btnOk = "", string btnCancel = "", string emptyTips = "")
        {
            if (cacheObject == null)
            {
                cacheObject = Resloader.Load<Object>("UI/UIInputBox");
            }

            UnityEngine.GameObject go = (UnityEngine.GameObject)UnityEngine.GameObject.Instantiate(cacheObject);
            UIInputBox msgBox = go.GetComponent<UIInputBox>();
            msgBox.Init(title, message, btnOk, btnCancel, emptyTips);
            return msgBox;
        }
    }
}