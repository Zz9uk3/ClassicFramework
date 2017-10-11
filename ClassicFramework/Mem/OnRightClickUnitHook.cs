using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ClassicFramework.Helpers.GreyMagic.Internals;

namespace ClassicFramework.Mem
{
    internal static class OnRightClickUnitHook
    {
        private static OnRightClickUnitDelegate _OnRightClickUnitDelegate;
        private static Detour _OnRightClickUnitHook;

        internal static void Apply()
        {
            if (_OnRightClickUnitHook == null)
            {
                byte[] old = Memory.Reader.ReadBytes((IntPtr)0x60BEA0, 8);
                _OnRightClickUnitDelegate = Memory.Reader.RegisterDelegate<OnRightClickUnitDelegate>((IntPtr)0x60BEA0);
                _OnRightClickUnitHook = Memory.Reader.Detours.CreateAndApply(_OnRightClickUnitDelegate, new OnRightClickUnitDelegate(OnRightClickUnitHookFunc), "OnRightClickUnitHook");
                byte[] custom = Memory.Reader.ReadBytes((IntPtr)0x60BEA0, 8);
                Hack autolootUnit = new Hack((IntPtr)0x60BEA0, custom, old, "OnRightClickUnit");
                AntiWarden.HookWardenMemScan.AddHack(autolootUnit);
            }
            _OnRightClickUnitHook.Apply();
        }

        internal static void Remove()
        {

            if (_OnRightClickUnitHook != null && _OnRightClickUnitHook.IsApplied)
            {
                _OnRightClickUnitHook.Remove();
            }
        }

        internal static void OnRightClickUnitHookFunc(IntPtr unitPtr, int autoLoot)
        {
            _OnRightClickUnitHook.Remove();
            Functions.OnRightClickUnit(unitPtr, 1);
            _OnRightClickUnitHook.Apply();
        }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void OnRightClickUnitDelegate(IntPtr unitPtr, int autoLoot);
    }

    

}
