using ClassicFramework.Mem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ClassicFramework.LuaFunctions.CustomFuncs
{
    internal static class AutoLoot
    {
        // ##########################
        // Other stuff is copy paste lel
        // ##########################
        private static string funcName = "AutoLoot";
        internal static bool enabled;

        private static int FuncBody(IntPtr parLuaState)
        {
            try
            {
                if (Lua.ArgsCount(parLuaState) != 0) return 0;

                if (enabled)
                {
                    enabled = false;
                    OnRightClickUnitHook.Remove();
                    OnRightClickObjectHook.Remove();
                    Lua.Print("Autoloot disabled");
                }
                else
                {
                    enabled = true;
                    OnRightClickUnitHook.Apply();
                    OnRightClickObjectHook.Apply();
                    Lua.Print("Autoloot enabled");
                }

                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                XDocument doc = XDocument.Load(path + "\\Settings.xml");
                doc.Element("Settings").Element("AutoLoot").Value = enabled.ToString();
                doc.Save(path + ".\\Settings.xml");
            }
            catch
            {
                MessageBox.Show("Settings.xml is broken or non-existent. Please delete it and relaunch the hack!");
                Process.GetCurrentProcess().Kill();
            }
            
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
