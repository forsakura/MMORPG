using Common;
using GameServer.Entities;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Managers
{
    public class FriendManager : IPostResponser
    {
        Character owner;

        List<NFriendInfo> friends = new List<NFriendInfo>();

        bool friendChanged = false;
        public FriendManager(Character owner)
        {
            this.owner = owner;
            this.InitFriends();
        }

        private void InitFriends()
        {
            this.friends.Clear();
            foreach (var item in this.owner.Data.Friends)
            {
                this.friends.Add(GetFriendInfo(item));
            }
            //SortFriendByStatus();
        }

        internal void GetFriendInfos(List<NFriendInfo> friends)
        {
            foreach (var item in this.friends)
            {
                friends.Add(item);
            }
        }

        private NFriendInfo GetFriendInfo(TCharacterFriend item)
        {
            NFriendInfo friendInfo = new NFriendInfo();
            Character cha = CharacterManager.Instance.GetCharacter(item.FriendID);
            friendInfo.friendInfo = new NCharacterInfo();
            friendInfo.Id = item.Id;

            if (cha == null)
            {
                friendInfo.friendInfo.Id = item.FriendID;
                friendInfo.friendInfo.Name = item.FriendName;
                friendInfo.friendInfo.Class = (CharacterClass)item.Class;
                friendInfo.friendInfo.Level = item.Level;
                friendInfo.Status = 0;
            }
            else
            {
                friendInfo.friendInfo = cha.GetBasicInfo();
                friendInfo.friendInfo.Name = cha.Name;
                friendInfo.friendInfo.Class = cha.Info.Class;
                friendInfo.friendInfo.Level = cha.Info.Level;
                cha.FriendManager.UpdateInfo(this.owner.Info, 1);
                friendInfo.Status = 1;
            }
            Log.InfoFormat("{0} {1} GetFriendInfo :{2}:{3} Status: {4}", this.owner.Id, this.owner.Info.Name, friendInfo.friendInfo.Id, friendInfo.friendInfo.Name, friendInfo.Status);
            return friendInfo;
        }


        public NFriendInfo GetFriendInfo(int id)
        {
            NFriendInfo info = null;
            foreach (var item in friends)
            {
                if (item.Id == id)
                    info = item;
            }
            return info;
        }

        internal void AddFriend(Character character)
        {
            TCharacterFriend tf = new TCharacterFriend();
            tf.FriendID = character.Id;
            tf.FriendName = character.Name;
            tf.Class = character.Data.Class;
            tf.Level = character.Data.Level;
            this.owner.Data.Friends.Add(tf);
            friendChanged = true;
        }

        internal bool RemoveFriendByFriendId(int friendId)
        {
            var removeItem =  this.owner.Data.Friends.FirstOrDefault(v => v.FriendID == friendId);
            if (removeItem != null)
            {
                DBService.Instance.Entities.CharacterFriends.Remove(removeItem);
            }
            friendChanged = true;
            return true;
        }

        public bool RemoveFriendByID(int id)
        {
            var removeItem = this.owner.Data.Friends.FirstOrDefault(v => v.CharacterID == id);
            if(removeItem != null)
            {
                DBService.Instance.Entities.CharacterFriends.Remove(removeItem);
            }
            friendChanged = true;
            return true;
        }

        public void PostProcess(NetMessageResponse message)
        {
            if(friendChanged)
            {
                this.InitFriends();
                if(message.friendList == null)
                {
                    message.friendList = new FriendListResponse();
                    message.friendList.Friends.AddRange(friends);
                }
                friendChanged = false;
            }
        }

        public void OfflineNotify()
        {
            foreach (var item in friends)
            {
                var cha = CharacterManager.Instance.GetCharacter(item.friendInfo.Id);
                if (cha != null)
                    cha.FriendManager.UpdateInfo(this.owner.Info, 0);
            }
        }

        internal void UpdateInfo(NCharacterInfo info, int v)
        {
            foreach (var item in friends)
            {
                if(item.friendInfo.Id == info.Id)
                {
                    item.Status = v;
                    break;
                }
            }
            this.friendChanged = true;
        }

        /// <summary>
        /// 按状态排序
        /// </summary>
        private void SortFriendByStatus()
        {
            int fastIndex = 0;
            int slowIndex = 0;
            for (; fastIndex < friends.Count; fastIndex++)
            {
                if (friends[fastIndex].Status == 1)
                    friends.Insert(slowIndex++, friends[fastIndex]);
            }
        }

        /// <summary>
        /// 模糊搜索
        /// </summary>
        private void FuzzySearch()
        {

        }
    }
}
