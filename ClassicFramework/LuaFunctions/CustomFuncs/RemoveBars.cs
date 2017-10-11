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
    internal static class RemoveBars
    {
        // ##########################
        // Other stuff is copy paste lel
        // ##########################
        private static string funcName = "RemoveBars";

        private static int FuncBody(IntPtr parLuaState)
        {
            if (Lua.ArgsCount(parLuaState) != 1 || !Lua.IsString(parLuaState, 1))
            {
                Lua.Print("Will remove a bar profile. Usage is RemoveBars(\\\"x\\\")");
                return 0;
            }
            string profName = Lua.ToString(parLuaState, 1);
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string file = path + "\\Bars.ini";

            if (!File.Exists(file))
            {
                return 0;
            }
            List<string> profiles = File.ReadAllText(file).Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            profiles.RemoveAll(i => i.Contains(profName) && i.Contains(ObjectManager.Player.Name));
            File.Delete(file);
            foreach (string str in profiles)
            {
                if (String.IsNullOrEmpty(str)) continue;
                File.AppendAllText(file, str + Environment.NewLine);
            }
            Lua.Print("Removed profile with name " + profName + " if it existed");
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
