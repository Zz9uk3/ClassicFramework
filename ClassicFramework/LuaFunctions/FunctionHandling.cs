using ClassicFramework.Mem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassicFramework.LuaFunctions
{
    internal static class FunctionHandling
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        internal delegate int CustomFuncDelegate(IntPtr luaState);

        private static List<CustomFuncDelegate> delegates = new List<CustomFuncDelegate>();
        
        internal static void ClearDelegates()
        {
            delegates.Clear();
        }

        internal static void Register(CustomFuncDelegate parDelegate, string parFuncName, ref uint parAddr)
        {
            delegates.Add(parDelegate);
            IntPtr addrSetZAxis = Marshal.GetFunctionPointerForDelegate(parDelegate);
            byte[] old = new byte[] { 0x08, 0x08, 0x08, 0x08, 0x08 };
            
            Mem.Memory.InjectAsm(parAddr, "jmp " + addrSetZAxis, "cf_" + parFuncName);
            Functions.RegisterFunction(parFuncName, parAddr);
            parAddr += 5;
        }

        internal static void Remove(string parFuncName, ref uint parAddr)
        {
            Functions.UnregisterFunction(parFuncName, parAddr);
            parAddr += 5;
        }
    }
}
