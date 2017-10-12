using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ClassicFramework.Constants;
using ClassicFramework.Objects;

namespace ClassicFramework.Mem
{
    internal static class ObjectManager
    {
        /// <summary>
        /// Delegate Instances for: Enum, Callback, GetPtrByGuid, GetActivePlayer
        /// </summary>
        private static bool Prepared;
        private static IntPtr ourCallback;
        private static /*readonly*/ EnumVisibleObjectsCallback _callback;
        private static ClntObjMgrObjectPtr getPtrForGuid;
        private static ClntObjMgrGetActivePlayer getActivePlayer;


        /// <summary>
        /// Objectmanager internal dictionary
        /// </summary>
        private static Dictionary<ulong, WoWObject> _Objects = new Dictionary<ulong, WoWObject>();

        /// <summary>
        /// Objectmanager internal lists
        /// </summary>
        internal static LocalPlayer Player { get; private set; }
        // the toons current target
        internal static WoWUnit Target
        {
            get
            {
                List<WoWUnit> mobs = Mobs;
                WoWUnit target = mobs
                    .FirstOrDefault(i => i.Guid == Player.TargetGuid);
                if (target == null)
                {
                    List<WoWPlayer> players = Players;
                    target = players
                        .FirstOrDefault(i => i.Guid == Player.TargetGuid);
                }
                return target;
            }
        }
        internal static List<WoWObject> Objects = new List<WoWObject>();
        internal static List<WoWUnit> Mobs
        {
            get
            {
                return Objects.OfType<WoWUnit>()
                    .Where(i => i.GetType() == typeof(WoWUnit))
                    .ToList();
            }
        }

        internal static List<WoWGameObject> GameObjects
        {
            get
            {
                return Objects.OfType<WoWGameObject>()
                    .ToList();
            }
        }
        internal static List<WoWPlayer> Players => Objects.OfType<WoWPlayer>()
            .ToList();
        internal static List<WoWItem> Items => Objects.OfType<WoWItem>().ToList();

        /// <summary>
        /// Delegates for: Enum, Callback, GetPtrByGuid, GetActivePlayer
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int EnumVisibleObjectsCallback(int filter, ulong guid);

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr ClntObjMgrObjectPtr(int filter, UInt64 guid);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate UInt64 ClntObjMgrGetActivePlayer();


        [DllImport("FastCallDll.dll", EntryPoint = "_EnumVisibleObjects", CallingConvention = CallingConvention.StdCall)]
        private static extern void _EnumVisibleObjects(IntPtr callback, int filter);

        private static void EnumVisibleObjects(IntPtr callback, int filter)
        {
            _EnumVisibleObjects(callback, filter);
        }


        /// <summary>
        /// Initialise Object Manager
        /// </summary>
        internal static void Init()
        {
            if (Prepared) return;
            _callback = Callback;
            getPtrForGuid = Memory.Reader.RegisterDelegate<ClntObjMgrObjectPtr>(new IntPtr((uint)Offsets.Functions.GetPtrForGuid));
            getActivePlayer = Memory.Reader.RegisterDelegate<ClntObjMgrGetActivePlayer>(new IntPtr((uint)Offsets.Functions.ClntObjMgrGetActivePlayer));
            ourCallback = Marshal.GetFunctionPointerForDelegate(_callback);
            Prepared = false;
        }

        /// <summary>
        /// Enumerate through the object manager
        /// true if ingame
        /// false if not ingame
        /// </summary>
        internal static bool EnumObjects()
        {
            // renew playerptr if invalid
            // return if no pointer can be retrieved
            ulong playerGuid = getActivePlayer();
            if (playerGuid == 0) return false;
            IntPtr playerObject = getPtrForGuid(-1, playerGuid);
            if (playerObject == IntPtr.Zero) return false;
            if (Player == null || playerObject != Player.Pointer)
                Player = new LocalPlayer(playerGuid, playerObject);

            // set the pointer of all objects to 0
            foreach (var obj in _Objects.Values)
                obj.Pointer = IntPtr.Zero;

            EnumVisibleObjects(ourCallback, -1);

            // remove the pointer that are stil zero 
            // (pointer not updated from 0 = object not in manager anymore)
            foreach (var pair in _Objects.Where(p => p.Value.Pointer == IntPtr.Zero).ToList())
                _Objects.Remove(pair.Key);

            // assign dictionary to list which is viewable from internal
            Objects = _Objects.Values.ToList();
            return true;
        }

        /// <summary>
        /// The callback for EnumVisibleObjects
        /// </summary>
        private static int Callback(int filter, ulong guid)
        {
            try
            {
                if (guid == 0) return 0;
                IntPtr ptr = getPtrForGuid(filter, guid);
                if (ptr == IntPtr.Zero) return 0;
                if (_Objects.ContainsKey(guid))
                {
                    _Objects[guid].Pointer = ptr;
                }
                else
                {
                    var objType = Memory.Reader.Read<byte>(IntPtr.Add(ptr, (int)Offsets.ObjectManager.ObjType));
                    switch (objType)
                    {
                        case (byte)Enums.ObjTypes.OT_CONTAINER:
                        case (byte)Enums.ObjTypes.OT_ITEM:
                            WoWItem tmpItem = new WoWItem(guid, ptr);
                            ////Console.WriteLine(tmpItem.Name + " " + guid + " " + ptr.ToString("X8"));
                            _Objects.Add(guid, tmpItem);
                            break;

                        case (byte)Enums.ObjTypes.OT_UNIT:
                            WoWUnit tmpUnit = new WoWUnit(guid, ptr);
                            ////Console.WriteLine(ptr.ToString("X8") + " " + guid);
                            _Objects.Add(guid, tmpUnit);
                            break;

                        case (byte)Enums.ObjTypes.OT_PLAYER:
                            WoWPlayer tmpPlayer = new WoWPlayer(guid, ptr);
                            _Objects.Add(guid, tmpPlayer);
                            break;

                        case (byte)Enums.ObjTypes.OT_GAMEOBJ:
                            WoWGameObject tmpGameObject = new WoWGameObject(guid, ptr);
                            _Objects.Add(guid, tmpGameObject);
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                // ignored
            }
            return 1;
        }
    }
}
