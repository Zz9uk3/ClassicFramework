using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassicFramework.Mem;

namespace ClassicFramework.Objects
{
    internal class WoWObject
    {
        

        /// <summary>
        /// Constructor taking guid aswell ptr to object
        /// </summary>
        internal WoWObject(UInt64 parGuid, IntPtr parPointer)
        {
            Guid = parGuid;
            Pointer = parPointer;

            UInt64 guid = Guid;
            if (Guid == 0) PackedGuid = new byte[] { 0 };
            else
            {
                var packedGuid = new List<byte> { 0 };
                for (var i = 0; guid != 0; i++)
                {
                    if ((guid & 0xFF) != 0)
                    {
                        packedGuid[0] |= (byte)(1 << i);
                        packedGuid.Add((byte)(guid & 0xFF));
                    }
                    guid >>= 8;
                }
                PackedGuid = packedGuid.ToArray();
            }
        }

        /// <summary>
        /// Pointer + Guid
        /// </summary>
        internal IntPtr Pointer { get; set; }
        internal ulong Guid { get; set; }

        internal byte[] PackedGuid
        {
            get;
            set;
        }

        /// <summary>
        /// Get descriptor function to avoid some code
        /// </summary>
        internal T GetDescriptor<T>(int descriptor) where T : struct
        {
            uint ptr = Memory.Reader.Read<uint>(IntPtr.Add(Pointer, 0x8));
            return Memory.Reader.Read<T>(new IntPtr(ptr + descriptor));
        }

        internal void SetDescriptor<T>(int descriptor, T parValue) where T : struct
        {
            uint ptr = Memory.Reader.Read<uint>(IntPtr.Add(Pointer, 0x8));
            Memory.Reader.Write<T>(new IntPtr(ptr + descriptor), parValue);
        }

        /// <summary>
        /// Read relative to base pointer
        /// </summary>
        internal T ReadRelative<T>(int offset) where T : struct
        {
            return Memory.Reader.Read<T>(IntPtr.Add(Pointer, offset));
        }

        /// <summary>
        /// Position of object
        /// </summary>
        internal virtual Vector3 Position
        {
            get;
            set;
        }

        /// <summary>
        /// Name of object
        /// </summary>
        internal virtual string Name
        {
            get;
            set;
        }
    }
}
