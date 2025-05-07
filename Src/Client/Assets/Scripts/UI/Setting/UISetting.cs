using Assets.Scripts.UI.Chat;
using Services;

namespace Assets.Scripts.UI.Setting
{
    public class UISetting : UIWindow
    {
        public void OnClickSelectChar()
        {
            SceneManager.Instance.LoadScene("CharacterSelect");
            UserService.Instance.SendGameLeave(false);
            this.Close(WindowResult.None);
        }

        public void OnClickSoundConfig()
        {
            UIManager.Instance.Show<UISoundConfig>();
            this.Close(WindowResult.None);
        }

        public void OnClickExit()
        {
            UserService.Instance.SendGameLeave(true);
        }
    }
}