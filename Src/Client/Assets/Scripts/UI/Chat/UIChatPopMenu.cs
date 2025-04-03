using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Chat
{
    public class UIChatPopMenu : UIWindow, IDeselectHandler
    {
        public int targteId;
        public string targetName;
        public void OnDeselect(BaseEventData eventData)
        {
            var ed = eventData as PointerEventData;
            if (ed.hovered.Contains(this.gameObject))
                return;
            this.Close(WindowResult.None);
        }

        public void OnEnable()
        {
            this.GetComponent<Selectable>().Select();
            this.Root.transform.position = Input.mousePosition + new Vector3(80, 0, 0);
        }

        public void OnClickChat()
        {
            Debug.Log("ClickChat");
            ChatManager.Instance.StartPrivateChat(targteId, targetName);
            this.Close(WindowResult.None);
        }

        public void OnClickFriendAdd()
        {
            if (FriendManager.Instance.HasFriend(targteId))
            {
                MessageBox.Show(string.Format("已和{0}是好友", targetName), "好友", MessageBoxType.Information, "确定");
                return;
            }
            MessageBox.Show(string.Format("确定添加玩家{0}吗？", targetName), "好友", MessageBoxType.Information, "确定", "取消").OnYes = () =>
            {
                FriendService.Instance.SendFriendAddRequest(targteId, targetName);
            };
            this.Close(WindowResult.None);
        }

        public void OnClickTeamInvite()
        {
            if(TeamManager.Instance.IsFull)
            {
                MessageBox.Show("您的队伍已满", "组队", MessageBoxType.Information, "确定");
                return;
            }
            if (TeamManager.Instance.HasTeamMember(targteId))
            {
                MessageBox.Show(string.Format("玩家{0}已在队伍中", targetName), "组队", MessageBoxType.Information, "确定");
                return;
            }
            MessageBox.Show(string.Format("确定与玩家{0}组队吗？", targetName), "组队", MessageBoxType.Information, "确定", "取消").OnYes = () =>
            {
                TeamService.Instance.SendTeamInviteRequest(targteId, targetName);
            };            
            this.Close(WindowResult.None);
        }
    }
}