using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using ClassicFramework.Mem;

namespace ClassicFramework.AntiWarden
{
    internal static class HookWardenMemScan
    {
        /// <summary>
        ///     Delegate to our c# function we will jmp to
        /// </summary>
        private static WardenMemCpyDelegate _wardenMemCpyDelegate;

        /// <summary>
        ///     Delegate to our c# function we will jmp to
        /// </summary>
        private static WardenPageScanDelegate _wardenPageScanDelegate;

        /// <summary>
        ///     First 5 bytes of Wardens Memscan function
        /// </summary>
        private static readonly byte[] MemScanOriginalBytes = { 0x56, 0x57, 0xFC, 0x8B, 0x54 };

        private static readonly byte[] PageScanOriginalBytes = { 0x8B, 0x45, 0x08, 0x8A, 0x04 };

        /// <summary>
        ///     Is Wardens Memscan modified?
        /// </summary>
        private static IntPtr WardensMemScanFuncPtr = IntPtr.Zero;

        private static IntPtr WardensPageScanFuncPtr = IntPtr.Zero;
        private static IntPtr WardenLuaStrCheckFuncPtr = IntPtr.Zero;
        private static IntPtr WardenMemCpyDetourPtr = IntPtr.Zero;
        private static IntPtr WardenPageScanDetourPtr = IntPtr.Zero;

        private static IntPtr AddrToWardenMemCpy = IntPtr.Zero;
        private static IntPtr AddrToWardenPageScan = IntPtr.Zero;

        /// <summary>
        ///     Delegate to our C# function
        /// </summary>
        private static readonly ModifyWardenDetour _modifyWarden;

        /// <summary>
        ///     A private list to keep track of all hacks registered
        /// </summary>
        private static readonly List<Hack> Hacks = new List<Hack>();

        static HookWardenMemScan()
        {
            Console.WriteLine("HookWardenMemScan created");
            _modifyWarden = DisableWarden;
            // get PTR for our c# function
            var addrToDetour = Marshal.GetFunctionPointerForDelegate(_modifyWarden);
            string[] asmCode =
            {
                "MOV [0xCE8978], EAX",
                "pushfd",
                "pushad",
                "push EAX",
                "call " + addrToDetour,
                "popad",
                "popfd",
                "jmp 0x006CA233"
            };
            var wardenDetour = Memory.InjectAsm(asmCode, "WardenLoadDetour");
            Memory.InjectAsm(0x006CA22E, "jmp " + wardenDetour, "WardenLoadDetourJmp");
        }

        /// <summary>
        ///     Init the hack
        /// </summary>
        [Obfuscation(Feature = "virtualization", Exclude = false)]
        private static void DisableWarden(IntPtr parWardenPtr1)
        {
            //var second = Memory.Reader.Read<IntPtr>(parWardenPtr1);
            var wardenModuleStart = Memory.Reader.Read<IntPtr>(parWardenPtr1);
            Console.WriteLine("WARDEN HERE: " + wardenModuleStart.ToString("X"));
            var memScanPtr = IntPtr.Add(wardenModuleStart, 0x2a7f);
            var pageScanPtr = IntPtr.Add(wardenModuleStart, 0x2B21);
            var luaStrCheckPtr = IntPtr.Add(wardenModuleStart, 0x2ED8);

            if (WardenLuaStrCheckFuncPtr != luaStrCheckPtr)
            {
                WardenLuaStrCheckFuncPtr = luaStrCheckPtr;
                Memory.InjectAsm((uint)pageScanPtr,
                    "jmp 0x0",
                    "WardenLuaStrCheckJmp");
            }

            if (pageScanPtr != WardensPageScanFuncPtr)
            {
                var CurrentBytes = Memory.Reader.ReadBytes(pageScanPtr, 5);
                Console.WriteLine("PageScan: " + pageScanPtr.ToString("X"));
                //var CurrrentBytes = (tmpPtr).ReadAs<Byte>(); //How do I read 5 bytes?
                var isEqual = CurrentBytes.SequenceEqual(PageScanOriginalBytes);
                if (!isEqual) return;

                if (AddrToWardenPageScan == IntPtr.Zero)
                {
                    _wardenPageScanDelegate = WardenPageScanHook;
                    AddrToWardenPageScan = Marshal.GetFunctionPointerForDelegate(_wardenPageScanDelegate);
                    if (WardenPageScanDetourPtr == IntPtr.Zero)
                    {
                        // IntPtr readBase, int readOffset, IntPtr writeTo
                        string[] asmCode =
                        {
                            "mov eax, [ebp+8]", // read base
                            "pushfd",
                            "pushad",
                            "mov ecx, esi",
                            "add ecx, edi",
                            "add ecx, 0x1C",
                            "push ecx",
                            "push edi",
                            "push eax",
                            "call " + (uint) AddrToWardenPageScan,
                            "popad",
                            "popfd",
                            "inc edi",
                            "jmp " + ((uint)wardenModuleStart + 0x2B2C)



                    };
                        WardenPageScanDetourPtr = Memory.InjectAsm(asmCode, "WardenPageScanDetour");
                    }
                }

                Memory.InjectAsm((uint)pageScanPtr,
                    "jmp 0x" + WardenPageScanDetourPtr.ToString("X"),
                    "WardenPageScanJmp");
                WardensPageScanFuncPtr = pageScanPtr;
            }

            if (memScanPtr != WardensMemScanFuncPtr)
            {
                Console.WriteLine("MemScan: " + memScanPtr.ToString("X"));
                var CurrentBytes = Memory.Reader.ReadBytes(memScanPtr, 5);
                //var CurrrentBytes = (tmpPtr).ReadAs<Byte>(); //How do I read 5 bytes?
                var isEqual = CurrentBytes.SequenceEqual(MemScanOriginalBytes);
                if (!isEqual) return;

                if (AddrToWardenMemCpy == IntPtr.Zero)
                {
                    _wardenMemCpyDelegate = WardenMemCpyHook;
                    AddrToWardenMemCpy = Marshal.GetFunctionPointerForDelegate(_wardenMemCpyDelegate);

                    if (WardenMemCpyDetourPtr == IntPtr.Zero)
                    {
                        string[] asmCodeOnline =
                        {
                            "PUSH ESI",
                            "PUSH EDI",
                            "CLD",
                            "MOV EDX, [ESP+20]",
                            "MOV ESI, [ESP+16]",
                            "MOV EAX, [ESP+12]",
                            "MOV ECX, EDX",
                            "MOV EDI, EAX",
                            "pushfd",
                            "pushad",
                            "PUSH EDI",
                            "PUSH ECX",
                            "PUSH ESI",
                            "call " + (uint)AddrToWardenMemCpy ,
                            "popad",
                            "popfd",
                            "POP EDI",
                            "POP ESI",
                            "jmp " +  (uint) (memScanPtr + 0x24)
                        };
                        WardenMemCpyDetourPtr = Memory.InjectAsm(asmCodeOnline, "WardenMemCpyDetour");
                    }
                }

                Memory.InjectAsm((uint)memScanPtr, "jmp 0x" + WardenMemCpyDetourPtr.ToString("X"), "WardenMemCpyJmp");
                WardensMemScanFuncPtr = memScanPtr;
            }
        }

        /// <summary>
        ///     add a hack to the list from the outside
        ///     hack contains: original bytes, bytes we inject, the address we inject to
        /// </summary>
        internal static void AddHack(Hack parHack)
        {
            if (Hacks.All(i => i.Address != parHack.Address))
            {
                RemoveHack(parHack.Name);
                Hacks.Add(parHack);
            }
        }

        internal static void RemoveHack(string parName)
        {
            var hack = Hacks.Where(i => i.Name == parName).ToList();
            foreach (var x in hack)
                x.Remove();
            Hacks.RemoveAll(i => i.Name == parName);
        }

        internal static void RemoveHack(IntPtr parAddress)
        {
            var hack = Hacks.Where(i => i.Address == parAddress).ToList();
            foreach (var x in hack)
                x.Remove();
            Hacks.RemoveAll(i => i.Address == parAddress);
        }

        internal static Hack GetHack(string parName)
        {
            return Hacks.FirstOrDefault(i => i.Name == parName);
        }

        internal static Hack GetHack(IntPtr parAddress)
        {
            return Hacks.FirstOrDefault(i => i.Address == parAddress);
        }


        private static void WardenPageScanHook(IntPtr readBase, int readOffset, IntPtr writeTo)
        {
            var readByteFrom = readBase + readOffset;

            var activeHacks = Hacks.Where(x => x.IsActivated && x.IsWithinScan(readByteFrom, 1)).ToList();
            activeHacks.ForEach(x =>
            {
                x.Remove();
                Console.WriteLine($@"[PageScan] Disabling {x.Name} at {x.Address.ToString("X")}");
            });
            var myByte = Memory.Reader.Read<byte>(readByteFrom);
            Memory.Reader.Write(writeTo, myByte);

            activeHacks.ForEach(x => x.Apply());
        }

        /// <summary>
        ///     Will be called from our ASM stub
        ///     will check if the scanned addr range contains any registered hack
        ///     if yes: restore original byte for the hack
        ///     do the scan
        ///     restore back to the "hacked" state
        /// </summary>
        private static void WardenMemCpyHook(IntPtr addr, int size, IntPtr bufferStart)
        {
            if (size != 0)
            {
                if ((int)addr <= 0x00CEEF74 && (int)addr + size > 0x00CEEF74)
                {
                    Environment.Exit(0);
                }

                // LINQ to get all affected hacks
                var match = Hacks
                    .Where(i => i.Address.ToInt32() <= IntPtr.Add(addr, size).ToInt32()
                                && i.Address.ToInt32() >= addr.ToInt32())
                    .ToList();

                var ActiveHacks = new List<Hack>();
                foreach (var x in match)
                {
                    if (!x.IsActivated) continue;
                    ActiveHacks.Add(x);
                    x.Remove();
                }
                // Do the memscan
                Memory.Reader.WriteBytes(bufferStart, Memory.Reader.ReadBytes(addr, size));
                // reapply
                ActiveHacks.ForEach(i => i.Apply());
            }
        }


        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void ModifyWardenDetour(IntPtr parWardenPtr);

        /// <summary>
        ///     Delegate for our c# function
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void WardenMemCpyDelegate(IntPtr addr, int size, IntPtr bufferStart);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void WardenPageScanDelegate(IntPtr readBase, int readOffset, IntPtr writeTo);
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.InteropServices;
//using ClassicFramework.Mem;

//namespace ClassicFramework.AntiWarden
//{
//    internal static class HookWardenMemScan
//    {
//        /// <summary>
//        ///     Delegate to our c# function we will jmp to
//        /// </summary>
//        private static WardenMemCpyDelegate _wardenMemCpyDelegate;

//        /// <summary>
//        ///     First 5 bytes of Wardens Memscan function
//        /// </summary>
//        private static readonly byte[] OriginalBytes = { 0x56, 0x57, 0xFC, 0x8B, 0x54 };

//        /// <summary>
//        ///     Is Wardens Memscan modified?
//        /// </summary>
//        private static IntPtr WardensMemScanFuncPtr = IntPtr.Zero;

//        private static IntPtr WardenMemCpyDetourPtr = IntPtr.Zero;
//        private static IntPtr AddrToWardenDetour = IntPtr.Zero;

//        /// <summary>
//        ///     Delegate to our C# function
//        /// </summary>
//        private static readonly ModifyWardenDetour _modifyWarden;

//        /// <summary>
//        ///     A private list to keep track of all hacks registered
//        /// </summary>
//        private static readonly List<Hack> Hacks = new List<Hack>();

//        static HookWardenMemScan()
//        {
//            Console.WriteLine("HookWardenMemScan created");
//            _modifyWarden = DisableWarden;
//            // get PTR for our c# function
//            var addrToDetour = Marshal.GetFunctionPointerForDelegate(_modifyWarden);
//            string[] asmCode =
//            {
//                "MOV [0xCE8978], EAX",
//                "pushfd",
//                "pushad",
//                "push EAX",
//                "call " + (uint) addrToDetour,
//                "popad",
//                "popfd",
//                "jmp 0x006CA233"
//            };
//            var WardenDetour = Memory.InjectAsm(asmCode, "WardenLoadDetour");
//            Memory.InjectAsm(0x006CA22E, "jmp " + WardenDetour, "WardenLoadDetourJmp");
//        }

//        /// <summary>
//        ///     Init the hack
//        /// </summary>
//        [Obfuscation(Feature = "virtualization", Exclude = false)]
//        private static void DisableWarden(IntPtr parWardenPtr1)
//        {
//            var second = Memory.Reader.Read<IntPtr>(parWardenPtr1);
//            var tmpPtr = IntPtr.Add(second, 0x2a7f);
//            if (tmpPtr == WardensMemScanFuncPtr) return;

//            var CurrentBytes = Memory.Reader.ReadBytes(tmpPtr, 5);
//            var isEqual = CurrentBytes.SequenceEqual(OriginalBytes);
//            if (!isEqual) return;

//            if (AddrToWardenDetour == IntPtr.Zero)
//            {
//                _wardenMemCpyDelegate = WardenMemCpyHook;
//                AddrToWardenDetour = Marshal.GetFunctionPointerForDelegate(_wardenMemCpyDelegate);

//                if (WardenMemCpyDetourPtr == IntPtr.Zero)
//                {
//                    string[] asmCodeOnline =
//                    {
//                        "PUSH ESI",
//                        "PUSH EDI",
//                        "CLD",
//                        "MOV EDX, [ESP+20]",
//                        "MOV ESI, [ESP+16]",
//                        "MOV EAX, [ESP+12]",
//                        "MOV ECX, EDX",
//                        "MOV EDI, EAX",
//                        "pushfd",
//                        "pushad",
//                        "PUSH EDI",
//                        "PUSH ECX",
//                        "PUSH ESI",
//                        "call " + (uint) AddrToWardenDetour,
//                        "popad",
//                        "popfd",
//                        "POP EDI",
//                        "POP ESI",
//                        "jmp " + (uint) (tmpPtr + 0x24)
//                    };
//                    WardenMemCpyDetourPtr = Memory.InjectAsm(asmCodeOnline, "WardenMemCpyDetour");
//                }
//            }

//            Memory.InjectAsm((uint)tmpPtr, "jmp 0x" + WardenMemCpyDetourPtr.ToString("X"), "WardenMemCpyJmp");
//            WardensMemScanFuncPtr = tmpPtr;
//            Console.WriteLine("Warden's Memscan address: "  + WardensMemScanFuncPtr.ToString("X"));
//        }

//        /// <summary>
//        ///     add a hack to the list from the outside
//        ///     hack contains: original bytes, bytes we inject, the address we inject to
//        /// </summary>
//        internal static void AddHack(Hack parHack)
//        {
//            if (Hacks.All(i => i.address != parHack.address))
//            {
//                RemoveHack(parHack.Name);
//                Hacks.Add(parHack);
//            }
//        }

//        internal static void RemoveHack(string parName)
//        {
//            var hack = Hacks.Where(i => i.Name == parName).ToList();
//            foreach (var x in hack)
//            {
//                x.Remove();
//            }
//            Hacks.RemoveAll(i => i.Name == parName);
//        }

//        internal static void RemoveHack(IntPtr parAddress)
//        {
//            var hack = Hacks.Where(i => i.address == parAddress).ToList();
//            foreach (var x in hack)
//            {
//                x.Remove();
//            }
//            Hacks.RemoveAll(i => i.address == parAddress);
//        }

//        internal static Hack GetHack(string parName)
//        {
//            return Hacks.FirstOrDefault(i => i.Name == parName);
//        }

//        internal static Hack GetHack(IntPtr parAddress)
//        {
//            return Hacks.FirstOrDefault(i => i.address == parAddress);
//        }

//        /// <summary>
//        ///     Will be called from our ASM stub
//        ///     will check if the scanned addr range contains any registered hack
//        ///     if yes: restore original byte for the hack
//        ///     do the scan
//        ///     restore back to the "hacked" state
//        /// </summary>
//        private static void WardenMemCpyHook(IntPtr addr, int size, IntPtr bufferStart)
//        {
//            if (size != 0)
//            {
//                // LINQ to get all affected hacks
//                var match = Hacks
//                    .Where(i => i.address.ToInt32() <= IntPtr.Add(addr, size).ToInt32()
//                                && i.address.ToInt32() >= addr.ToInt32())
//                    .ToList();

//                var ActiveHacks = new List<Hack>();
//                foreach (var x in match)
//                {
//                    if (!x.IsActivated) continue;
//                    Console.WriteLine($"Scan at {addr.ToString("X")}. Hiding {x.Name}");
//                    ActiveHacks.Add(x);
//                    x.Remove();
//                }
//                // Do the memscan
//                Memory.Reader.WriteBytes(bufferStart, Memory.Reader.ReadBytes(addr, size));
//                // reapply
//                ActiveHacks.ForEach(i => i.Apply());
//            }
//        }


//        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
//        private delegate void ModifyWardenDetour(IntPtr parWardenPtr);

//        /// <summary>
//        ///     Delegate for our c# function
//        /// </summary>
//        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
//        private delegate void WardenMemCpyDelegate(IntPtr addr, int size, IntPtr bufferEnd);
//    }
//}