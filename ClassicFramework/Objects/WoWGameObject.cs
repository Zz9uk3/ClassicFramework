using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassicFramework.Mem;

namespace ClassicFramework.Objects
{
    internal class WoWGameObject : WoWObject
    {
        /// <summary>
        /// Constructor taking guid aswell ptr to object
        /// </summary>
        internal WoWGameObject(UInt64 parGuid, IntPtr parPointer)
            : base(parGuid, parPointer)
        {
            
        }

        /// <summary>
        /// Position of object
        /// </summary>
        internal override Vector3 Position
        {
            get
            {
                float X = base.GetDescriptor<float>(0x3C);
                float Y = base.GetDescriptor<float>(0x3C + 4);
                float Z = base.GetDescriptor<float>(0x3C + 8);
                return new Vector3(X, Y, Z);
            }
        }

        /// <summary>
        /// Distance to object
        /// </summary>
        internal float DistanceTo(WoWObject parOtherObject)
        {
            return this.Position.DistanceTo(parOtherObject.Position);
        }

        /// <summary>
        /// Name of object
        /// </summary>
        internal override string Name
        {
            get
            {
                IntPtr ptr1 = base.ReadRelative<IntPtr>(0x214);
                IntPtr ptr2 = Memory.Reader.Read<IntPtr>(IntPtr.Add(ptr1, 0x8));
                return Memory.Reader.ReadString(ptr2, Encoding.ASCII, 30);
            }
        }

        internal void Interact(bool parAutoLoot)
        {
            Functions.OnRightClickObject(base.Pointer, Convert.ToInt32(parAutoLoot));
        }
    }
}
