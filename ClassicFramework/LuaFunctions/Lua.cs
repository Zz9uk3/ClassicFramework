using ClassicFramework.Mem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassicFramework.LuaFunctions
{
    internal static class Lua
    {
        internal static bool IsNumber(IntPtr parLuaState, int parArgNumber)
        {
            return Functions.LuaIsNumber(parLuaState, parArgNumber);
        }

        internal static bool IsString(IntPtr parLuaState, int parArgNumber)
        {
            return Functions.LuaIsString(parLuaState, parArgNumber);
        }

        internal static double ToNumber(IntPtr parLuaState, int parArgNumber)
        {
            return Functions.LuaToNumber(parLuaState, parArgNumber);
        }

        internal static string ToString(IntPtr parLuaState, int parArgNumber)
        {
            return Functions.LuaToString(parLuaState, parArgNumber);
        }

        internal static int ArgsCount(IntPtr parLuaState)
        {
            return Functions.LuaGetArgCount(parLuaState);
        }

        internal static void Print(string parMsg)
        {
            Functions.DoString("DEFAULT_CHAT_FRAME:AddMessage('" + parMsg + "', 0.0, 1.0, 0.0);");
        }

        internal static void Welcome()
        {
            Lua.Print("Zzuks ClassicFramework (v0.07c) - Visit http://zzukbot.com\\n" +
                "Use Commands() for an overview\\n"
                + "Default features: Understand all languages, See all levels, Lua unlocked, Anti AFK");
        }
    }
}
