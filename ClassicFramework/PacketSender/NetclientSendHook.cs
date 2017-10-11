using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ClassicFramework.Constants;
using ClassicFramework.Helpers.GreyMagic.Internals;
using ClassicFramework.Mem;

namespace ClassicFramework.PacketSender
{
    internal static class NetclientSendHook
    {
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void NetClientSend(IntPtr clientconn, int DataStore);

        private static NetClientSend NetClientSendDelegate;
        private static Detour _NetClientSend;

        internal static void Init()
        {
            byte[] origNetClientSend = Memory.Reader.ReadBytes((IntPtr)Offsets.Functions.NetClientSend, 5);
            
            NetClientSendDelegate = Memory.Reader.RegisterDelegate<NetClientSend>((IntPtr)Offsets.Functions.NetClientSend);
            _NetClientSend = Memory.Reader.Detours.CreateAndApply(NetClientSendDelegate, new NetClientSend(NetClientSendDetour), "NetClientSend");

            byte[] customNetClientSend = Memory.Reader.ReadBytes((IntPtr)Offsets.Functions.NetClientSend, 5);

            Hack hack = new Hack((IntPtr)Offsets.Functions.NetClientSend, customNetClientSend, origNetClientSend, "NetClientSendHook");
            AntiWarden.HookWardenMemScan.AddHack(hack);
        }

        private static void NetClientSendDetour(IntPtr parClientConn, int parDataStore)
        {
            Memory.Reader.Write<int>((IntPtr)parDataStore, 0x7FF9E4);

            ////Console.WriteLine();
            DataStoreStruct ds = Memory.Reader.Read<DataStoreStruct>((IntPtr)parDataStore);
            ////Console.WriteLine("vTable: " + ds.vTable.ToString("X"));
            ////Console.WriteLine("Buffer: " + ds.buffer.ToString("X"));
            ////Console.WriteLine("Base: " + ds._base);
            ////Console.WriteLine("Size: " + ds.size);
            ////Console.WriteLine("Read: " + ds.read);

            int opCode = Memory.Reader.Read<int>((IntPtr)ds.buffer);
            string opCodeName = Enum.GetName(typeof(Offsets.OpCodes), opCode);
            //Console.WriteLine("OpCode: " + opCodeName);
            
            //Console.WriteLine();


            //int basePtr = Memory.Reader.Read<int>((IntPtr)parDataStore + 4);
            //int opCode = Memory.Reader.Read<int>((IntPtr)basePtr);
            //string opCodeName = Enum.GetName(typeof(Offsets.OpCodes), opCode);

            ////Console.WriteLine(opCodeName
            //);

            

            _NetClientSend.Remove();
            NetClientSendDelegate(parClientConn, parDataStore);
            _NetClientSend.Apply();
        }
    }
}
