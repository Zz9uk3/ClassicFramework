using ClassicFramework.Mem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassicFramework.LuaFunctions.CustomFuncs
{
    internal static class NoClip2
    {
        // ##########################
        // Other stuff is copy paste lel
        // ##########################
        private static string funcName = "NoClip2";
        private static bool enabled = false;

        private static int FuncBody(IntPtr parLuaState)
        {
            if (Lua.ArgsCount(parLuaState) != 0) return 0;

            if (!enabled)
            {
                AntiWarden.HookWardenMemScan.GetHack("Collision2").Apply();
                Lua.Print("Noclip with buildings enabled!");
                enabled = true;
            }
            else
            {
                AntiWarden.HookWardenMemScan.GetHack("Collision2").Remove();
                Lua.Print("Noclip with buildings disabled!");
                enabled = false;
            }
            
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
