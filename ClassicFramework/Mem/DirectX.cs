using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using ClassicFramework.Constants;
using ClassicFramework.Helpers.GreyMagic.Internals;
using ClassicFramework.LuaFunctions;

namespace ClassicFramework.Mem
{
    [Program.Singleton]
    internal class DirectX
    {
        private readonly Direct3D9EndScene _endSceneDelegate;
        private readonly Detour _endSceneHook;
        private readonly ConcurrentQueue<Action> _actionQueue = new ConcurrentQueue<Action>();

        private DirectX()
        {
            Console.WriteLine("DirectX applied");
            _endSceneDelegate =
                Memory.Reader.RegisterDelegate<Direct3D9EndScene>(GetEndScene.Instance.ToPointer());
            _endSceneHook =
                Memory.Reader.Detours.CreateAndApply(
                    _endSceneDelegate,
                    new Direct3D9EndScene(EndSceneHook),
                    "D9EndScene");
        }

        internal static DirectX Instance { get; } = new DirectX();

        /// <summary>
        /// The code EndScene get detoured to
        /// </summary>
        private static int frameCounter = 0;
        internal delegate void Run(ref int FrameCount);
        private static Run _Run = RunDummy;
        private static bool _CanRun = true;
        internal static byte FirstRun = 0;

        /// <summary>
        /// Run the dummy again to stop executing current method which
        /// _Run delegates points to
        /// </summary>
        private static void RunDummy(ref int parFrameCount)
        {
            _CanRun = true;
        }

        /// <summary>
        /// Run the provided method in EndScene
        /// </summary>
        internal static bool RunInEndScene(Run parMethod)
        {
            if (_CanRun)
            {
                _Run = parMethod;
                _CanRun = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Stop running the current method in EndScene
        /// </summary>
        internal static void StopRunning()
        {
            _Run = RunDummy;
        }

        private int EndSceneHook(IntPtr parDevice)
        {
            if (FirstRun == 1)
            {
                if (ObjectManager.EnumObjects())
                {
                    FirstRun = 0;
                    Lua.Welcome();
                }
            }
            
            // run our delegate
            _Run(ref frameCounter);
            // reset the framecounter
            frameCounter = frameCounter % 180 + 1;
            if (frameCounter == 1)
            {
                Memory.Reader.Write<int>((IntPtr)Offsets.Functions.LastHardwareAction, Environment.TickCount);
            }
            return (int)_endSceneHook.CallOriginal(parDevice);
        }

        /// <summary>
        ///     EndScene delegate
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int Direct3D9EndScene(IntPtr device);
    }
}