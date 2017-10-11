using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ClassicFramework.Mem;

namespace ClassicFramework.PacketSender
{
    [StructLayoutAttribute(LayoutKind.Explicit)]
    internal struct DataStoreStruct
    {
        [FieldOffsetAttribute(0)]
        internal int vTable;
        [FieldOffsetAttribute(4)]
        internal int buffer;
        [FieldOffsetAttribute(8)]
        internal int _base;
        [FieldOffsetAttribute(12)]
        internal int type;
        [FieldOffsetAttribute(16)]
        internal int size;
        [FieldOffsetAttribute(20)]
        internal int read;
    }

    internal class DataStore
    {
        private DataStoreStruct ds;
        private List<byte> buffer;
        private IntPtr bufferPtr;
        private IntPtr dataStorePtr;
        internal DataStore()
        {
            buffer = new List<byte>();
            ds = new DataStoreStruct();
            ds.vTable = 0x7FF9E4;
            ds._base = 0;
            ds.read = 0;
            ds.type = 256;
        }

        internal void PutInt16(Int16 parValue)
        {
            byte[] arr = BitConverter.GetBytes(parValue);
            buffer.AddRange(arr);
        }

        internal void PutBytes(byte[] parBytes)
        {
            buffer.AddRange(parBytes);
        }

        internal void PutInt32(Int32 parValue)
        {
            byte[] arr = BitConverter.GetBytes(parValue);
            buffer.AddRange(arr);
        }

        internal void PutBool(bool parValue)
        {
            byte[] arr = BitConverter.GetBytes(parValue);
            buffer.AddRange(arr);
        }

        //internal void Send()
        //{
        //    byte[] arr = buffer.ToArray();
        //    bufferPtr = Memory.Reader.Alloc(arr.Length);
        //    Memory.Reader.WriteBytes(bufferPtr, arr);
        //    ds.buffer = bufferPtr.ToInt32();
        //    ds.size = arr.Length - 1;

        //    dataStorePtr = Memory.Reader.Alloc(Marshal.SizeOf(typeof(DataStoreStruct)));
        //    Memory.Reader.Write<DataStoreStruct>(dataStorePtr, ds);

        //    Functions.NetClientSend(dataStorePtr);
        //    Memory.Reader.Dealloc(bufferPtr);
        //    Memory.Reader.Dealloc(dataStorePtr);
        //}
    }
}
