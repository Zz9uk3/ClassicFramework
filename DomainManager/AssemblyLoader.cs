using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DomainManager
{
    [ComVisible(true)]
    public class ClassicFrameworkAssemblyLoader : MarshalByRefObject
    {
        public ClassicFrameworkAssemblyLoader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        #region IAssemblyLoader Members

        public void LoadAndRun(string file)
        {
            Assembly asm = Assembly.LoadFrom(file);
            MethodInfo entry = asm.EntryPoint;
            //object o = asm.CreateInstance(entry.Name);
            entry.Invoke(null, null);
        }

        #endregion

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name == Assembly.GetExecutingAssembly().FullName)
                return Assembly.GetExecutingAssembly();

            string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string shortAsmName = Path.GetFileName(args.Name);
            string fileName = Path.Combine(appDir, shortAsmName);

            if (File.Exists(fileName))
            {
                return Assembly.LoadFrom(fileName);
            }
            return Assembly.GetExecutingAssembly().FullName == args.Name ? Assembly.GetExecutingAssembly() : null;
        }
    }
}