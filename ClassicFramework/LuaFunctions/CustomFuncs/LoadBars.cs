using ClassicFramework.Mem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassicFramework.LuaFunctions.CustomFuncs
{
    internal static class LoadBars
    {
        // ##########################
        // Other stuff is copy paste lel
        // ##########################
        private static string funcName = "LoadBars";

        private static int FuncBody(IntPtr parLuaState)
        {
            if (Lua.ArgsCount(parLuaState) != 1 || !Lua.IsString(parLuaState, 1))
            {
                Lua.Print("Usage is LoadBars(\\\"x\\\")");
                return 0;
            }
            string profName = Lua.ToString(parLuaState, 1);
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string file = path + "\\Bars.ini";
            if (!File.Exists(file))
            {
                Lua.Print("No bar profile has been saved so far!");
                return 0;
            }
            List<string> profiles = File.ReadAllText(file).Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            uint barSlot = 0x00BC6980;
            string profile = profiles.OfType<string>().Where(i => i.Contains(profName) && i.Contains(ObjectManager.Player.Name)).FirstOrDefault();
            if (String.IsNullOrEmpty(profile))
            {
                Lua.Print("Profile with name " + profName + " doesnt exist yet!");
                return 0;
            }

            Functions.DoString(
                        "for i=1,109 do PickupAction(i) ClearCursor() end");


            string[] parts = profile.Split('|');
            for (int i = 0; i < 109; i++)
            {
                Memory.Reader.Write<uint>((IntPtr)barSlot, Convert.ToUInt32(parts[2 + i]));
                barSlot = barSlot + 0x4;
            }

            Functions.DoString(
                "for i=1,109 do PickupAction(i) PlaceAction(i) end");

            
            Lua.Print("Loaded profile " + profName);;
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
