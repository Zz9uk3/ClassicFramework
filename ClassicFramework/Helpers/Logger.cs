using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicFramework.Helpers
{
    internal static class Logger
    {
        internal static void Append(string parMessage)
        {
            Console.WriteLine("[" + DateTime.Now.ToString("HH:MM:ss") + "] " + parMessage);
        }
        
    }
}
