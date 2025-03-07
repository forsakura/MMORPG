using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.ListView
{
    public class ListView : MonoBehaviour
    {
        public UnityAction<ListViewItem> onItemSelected;
        public class ListViewItem : MonoBehaviour, IPointerClickHandler
        {
            bool selected;
            public bool Selected
            {
                get { return selected; }
                set
                {
                    selected = value;
                    OnSelected(selected);
                }
            }

            public virtual void OnSelected(bool selected)
            {
                
            }

            public ListView owner;

            public void OnPointerClick(PointerEventData eventData)
            {
                if(!selected)
                {
                    Selected = true;
                }
                else
                {
                    if(owner != null && owner.SelectedItem != this)
                    {
                        owner.SelectedItem = this;
                    }
                }
            }
        }

        List<ListViewItem> items = new List<ListViewItem>();
        ListViewItem selectedItem = null;
        public ListViewItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                if (onItemSelected != null)
                {
                    onItemSelected.Invoke(value);
                }
            }
        }
        public void AddItem(ListViewItem item)
        {
            item.owner = this;
            items.Add(item);
        }

        public void RemoveAll()
        {
            foreach (var item in items)
            {
                Destroy(item.gameObject);
            }
            items.Clear();
        }
    }
}