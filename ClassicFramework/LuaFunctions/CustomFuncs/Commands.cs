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
    internal static class Commands
    {
        // ##########################
        // Other stuff is copy paste lel
        // ##########################
        private static string funcName = "Commands";

        private static int FuncBody(IntPtr parLuaState)
        {
            if (Lua.ArgsCount(parLuaState) != 0)
            {
                return 0;
            }
            Lua.Print("Thanks(), SuperFly(), AddToZ(x), Wallclimb(x), NoClip1(), NoClip2(), NoClip3(), NoClip4(), UseItem(\\\"x\\\"), Loot(), UseSpell(\\\"x\\\"), UseObject(\\\"x\\\"), Interact(), AutoLoot(), SaveBars(\\\"x\\\"), LoadBars(\\\"x\\\"), RemoveBars(\\\"x\\\")");
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
