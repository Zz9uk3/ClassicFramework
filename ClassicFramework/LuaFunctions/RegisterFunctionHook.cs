using ClassicFramework.LuaFunctions.CustomFuncs;
using ClassicFramework.Mem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassicFramework.LuaFunctions
{
    internal static class RegisterFunctionHook
    {
        /// <summary>
        /// Delegate for our c# function
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void FuncDelegate();

        /// <summary>
        /// Delegate to our c# function we will jmp to
        /// </summary>
        private static FuncDelegate _FuncDelegate;

        internal static void Register()
        {
            FunctionHandling.ClearDelegates();
            uint CustomFuncTramps = 0x0047FA7D;

            CustomFuncs.About.Init(ref CustomFuncTramps);
            CustomFuncs.SuperFly.Init(ref CustomFuncTramps);
            CustomFuncs.AddToZ.Init(ref CustomFuncTramps);
            CustomFuncs.Commands.Init(ref CustomFuncTramps);
            Wallclimb.Init(ref CustomFuncTramps);

            CustomFuncTramps = 0x0047FA99;

            NoClip1.Init(ref CustomFuncTramps);
            NoClip2.Init(ref CustomFuncTramps);
            NoClip3.Init(ref CustomFuncTramps);
            NoClip4.Init(ref CustomFuncTramps);
            Loot.Init(ref CustomFuncTramps);
            UseItem.Init(ref CustomFuncTramps);
            UseSpell.Init(ref CustomFuncTramps);
            UseObject.Init(ref CustomFuncTramps);
            Interact.Init(ref CustomFuncTramps);
            Thanks.Init(ref CustomFuncTramps);
            AutoLoot.Init(ref CustomFuncTramps);
            SaveBars.Init(ref CustomFuncTramps);

            CustomFuncTramps = 0x0047FA61;

            LoadBars.Init(ref CustomFuncTramps);
            RemoveBars.Init(ref CustomFuncTramps);

            DirectX.FirstRun = 1;
        }

        internal static void Init()
        {
            _FuncDelegate = Register;
            IntPtr addrToRegisterFunc = Marshal.GetFunctionPointerForDelegate(_FuncDelegate);

            string[] detour = new string[]
                {
                    "pushfd",
                    "pushad",

                    "cmp edx, 0x004C4690",
                    "jne @outRegister",

                    "call " + addrToRegisterFunc,

                    "@outRegister:",

                    "popad",
                    "popfd",
                    "push esi",
                    "push edi",
                    "mov edi, ecx",
                    "call 0x007040D0",
                    "jmp 0x00704129",
                };
            IntPtr detourPtr = Memory.InjectAsm(detour, "RegisterFunctionDetour");

            byte[] orig = Memory.Reader.ReadBytes((IntPtr)0x00704120, 5);
            Memory.InjectAsm(0x00704120, "jmp " + detourPtr, "RegisterFunctionJmp");
        }
    }
}
