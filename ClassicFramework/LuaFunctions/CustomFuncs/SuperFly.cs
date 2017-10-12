//using ClassicFramework.Mem;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace ClassicFramework.LuaFunctions.CustomFuncs
//{
//    internal static class SuperFly
//    {
//        // ##########################
//        // Other stuff is copy paste lel
//        // ##########################
//        private static string funcName = "SuperFly";
//        internal static bool enabled = false;

//        private static int FuncBody(IntPtr parLuaState)
//        {
//            if (Lua.ArgsCount(parLuaState) != 0)
//            {
//                return 0;
//            }
//            if (enabled)
//            {
//                Memory.GetHack("Superfly").Remove();
//                Memory.GetHack("Antijump").Remove();
//                Lua.Print("Disabling Superfly");
//                enabled = false;
//            }
//            else
//            {
//                if ((ObjectManager.Player.MovementState & 0x00002000) == 0x00002000)
//                    ObjectManager.Player.MovementState = 0;
//                Memory.GetHack("Superfly").Apply();
//                Memory.GetHack("Antijump").Apply();
//                Lua.Print("Enabling Superfly");
//                enabled = true;
//            }
//            return 0;
//        }
//        // ##########################
//        // ##########################
//        // ##########################

//        internal static void Init(ref uint parTrampToFunction)
//        {
//            FunctionHandling.Register(FuncBody, funcName, ref parTrampToFunction);
//        }

//        internal static void Remove(ref uint parTrampToFunction)
//        {
//            FunctionHandling.Remove(funcName, ref parTrampToFunction);
//        }
//    }
//}
