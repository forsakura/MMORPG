using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BagItem
    {
        public ushort itemID;
        public ushort count;
        public static BagItem zero = new BagItem { itemID = 0, count = 0 };

        public BagItem(int itemID, int count)
        {
            this.itemID = (ushort)itemID;
            this.count = (ushort)count;
        }

        public static bool operator ==(BagItem lhs, BagItem rhs)
        {
            return lhs.itemID == rhs.itemID && lhs.count == rhs.count;
        }

        public static bool operator !=(BagItem lhs, BagItem rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            if (obj is BagItem)
            {
                return Equals((BagItem)obj);
            }
            return false;
        }

        public bool Equals(BagItem other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return itemID.GetHashCode() ^ (count.GetHashCode() << 2);
        }
    }
}