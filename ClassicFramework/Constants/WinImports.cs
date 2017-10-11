using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClassicFramework.Constants
{
    internal static class WinImports
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        /// MODULEENTRY32 struct for Module32First/Next
        /// </summary>
        [StructLayoutAttribute(LayoutKind.Sequential)]
        internal struct MODULEENTRY32
        {
            internal uint dwSize;
            internal uint th32ModuleID;
            internal uint th32ProcessID;
            internal uint GlblcntUsage;
            internal uint ProccntUsage;
            IntPtr modBaseAddr;
            internal uint modBaseSize;
            IntPtr hModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            internal string szModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            internal string szExePath;
        };

        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);
        internal struct SYSTEM_INFO
        {
            internal ushort processorArchitecture;
            ushort reserved;
            internal uint pageSize;
            internal IntPtr minimumApplicationAddress;  // minimum address
            internal IntPtr maximumApplicationAddress;  // maximum address
            internal IntPtr activeProcessorMask;
            internal uint numberOfProcessors;
            internal uint processorType;
            internal uint allocationGranularity;
            internal ushort processorLevel;
            internal ushort processorRevision;
        }

        /// <summary>
        /// Module32First
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        /// <summary>
        /// Module32Next
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool Module32Next(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        /// <summary>
        /// Get address of a function from a dll loaded inside the process (handle)
        /// </summary>
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        /// <summary>
        /// Get Module handle for a loaded dll
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        [DllImport("kernel32.dll")]
        public static extern bool CreateProcess(string lpApplicationName,
               string lpCommandLine, IntPtr lpProcessAttributes,
               IntPtr lpThreadAttributes,
               bool bInheritHandles, ProcessCreationFlags dwCreationFlags,
               IntPtr lpEnvironment, string lpCurrentDirectory,
               ref STARTUPINFO lpStartupInfo,
               out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll")]
        public static extern uint ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(IntPtr hThread);

        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        [Flags]
        public enum ProcessCreationFlags : uint
        {
            ZERO_FLAG = 0x00000000,
            CREATE_BREAKAWAY_FROM_JOB = 0x01000000,
            CREATE_DEFAULT_ERROR_MODE = 0x04000000,
            CREATE_NEW_CONSOLE = 0x00000010,
            CREATE_NEW_PROCESS_GROUP = 0x00000200,
            CREATE_NO_WINDOW = 0x08000000,
            CREATE_PROTECTED_PROCESS = 0x00040000,
            CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,
            CREATE_SEPARATE_WOW_VDM = 0x00001000,
            CREATE_SHARED_WOW_VDM = 0x00001000,
            CREATE_SUSPENDED = 0x00000004,
            CREATE_UNICODE_ENVIRONMENT = 0x00000400,
            DEBUG_ONLY_THIS_PROCESS = 0x00000002,
            DEBUG_PROCESS = 0x00000001,
            DETACHED_PROCESS = 0x00000008,
            EXTENDED_STARTUPINFO_PRESENT = 0x00080000,
            INHERIT_PARENT_AFFINITY = 0x00010000
        }

        [DllImport("User32.dll")]
        internal static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern void SetLastError(uint dwErrCode);
    }
}
