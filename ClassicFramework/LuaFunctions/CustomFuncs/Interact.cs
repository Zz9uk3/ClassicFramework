using ClassicFramework.Mem;
using ClassicFramework.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassicFramework.LuaFunctions.CustomFuncs
{
    internal static class Interact
    {
        // ##########################
        // Other stuff is copy paste lel
        // ##########################
        private static string funcName = "Interact";
        
        private static int FuncBody(IntPtr parLuaState)
        {
            if (Lua.ArgsCount(parLuaState) != 0)
            {
                Lua.Print("Usage is Interact()");
                return 0;
            }

            if (!ObjectManager.EnumObjects()) return 0;
            WoWUnit target = ObjectManager.Target;
            target?.Interact(false);
            
            return 0;
        }
        // ##########################
        // ##########################
        // ##########################

        internal static void Init(ref uint parTrampToFunction)
        {
            FunctionHandling.Register(FuncBody, funcName, ref parTrampToFunction);
        }

        internal static void Remove(ref uint parTrampToFunction)
        {
            FunctionHandling.Remove(funcName, ref parTrampToFunction);
        }
    }
}
