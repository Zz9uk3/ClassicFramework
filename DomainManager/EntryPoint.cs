using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DomainManager
{
    public class EntryPoint
    {
        [STAThread]
        public static int Main(string args)
        {
            try
            {
                new ClassicFrameworkDomain();
                Thread.Sleep(10);
            }
            catch (Exception e)
            {
                MessageBox.Show("Entry Point Exception: " + e);
            }
            Process.GetCurrentProcess().Kill();
            return 1;
        }
    }
}