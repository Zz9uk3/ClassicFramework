using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ClassicFramework.Constants;

namespace ClassicFramework.OOP
{
    internal static class Launch
    {
        internal static void Run()
        {
            string tmpPath = "";
            try
            {
                XDocument doc = XDocument.Load(".\\Settings.xml");
                XElement path = doc.Element("Settings").Element("Path");
                XElement loot = doc.Element("Settings").Element("AutoLoot");
                tmpPath = path.Value;
                LuaFunctions.CustomFuncs.AutoLoot.enabled = Convert.ToBoolean(loot.Value);
            }
            catch
            {
                MessageBox.Show("Settings.xml is corrupted! Please delete it and rerun the framework");
                return;
            }

            WinImports.STARTUPINFO si = new WinImports.STARTUPINFO();
            WinImports.PROCESS_INFORMATION pi = new WinImports.PROCESS_INFORMATION();
            bool success = WinImports.CreateProcess(tmpPath, null,
                IntPtr.Zero, IntPtr.Zero, false,
                WinImports.ProcessCreationFlags.CREATE_DEFAULT_ERROR_MODE,
                IntPtr.Zero, null, ref si, out pi);
            Thread.Sleep(500);
            WinImports.SuspendThread(pi.hThread);
            DllInjectionResult res = DllInjector.GetInstance.Inject((int)pi.dwProcessId);
            WinImports.ResumeThread(pi.hThread);
        }
    }
}
