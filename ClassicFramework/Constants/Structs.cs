using System;
using System.Runtime.InteropServices;

namespace ClassicFramework.Constants
{
    /// <summary>
    /// Intersection struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct Intersection
    {
        internal float X;
        internal float Y;
        internal float Z;
        internal float R;

        public override string ToString()
        {
            return String.Format("Intersection -> X: {0} Y: {1} Z: {2} R: {3}", X, Y, Z, R);
        }
    }

    /// <summary>
    /// two coordinates (vector3 1 and vector3 2)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct XYZXYZ
    {
        internal float X1;
        internal float Y1;
        internal float Z1;
        internal float X2;
        internal float Y2;
        internal float Z2;

        internal XYZXYZ(float x1, float y1, float z1,
            float x2, float y2, float z2)
            : this()
        {
            X1 = x1;
            Y1 = y1;
            Z1 = z1;
            X2 = x2;
            Y2 = y2;
            Z2 = z2;
        }

        public override string ToString()
        {
            return String.Format("Start -> X: {0} Y: {1} Z: {2}\n"
                + "End -> X: {3} Y: {4} Z: {5}", X1, Y1, Z1, X2, Y2, Z2);
        }
    }

    /// <summary>
    /// Coordinate struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct XYZ
    {
        internal float X;
        internal float Y;
        internal float Z;

        internal XYZ(float x, float y, float z)
            : this()
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    /// <summary>
    /// Struct with an item to restock at the restock npc
    /// </summary>
    internal struct RestockItem
    {
        internal string Item;
        internal int RestockUpTo;

        internal RestockItem(string parItem, int parRestockUpTo)
        {
            Item = parItem;
            RestockUpTo = parRestockUpTo;
        }
    }

}
