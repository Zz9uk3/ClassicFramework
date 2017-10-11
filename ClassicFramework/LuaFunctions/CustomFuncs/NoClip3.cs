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
    internal static class NoClip3
    {
        // ##########################
        // Other stuff is copy paste lel
        // ##########################
        private static string funcName = "NoClip3";
        private static bool enabled = false;

        private static int FuncBody(IntPtr parLuaState)
        {
            if (Lua.ArgsCount(parLuaState) != 0) return 0;

            if (!enabled)
            {
                AntiWarden.HookWardenMemScan.GetHack("Collision3").Apply();
                Lua.Print("Noclip with mailboxes etc. enabled!");
                enabled = true;
            }
            else
            {
                AntiWarden.HookWardenMemScan.GetHack("Collision3").Remove();
                Lua.Print("Noclip with mailboxes etc. disabled!");
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
