using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassicFramework.Mem;

namespace ClassicFramework.Objects
{
    internal class WoWPlayer : WoWUnit
    {
        /// <summary>
        /// Constructor taking guid aswell ptr to object
        /// </summary>
        internal WoWPlayer(UInt64 parGuid, IntPtr parPointer)
            : base(parGuid, parPointer)
        {
            
        }

        /// <summary>
        /// Name of object
        /// </summary>
        internal override string Name
        {
            get
            {
                ulong nextGuid = 0;
                IntPtr nameBasePtr = Memory.Reader.Read<IntPtr>(new IntPtr(0xC0E230));
                while (true)
                {
                    nextGuid = Memory.Reader.Read<ulong>(IntPtr.Add(nameBasePtr, 0xc));
                    if (nextGuid == 0)
                    {
                        return "";
                    }
                    else if (nextGuid != base.Guid)
                    {
                        nameBasePtr = Memory.Reader.Read<IntPtr>(nameBasePtr);
                    }
                    else
                    {
                        break;
                    }
                }
                return Memory.Reader.ReadString(IntPtr.Add(nameBasePtr, 0x14), Encoding.ASCII, 30);
            }
        }
    }
}
