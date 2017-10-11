using ClassicFramework.LuaFunctions.CustomFuncs;
using ClassicFramework.Mem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassicFramework.LuaFunctions
{
    internal static class UnregisterFunctionHook
    {
        /// <summary>
        /// Delegate for our c# function
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void FuncDelegate(string parFuncName);

        /// <summary>
        /// Delegate to our c# function we will jmp to
        /// </summary>
        private static FuncDelegate _FuncDelegate;

        internal static void Unregister(string parFuncName)
        {
            if (parFuncName != "GetPastDrawResult") return;
            uint CustomFuncTramps = 0x0047FA7D;

            CustomFuncs.About.Remove(ref CustomFuncTramps);
            CustomFuncs.SuperFly.Remove(ref CustomFuncTramps);
            CustomFuncs.AddToZ.Remove(ref CustomFuncTramps);
            CustomFuncs.Commands.Remove(ref CustomFuncTramps);
            Wallclimb.Remove(ref CustomFuncTramps);

            CustomFuncTramps = 0x0047FA99;

            NoClip1.Remove(ref CustomFuncTramps);
            NoClip2.Remove(ref CustomFuncTramps);
            NoClip3.Remove(ref CustomFuncTramps);
            NoClip4.Remove(ref CustomFuncTramps);
            Loot.Remove(ref CustomFuncTramps);
            UseItem.Remove(ref CustomFuncTramps);
            UseSpell.Remove(ref CustomFuncTramps);
            UseObject.Remove(ref CustomFuncTramps);
            Interact.Remove(ref CustomFuncTramps);
            Thanks.Remove(ref CustomFuncTramps);
            AutoLoot.Remove(ref CustomFuncTramps);
            SaveBars.Remove(ref CustomFuncTramps);

            CustomFuncTramps = 0x0047FA61;

            LoadBars.Remove(ref CustomFuncTramps);
            RemoveBars.Remove(ref CustomFuncTramps);


            Memory.GetHack("Superfly").Remove();
            Memory.GetHack("Antijump").Remove();
            SuperFly.enabled = false;
        }

        internal static void Init()
        {
            _FuncDelegate = Unregister;
            IntPtr addrToRegisterFunc = Marshal.GetFunctionPointerForDelegate(_FuncDelegate);

            string[] detour = new string[]
                {
                    "pushfd",
                    "pushad",

                    "push ecx",
                    "call " + addrToRegisterFunc,

                    "popad",
                    "popfd",


                    "PUSH ESI",
                    "PUSH EDI",
                    "MOV EDI,ECX",
                    "CALL 0x007040D0",

                    "jmp 0x00704169",
                };
            IntPtr detourPtr = Memory.InjectAsm(detour, "UnregisterFunctionDetour");

            byte[] orig = Memory.Reader.ReadBytes((IntPtr)0x00704160, 5);
            Memory.InjectAsm(0x00704160, "jmp " + detourPtr, "UnregisterFunctionJmp");
        }
    }
}
