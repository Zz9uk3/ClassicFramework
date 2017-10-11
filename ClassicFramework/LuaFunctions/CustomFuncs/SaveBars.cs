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
    internal static class SaveBars
    {
        // ##########################
        // Other stuff is copy paste lel
        // ##########################
        private static string funcName = "SaveBars";

        private static int FuncBody(IntPtr parLuaState)
        {
            if (Lua.ArgsCount(parLuaState) != 1 || !Lua.IsString(parLuaState, 1))
            {
                Lua.Print("Usage is SaveBars(\\\"x\\\")");
                return 0;
            }
            string profName = Lua.ToString(parLuaState, 1);
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string file = path + "\\Bars.ini";

            if (!File.Exists(file))
            {
                File.Create(file).Dispose();
            }
            List<string> profiles = File.ReadAllLines(file).ToList();
            string final = ObjectManager.Player.Name + "|" + profName + "|";
            uint barSlot = 0x00BC6980;
            for (int i = 0; i < 109; i++)
            {
                try
                {
                    final += Memory.Reader.Read<uint>((IntPtr)barSlot).ToString() + "|";
                }
                catch
                {
                    final += "0|";
                }
                barSlot = barSlot + 0x4;
            }
            profiles.RemoveAll(i => i.Contains(profName) && i.Contains(ObjectManager.Player.Name));
            profiles.Add(final);


            File.Delete(file);
            foreach (string str in profiles)
            {
                if (String.IsNullOrEmpty(str)) continue;
                File.AppendAllText(file, str + Environment.NewLine);
            }
            Lua.Print("Saved current bar setup with name " + profName);
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
