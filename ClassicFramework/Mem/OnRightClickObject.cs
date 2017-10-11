using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ClassicFramework.Helpers.GreyMagic.Internals;

namespace ClassicFramework.Mem
{
    internal static class OnRightClickObjectHook
    {
        private static OnRightClickObjectDelegate _OnRightClickObjectDelegate;
        private static Detour _OnRightClickObjectHook;

        internal static void Apply()
        {
            if (_OnRightClickObjectHook == null)
            {
                byte[] old = Memory.Reader.ReadBytes((IntPtr)0x005F8660, 8);
                _OnRightClickObjectDelegate = Memory.Reader.RegisterDelegate<OnRightClickObjectDelegate>((IntPtr)0x005F8660);
                _OnRightClickObjectHook = Memory.Reader.Detours.CreateAndApply(_OnRightClickObjectDelegate, new OnRightClickObjectDelegate(OnRightClickObjectHookFunc), "OnRightClickObjectHook");
                byte[] custom = Memory.Reader.ReadBytes((IntPtr)0x005F8660, 8);
                Hack autolootUnit = new Hack((IntPtr)0x005F8660, custom, old, "OnRightClickObject");
                AntiWarden.HookWardenMemScan.AddHack(autolootUnit);
            }
            _OnRightClickObjectHook.Apply();
        }

        internal static void Remove()
        {

            if (_OnRightClickObjectHook != null && _OnRightClickObjectHook.IsApplied)
            {
                _OnRightClickObjectHook.Remove();
            }
        }

        internal static void OnRightClickObjectHookFunc(IntPtr unitPtr, int autoLoot)
        {
            _OnRightClickObjectHook.Remove();
            Functions.OnRightClickObject(unitPtr, 1);
            _OnRightClickObjectHook.Apply();
        }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void OnRightClickObjectDelegate(IntPtr unitPtr, int autoLoot);
    }



}
