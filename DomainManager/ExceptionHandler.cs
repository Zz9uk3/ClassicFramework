using System;
using System.Windows.Forms;

namespace DomainManager
{
    static class ExceptionHandler
    {
        public static void Unhandled(object sender, UnhandledExceptionEventArgs args)
        {
            MessageBox.Show(args.ExceptionObject.ToString(), "Unknown Exception", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
    }
}
