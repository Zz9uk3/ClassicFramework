using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ClassicFramework.Constants;
using ClassicFramework.Mem;
using ClassicFramework.OOP;
using System.Threading;
using ClassicFramework.LuaFunctions;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using ClassicFramework.Helpers;

namespace ClassicFramework
{
    static class Program
    {
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string name = new AssemblyName(args.Name).Name;
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + name + ".dll";
            return Assembly.LoadFile(path);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [MethodImplAttribute(MethodImplOptions.NoInlining)] 
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            MainTwo();
        }

        static void MainTwo()
        {
            // Setting culture for float etc (. instead of ,)
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // ClassicFramework cant be run alone!
            if (Process.GetCurrentProcess().ProcessName.StartsWith("ClassicFramework"))
            {
                if (!Assembly.GetExecutingAssembly().Location.ToLower().Contains("internal"))
                {
                    MessageBox.Show("Your file structure is corrupt. Please redownload ClassicFramework");
                    Environment.Exit(-1);
                }
                // Do the settings exist?
                if (!File.Exists(".\\Settings.xml"))
                {
                    OpenFileDialog loc = new OpenFileDialog();
                    loc.CheckFileExists = true;
                    loc.CheckPathExists = true;
                    loc.Filter = "executable (*.exe)|*.exe";
                    loc.FilterIndex = 1;
                    loc.Title = "Please locate your WoW.exe";
                    if (loc.ShowDialog() == DialogResult.OK)
                    {
                        OOP.Settings.Recreate(loc.FileName);
                    }
                }
                Launch.Run();
                Environment.Exit(0);
            }
            WinImports.LoadLibrary(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FastCallDll.dll"
            );
            //WinImports.AllocConsole();
            //Logger.Append("We are injected now!");

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            XDocument doc = XDocument.Load(path + "\\Settings.xml");
            XElement loot = doc.Element("Settings").Element("AutoLoot");
            LuaFunctions.CustomFuncs.AutoLoot.enabled = Convert.ToBoolean(loot.Value);
            Memory.Init();
            RegisterFunctionHook.Init();
            UnregisterFunctionHook.Init();
            if (LuaFunctions.CustomFuncs.AutoLoot.enabled)
            {
                OnRightClickUnitHook.Apply();
                OnRightClickObjectHook.Apply();
            }
            Singleton.Initialise();
            while (true)
            {
                Thread.Sleep(250);
            }
            // Run the GUI
            //Application.Run(new Main());
        }

        internal class Singleton : Attribute
        {
            internal static void Initialise()
            {
                // get a list of types which are marked with the InitOnLoad attribute
                var types =
                    AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .Where(t => t.GetCustomAttributes(typeof(Singleton), false).Any());

                // process each type to force initialise it
                foreach (var type in types)
                {
                    // try to find a static field which is of the same type as the declaring class
                    var field =
                        type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                            .FirstOrDefault(f => f.FieldType == type);
                    // evaluate the static field if found
                    field?.GetValue(null);
                }
            }
        }
    }

    internal class Vector3
    {
        internal float X;
        internal float Y;
        internal float Z;

        internal Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        internal float DistanceTo(Vector3 l)
        {
            var dx = X - l.X;
            var dy = Y - l.Y;
            var dz = Z - l.Z;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        internal float DistanceTo2D(Vector3 l)
        {
            var dx = X - l.X;
            var dy = Y - l.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
