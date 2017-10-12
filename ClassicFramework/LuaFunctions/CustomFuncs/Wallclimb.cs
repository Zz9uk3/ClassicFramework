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
//    internal static class Wallclimb
//    {
//        // ##########################
//        // Other stuff is copy paste lel
//        // ##########################
//        private static string funcName = "Wallclimb";

//        private static int FuncBody(IntPtr parLuaState)
//        {
//            if (Lua.ArgsCount(parLuaState) != 1 || !Lua.IsNumber(parLuaState, 1))
//            {
//                Lua.Print("Value for Wallclimb(x) must be between 0 and 0.64.\\n"
//                    + "Wallclimb cant be detected by Warden however a to low value is also risky. Be careful!");
//                return 0;
//            }
//            double val = Lua.ToNumber(parLuaState, 1);
//            if (val >= 0 && val <= 0.64)
//            {
//                float val2 = Convert.ToSingle(val);
//                byte[] bytes = BitConverter.GetBytes(val2);
//                AntiWarden.HookWardenMemScan.GetHack("Wallclimb").CustomBytes = bytes;
//                    Memory.Reader.WriteBytes((IntPtr)0x0080DFFC, bytes);
//                    Lua.Print("Wallclimb angle set to " + val2);

//            }
//            else
//            {
//                Lua.Print("Value for Wallclimb(x) must be between 0 and 0.64.\\n"
//                    + "Wallclimb cant be detected by Warden however a to low value is also risky. Be careful!");
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
