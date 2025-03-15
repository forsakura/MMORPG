using Common.Data;
using SkillBridge.Message;

namespace Assets.Scripts.Models
{
    public class User : Singleton<User>
    {
        NUserInfo userInfo;

        public NCharacterInfo currentCharacter;

        public MapDefine curentMiniMap { get; set; }

        public UnityEngine.GameObject currentCharacterObject { get; set; }
        public NTeamInfo TeamInfo { get; set; }
        public NUserInfo Info
        {
            get { return userInfo; }
        }

        public void SetUserInfo(NUserInfo info)
        {
            userInfo = info;
        }

        internal void AddMoney(int value)
        {
            currentCharacter.Gold += value;
        }
    }
}