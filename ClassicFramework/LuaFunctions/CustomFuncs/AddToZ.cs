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
//    internal static class AddToZ
//    {
//        // ##########################
//        // Other stuff is copy paste lel
//        // ##########################
//        private static string funcName = "AddToZ";

//        private static int FuncBody(IntPtr parLuaState)
//        {
//            if (Lua.ArgsCount(parLuaState) != 1 || !Lua.IsNumber(parLuaState, 1))
//            {
//                Lua.Print("Usage is AddToZ(x). x will be added to the toons Z Axis.");
//                return 0;
//            }
//            ObjectManager.EnumObjects();
//            ObjectManager.Player.ZAxis += (float)Lua.ToNumber(parLuaState, 1);
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
