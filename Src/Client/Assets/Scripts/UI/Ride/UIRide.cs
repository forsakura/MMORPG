using Assets.Scripts.Models;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Ride
{
    public class UIRide : UIWindow
    {
        public UnityEngine.GameObject uiPrefab;
        public Text RideInfo;
        public ListView.ListView mainView;
        UIRideItem selectedItem;
        void Start()
        {
            mainView.onItemSelected += OnItemSelected;
            RefreshUI();
        }

        void OnItemSelected(ListView.ListView.ListViewItem item)
        {
            selectedItem = item as UIRideItem;
            if (RideInfo != null)
            {
                RideInfo.text = selectedItem.ride.Description;
            }
            foreach (var ride in mainView.items)
            {
                ride.Selected = ride == item;
            }
        }

        private void RefreshUI()
        {
            ClearList();
            InitRides();
        }

        private void ClearList()
        {
            mainView.RemoveAll();
        }

        private void InitRides()
        {
            foreach (var kv in ItemManager.Instance.items)
            {
                if (kv.Value.itemDefine.Type == SkillBridge.Message.ItemType.Ride && (kv.Value.rideDefine.LimitClass == SkillBridge.Message.CharacterClass.None || kv.Value.rideDefine.LimitClass == User.Instance.currentCharacter.Class))
                {
                    var go = Instantiate(uiPrefab, mainView.transform);
                    var ui = go.GetComponent<UIRideItem>();
                    ui.SetInfo(kv.Value.rideDefine);
                    mainView.AddItem(ui);
                }
            }
        }

        public void DoRide()
        {
            if(selectedItem == null)
            {
                MessageBox.Show("请选择要召唤的坐骑", "提示");
                return;
            }
            User.Instance.Ride(selectedItem.ride.ID);
        }
    }
}