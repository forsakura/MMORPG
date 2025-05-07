using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIInputBox : MonoBehaviour
    {
        public Text title;
        public Text messageTop;
        public Text messageBottom;
        public InputField input;
        public Button buttonYes;
        public Button buttonNo;

        public Text buttonYesTitle;
        public Text buttonNoTitle;

        public delegate bool SubmitHandler(string inputText, out string tips);
        public event SubmitHandler OnSubmit;
        public UnityAction OnNo;

        public string emptyTips;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            InputManager.Instance.isInputMode = input.isFocused;
        }

        public void Init(string title, string message, string btnOK = "", string btnCancel = "", string emptyTips = "")
        {
            if (!string.IsNullOrEmpty(title)) this.title.text = title;
            this.messageTop.text = message;
            this.messageBottom.text = null;
            this.OnSubmit = null;
            this.emptyTips = null;

            if (!string.IsNullOrEmpty(btnOK)) buttonYesTitle.text = btnOK;
            if (!string.IsNullOrEmpty(btnCancel)) buttonNoTitle.text = btnCancel;

            buttonYes.onClick.AddListener(OnClickYes);
            buttonNo.onClick.AddListener(OnClickNo);
        }

        void OnClickYes()
        {
            this.messageBottom.text = "";
            if(string.IsNullOrEmpty(input.text))
            {
                this.messageBottom.text = this.emptyTips;
                return;
            }
            if (OnSubmit != null)
            {
                string tips;
                if(!OnSubmit(input.text, out tips))
                {
                    this.messageBottom.text = tips;
                    return;
                }
            }
            Destroy(this.gameObject);
        }

        void OnClickNo()
        {
            //SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Win_Close);
            Destroy(gameObject);
            if (OnNo != null)
                OnNo();
        }
    }
}