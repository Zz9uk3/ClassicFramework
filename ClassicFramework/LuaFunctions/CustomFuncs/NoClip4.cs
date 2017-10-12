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
//    internal static class NoClip4
//    {
//        // ##########################
//        // Other stuff is copy paste lel
//        // ##########################
//        private static string funcName = "NoClip4";
//        private static bool enabled = false;

//        private static int FuncBody(IntPtr parLuaState)
//        {
//            if (Lua.ArgsCount(parLuaState) != 0) return 0;

//            if (!enabled)
//            {
//                Hack hack = AntiWarden.HookWardenMemScan.GetHack("Collision4");
//                if (hack == null)
//                {
//                    hack = new Hack(true, 0xA58, new byte[] { 0x00, 0x00, 0x00, 0x00 }, "Collision4");
//                    AntiWarden.HookWardenMemScan.AddHack(hack);
//                }

//                hack.Apply();
//                Memory.GetHack("Superfly").Apply();
//                Memory.GetHack("Antijump").Apply();
//                SuperFly.enabled = true;
//                Lua.Print("Noclip with terrain aswell SuperFly enabled!");
//                enabled = true;
//            }
//            else
//            {
//                AntiWarden.HookWardenMemScan.GetHack("Collision4").Remove();
//                Lua.Print("Noclip with terrain disabled!");
//                enabled = false;
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
