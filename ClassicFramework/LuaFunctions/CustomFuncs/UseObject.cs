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
    internal static class UseObject
    {
        // ##########################
        // Other stuff is copy paste lel
        // ##########################
        private static string funcName = "UseObject";
        private static bool enabled = false;

        private static int FuncBody(IntPtr parLuaState)
        {
            if (Lua.ArgsCount(parLuaState) != 1 || !Lua.IsString(parLuaState, 1))
            {
                Lua.Print("Usage is UseObject(\\\"name\\\")");
                return 0;
            }
            string wObject = Lua.ToString(parLuaState, 1);

            if (!ObjectManager.EnumObjects()) return 0;
            WoWGameObject obj = ObjectManager.Objects.OfType<WoWGameObject>()
                .Where(i => i.Name == wObject)
                .OrderBy(i => i.DistanceTo(ObjectManager.Player))
                .FirstOrDefault();
            if (obj == null) return 0;
            obj.Interact(false);
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
