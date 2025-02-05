using UnityEngine;

//class MessageBox
//{
//    static Object cacheObject = null;

//    public static UIMessageBox Show(string message, string title="", MessageBoxType type = MessageBoxType.Information, string btnOK = "", string btnCancel = "")
//    {
//        if(cacheObject==null)
//        {
//            cacheObject = Resloader.Load<Object>("UI/UIMessageBox");
//        }

//        GameObject go = (GameObject)GameObject.Instantiate(cacheObject);
//        UIMessageBox msgbox = go.GetComponent<UIMessageBox>();
//        msgbox.Init(title, message, type, btnOK, btnCancel);
//        return msgbox;
//    }
//}

//public enum MessageBoxType
//{
//    /// <summary>
//    /// Information Dialog with OK button
//    /// </summary>
//    Information = 1,

//    /// <summary>
//    /// Confirm Dialog whit OK and Cancel buttons
//    /// </summary>
//    Confirm = 2,

//    /// <summary>
//    /// Error Dialog with OK buttons
//    /// </summary>
//    Error = 3
//}
/// <summary>
/// 该类用来显示提示面板的提示信息,思路是将资源中的预制体加载到场景中，并对预制体进行初始化
/// </summary>
class MessageBox
{
    static Object cacheObject = null;
    public static UIMessageBox Show(string message, string title = "", MessageBoxType type = MessageBoxType.Information, string btnOk = "", string btnCancel = "")
    {
        if(cacheObject == null) 
        {
            cacheObject = Resloader.Load<Object>("UI/UIMessageBox");
        }

        GameObject go = (GameObject)GameObject.Instantiate(cacheObject);
        UIMessageBox msgBox = go.GetComponent<UIMessageBox>();
        msgBox.Init(title, message, type, btnOk, btnCancel);
        return msgBox;
    }
}

public enum MessageBoxType
{
    Information = 1,
    Confirm,
    Error
}