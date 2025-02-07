using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Managers;
using System.Diagnostics;

namespace GameServer.Services
{
    class UserService : Singleton<UserService>
    {

        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(OnCreateCharacter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(OnGameEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameLeaveRequest>(OnGameLeave);
            
            

        }

        public void Init()
        {

        }


        // 用户登录客户端需要做以下事件
        // 检测数据库中是否有登录的账号
        // 检测该账号密码是否输入正确
        // 账号密码都正确则将玩家信息返回
        void OnLogin(NetConnection<NetSession> sender, UserLoginRequest request)
        {
            Log.ErrorFormat("UserLoginRequest: User:{0}, Pass:{1}", request.User, request.Passward);
            sender.Session.Response.userLogin = new UserLoginResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if(user == null)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "账号不存在。";
            }
            else if(user.Password != request.Passward)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "密码错误。";
            }
            else
            {
                sender.Session.User = user;
                sender.Session.Response.userLogin.Result = Result.Success;
                sender.Session.Response.userLogin.Errormsg = "None";
                sender.Session.Response.userLogin.Userinfo = new NUserInfo();
                sender.Session.Response.userLogin.Userinfo.Id = (int)user.ID;
                sender.Session.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                sender.Session.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                foreach(var val in user.Player.Characters)
                {
                    NCharacterInfo characterInfo = new NCharacterInfo();
                    characterInfo.Name = val.Name;
                    characterInfo.Id = val.ID;
                    characterInfo.Type = CharacterType.Player;
                    characterInfo.Class = (CharacterClass)val.Class;
                    characterInfo.ConfigId = val.ID;
                    sender.Session.Response.userLogin.Userinfo.Player.Characters.Add(characterInfo);
                }
            }
            sender.SendResponse();
        }
        /*void OnLogin(NetConnection<NetSession> sender, UserLoginRequest request)  
        {
            Log.InfoFormat("UserLoginRequest: User:{0}  Pass:{1}", request.User, request.Passward);

            sender.Session.Response.userLogin = new UserLoginResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user == null)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "用户不存在";
            }
            else if (user.Password != request.Passward)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "密码错误";
            }
            else
            {
                sender.Session.User = user;

                sender.Session.Response.userLogin.Result = Result.Success;
                sender.Session.Response.userLogin.Errormsg = "None";
                sender.Session.Response.userLogin.Userinfo = new NUserInfo();
                sender.Session.Response.userLogin.Userinfo.Id = (int)user.ID;
                sender.Session.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                sender.Session.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                foreach (var c in user.Player.Characters)
                {
                    NCharacterInfo info = new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Type = CharacterType.Player;
                    info.Class = (CharacterClass)c.Class;
                    info.ConfigId = c.ID;
                    sender.Session.Response.userLogin.Userinfo.Player.Characters.Add(info);
                }

            }
            sender.SendResponse();
        }*/


        // 玩家注册服务端需要做的操作有
        // 检测数据库中是否存在该账号
        // 不存在则创建该账号并更新数据库
        void OnRegister(NetConnection<NetSession> connection, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: User{0}, Password :{1}", request.User, request.Passward);
            connection.Session.Response.userRegister = new UserRegisterResponse();
            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();

            if (user != null)
            {
                connection.Session.Response.userRegister.Result = Result.Failed;
                connection.Session.Response.userRegister.Errormsg = "用户名已存在。";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Passward, Player = player });
                DBService.Instance.Entities.SaveChanges();
                connection.Session.Response.userRegister.Result = Result.Success;
                connection.Session.Response.userRegister.Errormsg = "None";
            }
            connection.SendResponse();
        }
        /*void OnRegister(NetConnection<NetSession> conn, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: User:{0}  Pass:{1}", request.User, request.Passward);
            conn.Session.Response.userRegister = new UserRegisterResponse();


            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                conn.Session.Response.userRegister.Result = Result.Failed;
                conn.Session.Response.userRegister.Errormsg = "用户已存在.";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Passward, Player = player });
                DBService.Instance.Entities.SaveChanges();
                conn.Session.Response.userRegister.Result = Result.Success;
                conn.Session.Response.userRegister.Errormsg = "None";
            }
            conn.SendResponse();
        }*/

        /// <summary>
        /// 玩家创建角色时服务端要做的是，
        /// 先根据客户端发送的数据创建一个职业角色
        /// 再为数据库中对应的账号玩家添加该数据并更新数据库
        /// 新建服务端回应对象，为该对象赋值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        /*private void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {
            Log.InfoFormat("UserCreateCharacterRequest:: Name:{0} Class:{1}", request.Name, request.Class);

            TCharacter character = new TCharacter()
            {
                Name = request.Name,
                Class = (int)request.Class,
                TID = (int)request.Class,
                MapID = 1,
                MapPosX = 5000,
                MapPosY = 4000,
                MapPosZ = 820
            };

            DBService.Instance.Entities.Characters.Add(character);
            sender.Session.User.Player.Characters.Add(character);
            DBService.Instance.Entities.SaveChanges();

            NetMessage message = new NetMessage();
            message.Response.createChar = new UserCreateCharacterResponse()
            {
                Result = Result.Success,
                Errormsg = "None",
            };

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);            
        }*/
        private void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {
            Log.InfoFormat("UserCreateCharacterRequest: Name:{0}  Class:{1}", request.Name, request.Class);

            TCharacter character = new TCharacter()
            {
                Name = request.Name,
                Class = (int)request.Class,
                TID = (int)request.Class,
                Level = 1,
                MapID = 1,
                MapPosX = 5000, //初始出生位置X
                MapPosY = 4000, //初始出生位置Y
                MapPosZ = 820,
                Gold = 100000, //初始10万金币
                Equips = new byte[28]
            };
            var bag = new TCharacterBag();
            bag.Owner = character;
            bag.Items = new byte[0];
            bag.Unlocked = 20;
            character.Bag = DBService.Instance.Entities.CharacterBags.Add(bag);

            character = DBService.Instance.Entities.Characters.Add(character);
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 1,
                ItemCount = 20,
            });
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 2,
                ItemCount = 20,
            });
            sender.Session.User.Player.Characters.Add(character);

            DBService.Instance.Entities.SaveChanges();

            sender.Session.Response.createChar = new UserCreateCharacterResponse();
            sender.Session.Response.createChar.Result = Result.Success;
            sender.Session.Response.createChar.Errormsg = "None";

            foreach (var c in sender.Session.User.Player.Characters)
            {
                NCharacterInfo info = new NCharacterInfo();
                info.Id = c.ID;
                info.Name = c.Name;
                info.Type = CharacterType.Player;
                info.Class = (CharacterClass)c.Class;
                info.ConfigId = c.TID;
                sender.Session.Response.createChar.Characters.Add(info);
            }
            sender.SendResponse();
        }


        private void OnGameLeave(NetConnection<NetSession> sender, UserGameLeaveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("UserGameLeaveRequest::{0}", character.Name);
            CharacterLeave(character);
        }
        public void CharacterLeave(Character character)
        {
            CharacterManager.Instance.RemoveCharacter(character.Id);
            character.Clear();
            MapManager.Instance[character.Info.mapId].CharacterLeave(character.Info);
        }


        private void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest message)
        {
            TCharacter dbchar = sender.Session.User.Player.Characters.ElementAt(message.characterIdx);
            Log.InfoFormat("UserGameEnterRequest:: CharacterName:{0} CharacterID:{1} CharacterMapID:{2}", dbchar.Name, dbchar.ID, dbchar.MapID);
            Character cha = CharacterManager.Instance.AddCharacter(dbchar);
            sender.Session.Response.gameEnter = new UserGameEnterResponse();
            sender.Session.Response.gameEnter.Result = Result.Success;
            sender.Session.Response.gameEnter.Errormsg = "None";

            sender.Session.Character = cha;
            sender.Session.PostResponser = cha;
            sender.Session.Response.gameEnter.Character = cha.Info;
            sender.SendResponse();

            MapManager.Instance[dbchar.MapID].CharacterEnter(sender, cha);

        }

        /*void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            TCharacter dbchar = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);
            Log.InfoFormat("UserGameEnterRequest: characterID:{0}:{1} Map:{2}", dbchar.ID, dbchar.Name, dbchar.MapID);
            Character character = CharacterManager.Instance.AddCharacter(dbchar);
            //SessionManager.Instance.AddSession(character.Id, sender);
            sender.Session.Response.gameEnter = new UserGameEnterResponse();
            sender.Session.Response.gameEnter.Result = Result.Success;
            sender.Session.Response.gameEnter.Errormsg = "None";

            //进入成功，发送初始角色信息
            sender.Session.Character = character;
            sender.Session.PostResponser = character;

            sender.Session.Response.gameEnter.Character = character.Info;
            sender.SendResponse();

            MapManager.Instance[dbchar.MapID].CharacterEnter(sender, character);
        }

        void OnGameLeave(NetConnection<NetSession> sender, UserGameLeaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("UserGameLeaveRequest: characterID:{0}:{1} Map:{2}", character.Id, character.Info.Name, character.Info.mapId);
            this.CharacterLeave(character);
            sender.Session.Response.gameLeave = new UserGameLeaveResponse();
            sender.Session.Response.gameLeave.Result = Result.Success;
            sender.Session.Response.gameLeave.Errormsg = "None";

            sender.SendResponse();
        }

        public void CharacterLeave(Character character)
        {
            Log.InfoFormat("CharacterLeave： characterID:{0}:{1}", character.Id, character.Info.Name);
            //SessionManager.Instance.RemoveSession(character.Id);
            CharacterManager.Instance.RemoveCharacter(character.Id);
            character.Clear();
            MapManager.Instance[character.Info.mapId].CharacterLeave(character);
        }*/
    }
}
