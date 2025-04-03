using Common.Data;
using SkillBridge.Message;

namespace Assets.Scripts.Models
{
    public class User : Singleton<User>
    {
        NUserInfo userInfo;

        public NCharacterInfo currentCharacter;

        public MapDefine curentMiniMap { get; set; }

        public PlayerInputController currentCharacterObject { get; set; }
        public NTeamInfo TeamInfo { get; set; }
        public NUserInfo Info
        {
            get { return userInfo; }
        }

        public int CurrentRide = 0;
        public void Ride(int id)
        {
            if(CurrentRide != id)
            {
                CurrentRide = id;
                currentCharacterObject.SendEntityEvent(EntityEvent.Ride, CurrentRide);
            }
            else
            {
                CurrentRide = 0;
                currentCharacterObject.SendEntityEvent(EntityEvent.Ride, 0);
            }
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