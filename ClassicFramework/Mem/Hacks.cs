using System;
using System.Linq;

namespace ClassicFramework.Mem
{
    /// <summary>
    ///     Class for a simple hack (read: changing bytes in memory)
    /// </summary>
    internal class Hack
    {
        // address where the bytes will be changed
        private IntPtr _address = IntPtr.Zero;

        // old bytes
        private byte[] _originalBytes;

        /// <summary>
        ///     Constructor: addr and the new bytes
        /// </summary>
        internal Hack(IntPtr parAddress, byte[] parCustomBytes, string parName)
        {
            Address = parAddress;
            CustomBytes = parCustomBytes;
            _originalBytes = Memory.Reader.ReadBytes(Address, CustomBytes.Length);
            Name = parName;
        }

        internal Hack(bool parRelativeToPlayer, uint offset, byte[] parCustomBytes, string parName)
        {
            RelativeToPlayerBase = parRelativeToPlayer;
            _address = (IntPtr)offset;
            CustomBytes = parCustomBytes;
            Name = parName;
        }

        /// <summary>
        ///     Constructor: addr, new bytes aswell old bytes
        /// </summary>
        internal Hack(IntPtr parAddress, byte[] parCustomBytes, byte[] parOriginalBytes, string parName)
        {
            Address = parAddress;
            CustomBytes = parCustomBytes;
            _originalBytes = parOriginalBytes;
            Name = parName;
        }

        internal Hack(uint offset, byte[] parCustomBytes, string parName)
        {
            _address = (IntPtr)offset;
            CustomBytes = parCustomBytes;
            Name = parName;
        }

        internal bool DynamicHide { get; set; } = false;
        // is the hack applied
        //internal bool IsApplied { get; private set; }

        internal bool RelativeToPlayerBase { get; set; } = false;

        internal IntPtr Address
        {
            get
            {
                return !RelativeToPlayerBase
                    ? _address
                    : IntPtr.Add(ObjectManager.Player.Pointer, (int)_address);
            }
            private set { _address = value; }
        }

        // new bytes
        public byte[] CustomBytes { get; set; }
        // name of hack
        internal string Name { get; private set; }

        internal bool IsActivated
        {
            get
            {
                if (RelativeToPlayerBase)
                {
                    if (ObjectManager.Player == null) return false;
                }
                var curBytes = Memory.Reader.ReadBytes(Address, _originalBytes.Length);
                return !curBytes.SequenceEqual(_originalBytes);
            }
        }

        internal bool IsWithinScan(IntPtr scanStartAddress, int size)
        {
            var scanStart = (int)scanStartAddress;
            var scanEnd = (int)IntPtr.Add(scanStartAddress, size);

            var hackStart = (int)Address;
            var hackEnd = (int)Address + CustomBytes.Length;

            if (hackStart >= scanStart && hackStart < scanEnd)
                return true;

            if (hackEnd > scanStart && hackEnd <= scanEnd)
                return true;

            return false;
        }

        /// <summary>
        ///     Apply the new bytes to address
        /// </summary>
        internal void Apply()
        {
            if (RelativeToPlayerBase)
            {
                if (ObjectManager.Player == null) return;
                if (_originalBytes == null)
                    _originalBytes = Memory.Reader.ReadBytes(Address, CustomBytes.Length);
            }
            Memory.Reader.WriteBytes(Address, CustomBytes);
        }

        /// <summary>
        ///     Restore the old bytes to the address
        /// </summary>
        internal void Remove()
        {
            if (RelativeToPlayerBase)
            {
                if (ObjectManager.Player == null) return;
            }
            if (DynamicHide && IsActivated)
                CustomBytes = Memory.Reader.ReadBytes(Address, _originalBytes.Length);
            Memory.Reader.WriteBytes(Address, _originalBytes);
        }
    }
}

//using ClassicFramework.Helpers;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace ClassicFramework.Mem
//{
//    /// <summary>
//    /// Class for a simple hack (read: changing bytes in memory)
//    /// </summary>
//    internal class Hack
//    {
//        // address where the bytes will be changed
//        private IntPtr _address = IntPtr.Zero;

//        internal IntPtr address 
//        { 
//            get 
//            {
//                if (!RelativeToPlayerBase)
//                {
//                    return _address;
//                }
//                else
//                {
//                    return IntPtr.Add(ObjectManager.Player.Pointer, (int)_address);
//                }
//            }
//            private set 
//            {
//                _address = value;
//            } 
//        }
//        // old bytes
//        internal byte[] OriginalBytes { get; set; }
//        // new bytes
//        internal byte[] CustomBytes { get; set; }
//        // name of hack
//        internal string Name { get; private set; }
//        // is the hack applied
//        internal bool IsApplied { get; private set; }

//        internal bool RelativeToPlayerBase = false;

//        internal string OriginalBytesToString
//        {
//            get
//            {
//                string str = "";
//                foreach (byte b in OriginalBytes)
//                {
//                    str += "0x" + b.ToString("X2") + ", ";
//                }
//                str = str.Trim().TrimEnd(',');
//                return str;
//            }
//        }

//        internal string CustomBytesToString
//        {
//            get
//            {
//                string str = "";
//                foreach (byte b in CustomBytes)
//                {
//                    str += "0x" + b.ToString("X2") + ", ";
//                }
//                str = str.Trim().TrimEnd(',');
//                return str;
//            }
//        }

//        /// <summary>
//        /// Constructor: addr and the new bytes
//        /// </summary>
//        internal Hack(IntPtr parAddress, byte[] parCustomBytes, string parName)
//        {
//            address = parAddress;
//            CustomBytes = parCustomBytes;
//            OriginalBytes = Memory.Reader.ReadBytes(address, CustomBytes.Length);
//            Name = parName;
//        }

//        internal bool IsActivated
//        {
//            get
//            {
//                if (RelativeToPlayerBase)
//                {
//                    if (!ObjectManager.EnumObjects()) return false;
//                    if (ObjectManager.Player == null) return false;
//                }
//                byte[] curBytes = Memory.Reader.ReadBytes(address, OriginalBytes.Length);
//                return !curBytes.SequenceEqual(OriginalBytes);
//            }
//        }

//        /// <summary>
//        /// Constructor: addr, new bytes aswell old bytes
//        /// </summary>
//        internal Hack(IntPtr parAddress, byte[] parCustomBytes, byte[] parOriginalBytes, string parName)
//        {
//            address = parAddress;
//            CustomBytes = parCustomBytes;
//            OriginalBytes = parOriginalBytes;
//            Name = parName;
//        }

//        internal Hack(bool parRelativeToPlayer, uint offset, byte[] parCustomBytes, string parName)
//        {
//            RelativeToPlayerBase = parRelativeToPlayer;
//            _address = (IntPtr)offset;
//            CustomBytes = parCustomBytes;
//            Name = parName;
//        }

//        /// <summary>
//        /// Apply the new bytes to address
//        /// </summary>
//        internal void Apply()
//        {
//            if (RelativeToPlayerBase)
//            {
//                if (!ObjectManager.EnumObjects()) return;
//                if (ObjectManager.Player == null) return;
//                if (OriginalBytes == null)
//                {
//                    OriginalBytes = Memory.Reader.ReadBytes(address, CustomBytes.Length);
//                }
//            }
//            Memory.Reader.WriteBytes(address, CustomBytes);
//        }

//        /// <summary>
//        /// Restore the old bytes to the address
//        /// </summary>
//        internal void Remove()
//        {
//            if (RelativeToPlayerBase)
//            {
//                if (!ObjectManager.EnumObjects()) return;
//                if (ObjectManager.Player == null) return;
//            }
//            Memory.Reader.WriteBytes(address, OriginalBytes);
//        }
//    }
//}
