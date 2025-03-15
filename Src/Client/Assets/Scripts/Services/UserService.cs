using System;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;
using UnityEngine.Events;
using Assets.Scripts.Services;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
//using Models;
//using Managers;

namespace Services
{
    class UserService : Singleton<UserService>, IDisposable
    {

        public UnityEngine.Events.UnityAction<Result, string> OnLogin;
        public UnityEngine.Events.UnityAction<Result, string> OnRegister;
        public UnityEngine.Events.UnityAction<Result, string> OnCharacterCreate;
        public UnityEngine.Events.UnityAction<Result, string> OnGameEnter;
        public UnityAction<Result, string> OnGameLeave;

        NetMessage pendingMessage = null;

        bool connected = false;

        bool isQuitGame = false;

        public UserService()
        {
            NetClient.Instance.OnConnect += OnGameServerConnect;
            NetClient.Instance.OnDisconnect += OnGameServerDisconnect;
            MessageDistributer.Instance.Subscribe<UserLoginResponse>(this.OnUserLogin);
            MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
            MessageDistributer.Instance.Subscribe<UserGameEnterResponse>(OnUserGameEnter);
            MessageDistributer.Instance.Subscribe<UserGameLeaveResponse>(OnUserGameLeave);
            
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<UserLoginResponse>(this.OnUserLogin);
            MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
            MessageDistributer.Instance.Unsubscribe<UserGameEnterResponse>(OnUserGameEnter);
            MessageDistributer.Instance.Unsubscribe<UserGameLeaveResponse>(OnUserGameLeave);
            NetClient.Instance.OnConnect -= OnGameServerConnect;
            NetClient.Instance.OnDisconnect -= OnGameServerDisconnect;
        }

        public void Init()
        {

        }

        public void ConnectToServer()
        {
            Debug.Log("ConnectToServer() Start ");
            //NetClient.Instance.CryptKey = this.SessionId;
            NetClient.Instance.Init("127.0.0.1", 8000);
            NetClient.Instance.Connect();
        }


        void OnGameServerConnect(int result, string reason)
        {
            Log.InfoFormat("LoadingMesager::OnGameServerConnect :{0} reason:{1}", result, reason);
            if (NetClient.Instance.Connected)
            {
                this.connected = true;
                if(this.pendingMessage!=null)
                {
                    NetClient.Instance.SendMessage(this.pendingMessage);
                    this.pendingMessage = null;
                }
            }
            else
            {
                if (!this.DisconnectNotify(result, reason))
                {
                    MessageBox.Show(string.Format("网络错误，无法连接到服务器！\n RESULT:{0} ERROR:{1}", result, reason), "错误", MessageBoxType.Error);
                }
            }
        }

        public void OnGameServerDisconnect(int result, string reason)
        {
            this.DisconnectNotify(result, reason);
            return;
        }

        bool DisconnectNotify(int result,string reason)
        {
            if (this.pendingMessage != null)
            {
                if (this.pendingMessage.Request.userLogin!=null)
                {
                    if (this.OnLogin != null)
                    {
                        this.OnLogin(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                else if(this.pendingMessage.Request.userRegister!=null)
                {
                    if (this.OnRegister != null)
                    {
                        this.OnRegister(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                else
                {
                    if (this.OnCharacterCreate != null)
                    {
                        this.OnCharacterCreate(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 发送用户登录请求
        /// </summary>
        /// <param name="user"></param>
        /// <param name="psw"></param>
        public void SendLogin(string user, string psw)
        {
            Debug.LogFormat("UserLoginRequest::user:{0} psw:{1}", user, psw);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userLogin = new UserLoginRequest();
            message.Request.userLogin.User = user;
            message.Request.userLogin.Passward = psw;

            // 判断是否处于连接
            // 是则清空缓存消息列表并直接发送到服务端
            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            //不是则将该消息放入缓存消息队列并重新连接服务端
            else
            {
                pendingMessage = message;
                ConnectToServer();
            }
        }
        /// <summary>
        /// 当服务端发送回应时，触发登录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>

        void OnUserLogin(object sender, UserLoginResponse response)
        {
            Log.InfoFormat("OnUserLogin:{0}[{1}]", response.Result, response.Errormsg);

            if (response.Result == Result.Success)
            {
                User.Instance.SetUserInfo(response.Userinfo);
            }
            if (this.OnLogin != null)
            {
                this.OnLogin(response.Result, response.Errormsg);
            }
        }

        /// <summary>
        /// 发送客户端注册请求至服务端
        /// </summary>
        /// <param name="user"></param>
        /// <param name="psw"></param>
        public void SendRegister(string user, string psw)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userRegister = new UserRegisterRequest();
            message.Request.userRegister.User = user;
            message.Request.userRegister.Passward = psw;
            if(connected &&NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                pendingMessage = message;
                ConnectToServer();
            }
        }

        /// <summary>
        /// 接收服务端注册请求回应，并触发注册事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        void OnUserRegister(object sender, UserRegisterResponse response)
        {
            Debug.LogFormat("UserRegisterResponse::{0}[{1}]", response.Result, response.Errormsg);

            if (this.OnRegister != null)
            {
                this.OnRegister(response.Result, response.Errormsg);
            }
        }

        //发送创建角色的请求：有如下操作
        //创建创建角色请求，为创建角色请求的角色名称和角色类型赋值
        public void SendCharacterCreate(string name, CharacterClass clas)
        {
            Debug.LogFormat("UserCreateCharacterRequest::name :{0} class:{1}", name, clas);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.createChar = new UserCreateCharacterRequest();
            message.Request.createChar.Name = name;
            message.Request.createChar.Class = clas;
            if(connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage= message;
                this.ConnectToServer();
            }
        }

        /// <summary>
        /// 接收服务端创建角色的回应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>

        void OnUserCreateCharacter(object sender, UserCreateCharacterResponse response)
        {
            Debug.LogFormat("UserCreateCharacterResponse::{0}:{1}", response.Result, response.Errormsg);

            if(response.Result == Result.Success)
            {
                User.Instance.Info.Player.Characters.Clear();
                User.Instance.Info.Player.Characters.AddRange(response.Characters);
            }
            if(OnCharacterCreate != null)
            {
                OnCharacterCreate(response.Result, response.Errormsg);
            }
        }

        /// <summary>
        /// 发送客户端角色进入游戏的逻辑，将该玩家的角色序号发给服务端
        /// </summary>
        /// <param name="charIndex"></param>
        public void SendGameEnter(int charIndex)
        {
            Debug.LogFormat("UserGameEnterRequest::charIndex:{0}", charIndex);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameEnter = new UserGameEnterRequest();
            message.Request.gameEnter.characterIdx = charIndex;
            if(connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }

        /// <summary>
        /// 发送客户端玩家离开游戏请求
        /// </summary>
        /// <param name="isQuitGame"></param>
        public void SendGameLeave(bool isQuitGame)
        {
            this.isQuitGame = isQuitGame;
            Debug.LogFormat("UserGameLeaveRequest::{0}", isQuitGame);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameLeave = new UserGameLeaveRequest();
            NetClient.Instance.SendMessage(message);
        }

        private void OnUserGameEnter(object sender, UserGameEnterResponse response)
        {
            Debug.LogFormat("UserGameEnterResponse::{0} {1}", response.Result, response.Errormsg);
            if (response.Result == Result.Success)
            {
                if (response.Character != null)
                {
                    User.Instance.currentCharacter = response.Character;
                    ItemManager.Instance.Init(response.Character.Items);
                    BagManager.Instance.Init(response.Character.Bag);
                    EquipManager.Instance.Init(response.Character.Equips);
                    QuestManager.Instance.Init(response.Character.Quests);
                    FriendManager.Instance.Init(response.Character.Friends);
                }
            }
        }

        private void OnUserGameLeave(object sender, UserGameLeaveResponse message)
        {
            MapService.Instance.currentMapId = 0;
            User.Instance.currentCharacter = null;
            Debug.LogFormat("UserGameLeaveResponse::{0} {1}", message.Result, message.Errormsg);
            /*if (this.isQuitGame)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }*/
        }

        /*void OnGameLeave(object sender, UserGameLeaveResponse response)
        {
            //MapService.Instance.CurrentMapId = 0;
            //User.Instance.CurrentCharacter = null;
            Debug.LogFormat("OnGameLeave:{0} [{1}]", response.Result, response.Errormsg);
            if(this.isQuitGame)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }*/
    }
}
