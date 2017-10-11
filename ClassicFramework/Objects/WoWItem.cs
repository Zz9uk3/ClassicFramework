using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassicFramework.Mem;

namespace ClassicFramework.Objects
{
    internal class WoWItem : WoWObject
    {
        /// <summary>
        /// Constructor taking guid aswell ptr to object
        /// </summary>
        internal WoWItem(UInt64 parGuid, IntPtr parPointer) : base(parGuid, parPointer)
        {
        }

        /// <summary>
        /// Pointer to WDB cache
        /// </summary>
        private IntPtr ItemCachePointer
        {
            get
            {
                return Functions.ItemCacheGetRow(ItemId);
            }
        }

        /// <summary>
        /// Pointer to Cache element for UseItem
        /// </summary>
        internal IntPtr UseItemPointer
        {
            get
            {
                return IntPtr.Add(ItemCachePointer, 0xABE8);
            }
        }

        /// <summary>
        /// Get Item ID
        /// </summary>
        private int ItemId
        {
            get
            {
                return base.GetDescriptor<int>(0xC);
            }
        }

        /// <summary>
        /// Durability
        /// </summary>
        internal int Durability
        {
            get
            {
                return base.GetDescriptor<int>(0xB8);
            }
        }

        internal int MaxDurability
        {
            get
            {
                return base.GetDescriptor<int>(0xBC);
            }
        }

        internal int DurabilityPercent
        {
            get
            {
                return (int)((float)((float)Durability / (float)MaxDurability) * 100);
            }
        }

        /// <summary>
        /// Stack count
        /// </summary>
        internal int StackCount
        {
            get
            {
                return base.GetDescriptor<int>(0x38);
            }
        }

        /// <summary>
        /// Item quality | 0 = grey
        /// </summary>
        internal int Quality
        {
            get
            {
                return Memory.Reader.Read<int>(IntPtr.Add(ItemCachePointer, 0x1C));
            }
        }

        /// <summary>
        /// Item name
        /// </summary>
        internal override string Name
        {
            get
            {
                IntPtr ptr = Memory.Reader.Read<IntPtr>(IntPtr.Add(ItemCachePointer, 0x8));
                return Memory.Reader.ReadString(ptr, Encoding.ASCII);
            }
        }

        /// <summary>
        /// 0, 0, 0 cause items have no coordinates
        /// </summary>
        internal override Vector3 Position
        {
            get
            {
                return new Vector3(0, 0, 0);
            }
        }

        /// <summary>
        /// total slots of item
        /// </summary>
        internal int Slots
        {
            get
            {
                return base.ReadRelative<int>(0x6c8);
            }
        }

        public override string ToString()
        {
            return Name + "-> Stackcount: " + StackCount + " Durability: " + Durability + " Quality: " + Quality;
        }
    }
}
