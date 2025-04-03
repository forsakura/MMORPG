using SkillBridge.Message;
using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
    internal class FriendManager : Singleton<FriendManager>
    {
        internal List<NFriendInfo> allFriends = new List<NFriendInfo>();

        public void Init(List<NFriendInfo> friends)
        {
            allFriends = friends;
        }

        public bool HasFriend(int friendId)
        {
            foreach (var item in allFriends)
            {
                if(item.friendInfo.Id == friendId)
                    return true;
            }
            return false;
        }
    }
}
