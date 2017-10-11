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
    internal static class NoClip1
    {
        // ##########################
        // Other stuff is copy paste lel
        // ##########################
        private static string funcName = "NoClip1";
        private static bool enabled = false;

        private static int FuncBody(IntPtr parLuaState)
        {
            if (Lua.ArgsCount(parLuaState) != 0) return 0;

            if (!enabled)
            {
                AntiWarden.HookWardenMemScan.GetHack("Collision1").Apply();
                Lua.Print("Noclip with trees enabled!");
                enabled = true;
            }
            else
            {
                AntiWarden.HookWardenMemScan.GetHack("Collision1").Remove();
                Lua.Print("Noclip with trees disabled!");
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
