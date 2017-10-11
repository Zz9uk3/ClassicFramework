using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ClassicFramework.Helpers.GreyMagic.Internals;

namespace ClassicFramework.Mem
{
    internal sealed class GetEndScene
    {
        private static readonly object lockObject = new object();

        private static readonly Lazy<GetEndScene> _instance =
            new Lazy<GetEndScene>(() => new GetEndScene());

        private Direct3D9IsSceneEnd _isSceneEndDelegate;
        private Detour _isSceneEndHook;

        private IntPtr EndScenePtr = IntPtr.Zero;

        internal static GetEndScene Instance
        {
            get
            {
                lock (lockObject)
                {
                    Console.WriteLine("GetEndScene created");
                    return _instance.Value;
                }
            }
        }

        [Obfuscation(Feature = "virtualization", Exclude = false)]
        internal IntPtr ToPointer()
        {
            if (EndScenePtr != IntPtr.Zero) return EndScenePtr;

            _isSceneEndDelegate =
                Memory.Reader.RegisterDelegate<Direct3D9IsSceneEnd>((IntPtr)0x005A17A0);
            _isSceneEndHook =
                Memory.Reader.Detours.CreateAndApply(
                    _isSceneEndDelegate,
                    new Direct3D9IsSceneEnd(IsSceneEndHook),
                    "IsSceneEnd");

            while (EndScenePtr == IntPtr.Zero)
                Task.Delay(5).Wait();

            return EndScenePtr;
        }

        [Obfuscation(Feature = "virtualization", Exclude = false)]
        private IntPtr IsSceneEndHook(IntPtr device)
        {
            //[[ESI+38A8]]+A8
            var ptr1 = Memory.Reader.Read<IntPtr>(IntPtr.Add(device, 0x38a8));
            var ptr2 = Memory.Reader.Read<IntPtr>(ptr1);
            var ptr3 = Memory.Reader.Read<IntPtr>(IntPtr.Add(ptr2, 0xa8));
            EndScenePtr = ptr3;

            _isSceneEndHook.Remove();
            return _isSceneEndDelegate(device);
        }


        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr Direct3D9IsSceneEnd(IntPtr device);
    }
}