using System;
using System.Runtime.InteropServices;
using System.Text;
using ClassicFramework.Constants;

namespace ClassicFramework.Mem
{
    internal static class Functions
    {
        /// <summary>
        /// DoString: Delegate + Function
        /// </summary>
        [DllImport("FastCallDll.dll", EntryPoint = "_DoString", CallingConvention = CallingConvention.StdCall)]
        private static extern void _DoString(string parLuaCode);

        internal static void DoString(string parLuaCode)
        {
            _DoString(parLuaCode);
            //DoString_Stub(new IntPtr((uint)Offsets.functions.DoString), parLuaCode, parScriptName);
        }

        [DllImport("FastCallDll.dll", EntryPoint = "_RegFunc", CallingConvention = CallingConvention.StdCall)]
        private static extern void _RegFunc(string parFuncName, uint parFuncPtr);

        internal static void RegisterFunction(string parFuncName, uint parFuncPtr)
        {
            _RegFunc(parFuncName, parFuncPtr);
            //DoString_Stub(new IntPtr((uint)Offsets.functions.DoString), parLuaCode, parScriptName);
        }

        [DllImport("FastCallDll.dll", EntryPoint = "_UnregFunc", CallingConvention = CallingConvention.StdCall)]
        private static extern void _UnregFunc(string parFuncName, uint parFuncPtr);

        internal static void UnregisterFunction(string parFuncName, uint parFuncPtr)
        {
            _UnregFunc(parFuncName, parFuncPtr);
            //DoString_Stub(new IntPtr((uint)Offsets.functions.DoString), parLuaCode, parScriptName);
        }

        [DllImport("FastCallDll.dll", EntryPoint = "_LuaToNumber", CallingConvention = CallingConvention.StdCall)]
        private static extern double _LuaToNumber(IntPtr parLuaState, int number);

        [DllImport("FastCallDll.dll", EntryPoint = "_LuaToString", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr _LuaToString(IntPtr parLuaState, int number);

        [DllImport("FastCallDll.dll", EntryPoint = "_LuaIsString", CallingConvention = CallingConvention.StdCall)]
        private static extern int _LuaIsString(IntPtr parLuaState, int number);

        [DllImport("FastCallDll.dll", EntryPoint = "_LuaIsNumber", CallingConvention = CallingConvention.StdCall)]
        private static extern int _LuaIsNumber(IntPtr parLuaState, int number);

        internal static bool LuaIsString(IntPtr parLuaState, int number)
        {
            return (_LuaIsString(parLuaState, number) == 1 ? true : false);
        }

        internal static bool LuaIsNumber(IntPtr parLuaState, int number)
        {
            return (_LuaIsNumber(parLuaState, number) == 1 ? true : false);
        }

        /// <summary>
        /// GetText: Delegate + Function
        /// </summary>
        [DllImport("FastCallDll.dll", EntryPoint = "_GetText", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr _GetText(string varName);
        internal static string GetText(string parVarName)
        {
            IntPtr addr = _GetText(parVarName);//GetText_Stub(new IntPtr((uint)Offsets.functions.GetText), "hallo12", 0xFFFFFFFF, 0);
            return Memory.Reader.ReadString(addr, Encoding.ASCII);
        }

        /// <summary>
        /// Basic movement: GetActive CGInputControl
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr CGInputControl__GetActiveDelegate();
        private static CGInputControl__GetActiveDelegate CGInputControl__GetActiveFunction;
        private static IntPtr CGInputControl__GetActive()
        {
            if (CGInputControl__GetActiveFunction == null)
                CGInputControl__GetActiveFunction = Memory.Reader.RegisterDelegate<CGInputControl__GetActiveDelegate>((IntPtr)Constants.Offsets.Functions.CGInputControl__GetActive);
            return CGInputControl__GetActiveFunction();
        }

        /// <summary>
        /// Basic movement: Set CGInputControlBit
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void SetControlBitDelegate(IntPtr device, int bit, int state, int tickCount);
        private static SetControlBitDelegate SetControlBitFunction;
        internal static void SetControlBit(int parBit, int parState, int parTickCount)
        {
            if (SetControlBitFunction == null)
                SetControlBitFunction = Memory.Reader.RegisterDelegate<SetControlBitDelegate>((IntPtr)Constants.Offsets.Functions.CGInputControl__SetControlBit);
            IntPtr ptr = CGInputControl__GetActive();
            SetControlBitFunction(ptr, parBit, parState, parTickCount);
        }

        /// <summary>
        /// Basic movement: Set Facing
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void SetFacingDelegate(IntPtr playerPtr, float facing);
        private static SetFacingDelegate SetFacingFunction;
        internal static void SetFacing(IntPtr parPlayerPtr, float parFacing)
        {
            if (SetFacingFunction == null)
                SetFacingFunction = Memory.Reader.RegisterDelegate<SetFacingDelegate>((IntPtr)Constants.Offsets.Functions.SetFacing);
            SetFacingFunction(parPlayerPtr, parFacing);
        }

        /// <summary>
        /// Send a movement update
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void SendMovementUpdateDelegate(IntPtr playerPtr, int timestamp, int opcode, int zero, int zero2);
        private static SendMovementUpdateDelegate SendMovementUpdateFunction;
        internal static void SendMovementUpdate(IntPtr parPlayerPtr, int parTimeStamp, int parOpcode)
        {
            if (SendMovementUpdateFunction == null)
                SendMovementUpdateFunction = Memory.Reader.RegisterDelegate<SendMovementUpdateDelegate>((IntPtr)Constants.Offsets.Functions.SendMovementPacket);
            SendMovementUpdateFunction(parPlayerPtr, parTimeStamp, parOpcode, 0, 0);
        }

        /// <summary>
        /// Interact with Unit
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void OnRightClickUnitDelegate(IntPtr unitPtr, int autoLoot);
        private static OnRightClickUnitDelegate OnRightClickUnitFunction;
        internal static void OnRightClickUnit(IntPtr parPlayerPtr, int parAutoLoot)
        {
            if (OnRightClickUnitFunction == null)
                OnRightClickUnitFunction = Memory.Reader.RegisterDelegate<OnRightClickUnitDelegate>((IntPtr)Constants.Offsets.Functions.OnRightClickUnit);
            OnRightClickUnitFunction(parPlayerPtr, parAutoLoot);
        }

        /// <summary>
        /// Interact with Object
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void OnRightClickObjectDelegate(IntPtr unitPtr, int autoLoot);
        private static OnRightClickObjectDelegate OnRightClickObjectFunction;
        internal static void OnRightClickObject(IntPtr parPlayerPtr, int parAutoLoot)
        {
            if (OnRightClickObjectFunction == null)
                OnRightClickObjectFunction = Memory.Reader.RegisterDelegate<OnRightClickObjectDelegate>((IntPtr)Constants.Offsets.Functions.OnRightClickObject);
            OnRightClickObjectFunction(parPlayerPtr, parAutoLoot);
        }

        /// <summary>
        /// Are we looting?
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate byte IsLootingDelegate(IntPtr playerPtr);
        private static IsLootingDelegate IsLootingFunction;
        internal static bool IsLooting(IntPtr parPlayerPtr)
        {
            if (IsLootingFunction == null)
                IsLootingFunction = Memory.Reader.RegisterDelegate<IsLootingDelegate>((IntPtr)Constants.Offsets.Functions.IsLooting);

            return IsLootingFunction(parPlayerPtr) == 1;
        }

        /// <summary>
        /// Lets loot everything
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void LootAllDelegate();
        private static LootAllDelegate LootAllFunction;
        internal static void LootAll()
        {
            if (LootAllFunction == null)
                LootAllFunction = Memory.Reader.RegisterDelegate<LootAllDelegate>((IntPtr)Constants.Offsets.Functions.AutoLoot);
            LootAllFunction();
        }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int UnitReactionDelegate(IntPtr unitPtr1, IntPtr unitPtr2);
        private static UnitReactionDelegate UnitReactionFunction;
        internal static int UnitReaction(IntPtr unitPtr1, IntPtr unitPtr2)
        {
            if (UnitReactionFunction == null)
                UnitReactionFunction = Memory.Reader.RegisterDelegate<UnitReactionDelegate>((IntPtr)0x006061E0);
            return UnitReactionFunction(unitPtr1, unitPtr2);
        }

        /// <summary>
        /// How many items can we loot?
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int OpenLootSlotsDelegate();
        private static OpenLootSlotsDelegate OpenLootSlotsFunction;
        internal static int OpenLootSlots()
        {
            if (OpenLootSlotsFunction == null)
                OpenLootSlotsFunction = Memory.Reader.RegisterDelegate<OpenLootSlotsDelegate>((IntPtr)Constants.Offsets.Functions.GetLootSlots);
            return OpenLootSlotsFunction();
        }

        /// <summary>
        /// Get the current map id
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetMapDelegate();
        private static GetMapDelegate GetMapFunction;
        internal static int GetMap()
        {
            if (GetMapFunction == null)
                GetMapFunction = Memory.Reader.RegisterDelegate<GetMapDelegate>((IntPtr)Constants.Offsets.Functions.ClntObjMgrGetMapId);
            return GetMapFunction();
        }

        /// <summary>
        /// Set the target by guid
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetTargetDelegate(ulong guid);
        private static SetTargetDelegate SetTargetFunction;
        internal static void SetTarget(ulong parGuid)
        {
            if (SetTargetFunction == null)
                SetTargetFunction = Memory.Reader.RegisterDelegate<SetTargetDelegate>((IntPtr)0x493540);
            SetTargetFunction(parGuid);
        }

        /// <summary>
        /// Intersect between two points
        /// </summary>
        [DllImport("FastCallDll.dll", EntryPoint = "_Intersect", CallingConvention = CallingConvention.StdCall)]
        private static extern byte _Intersect(ref XYZXYZ points, ref float distance, ref Intersection intersection, int flags);

        internal static byte Intersect(Vector3 parStart, Vector3 parEnd)
        {
            XYZXYZ points = new XYZXYZ(parStart.X, parStart.Y, parStart.Z,
                parEnd.X, parEnd.Y, parEnd.Z);
            points.Z1 += 2;
            points.Z2 += 2;
            Intersection intersection = new Intersection();
            float distance = parStart.DistanceTo(parEnd);
            int flags = 0x100111;
            return _Intersect(ref points, ref distance, ref intersection, flags);
        }

        /// <summary>
        /// Get Item from Cache
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr ItemCacheGetRowDelegate(IntPtr fixedPtr, int itemId, IntPtr _fixedPtr, int zero, int _zero, int __zero);
        private static ItemCacheGetRowDelegate ItemCacheGetRowFunction;
        internal static IntPtr ItemCacheGetRow(int parItemId)
        {
            if (ItemCacheGetRowFunction == null)
                ItemCacheGetRowFunction = Memory.Reader.RegisterDelegate<ItemCacheGetRowDelegate>(new IntPtr(0x0055BA30));
            return ItemCacheGetRowFunction(new IntPtr(0x00C0E2A0), parItemId, new IntPtr(0x0018F1E0), 0, 0, 0);
        }


        /// <summary>
        /// GETSPELLCOOLDOWN
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void GetSpellCooldownDelegate(IntPtr spellCooldownPtr, int spellId, int zero, ref int first, ref int second, ref bool third);
        private static GetSpellCooldownDelegate GetSpellCooldownFunction;
        internal static bool IsSpellReady(int spellId)
        {
            if (GetSpellCooldownFunction == null)
                GetSpellCooldownFunction = Memory.Reader.RegisterDelegate<GetSpellCooldownDelegate>(new IntPtr(0x006E13E0));

            int CdDuration = 0;
            int CdStartedAt = 0;
            bool third = false;
            GetSpellCooldownFunction(new IntPtr(0x00CECAEC), spellId, 0, ref CdDuration, ref CdStartedAt, ref third);
            return (CdDuration == 0 || CdStartedAt == 0);
        }

        [DllImport("FastCallDll.dll", EntryPoint = "_CastSpell", CallingConvention = CallingConvention.StdCall)]
        private static extern void _CastSpell(int SpellId);
        internal static void CastSpell(int spellId)
        {
            _CastSpell(spellId);
        }

        /// <summary>
        /// Use an item
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void UseItemDelegate(IntPtr ptr, ref IntPtr zeroPtr, int zero);
        private static UseItemDelegate UseItemFunction;
        internal static void UseItem(IntPtr ptr)
        {
            if (UseItemFunction == null)
                UseItemFunction = Memory.Reader.RegisterDelegate<UseItemDelegate>((IntPtr)0x005D8D00);
            IntPtr tmpPtr = IntPtr.Zero;
            UseItemFunction(ptr, ref tmpPtr, 0);
        }

        /// <summary>
        /// ClickToMove
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void CtmDelegate
        (IntPtr playerPtr, uint clickType, ref ulong interactGuidPtr, ref XYZ posPtr, float precision);
        private static CtmDelegate CtmFunction;
        internal static void Ctm(IntPtr parPlayerPtr, Enums.CtmType parType, XYZ parPosition, ulong parGuid)
        {
            if (CtmFunction == null)
                CtmFunction = Memory.Reader.RegisterDelegate<CtmDelegate>((IntPtr)0x00611130);
            ulong guid = parGuid;
            CtmFunction(parPlayerPtr, (uint)parType, ref guid,
                ref parPosition, 2);
        }

        /// <summary>
        /// QueryDbCreatureCache
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate IntPtr DbQueryCreatureCacheDelegate
        (IntPtr ptrToCache, int IdOfCreature, int randomShit, int randomShit2, int randomShit3, int randomShit4 );
        private static DbQueryCreatureCacheDelegate DbQueryCreatureCacheFunction;
        internal static IntPtr DbQueryCreatureCache(int parCreatureId)
        {
            if (DbQueryCreatureCacheFunction == null)
                DbQueryCreatureCacheFunction = Memory.Reader.RegisterDelegate<DbQueryCreatureCacheDelegate>((IntPtr)0x00556AA0);
            return DbQueryCreatureCacheFunction((IntPtr)0x00C0E354, parCreatureId, 0, 0, 0, 0);
        }

        /// <summary>
        /// NetClientSend
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void NetClientSendDelegate
        (IntPtr clientConn, int pDataStore);
        private static NetClientSendDelegate NetClientSendFunction;
        internal static void NetClientSend(IntPtr pDataStore)
        {
            if (NetClientSendFunction == null)
                NetClientSendFunction = Memory.Reader.RegisterDelegate<NetClientSendDelegate>((IntPtr)Offsets.Functions.NetClientSend);
            NetClientSendFunction(ClientConnection(), pDataStore.ToInt32());
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr ClientConnectionDelegate
        ();
        private static ClientConnectionDelegate ClientConnectionFunction;
        internal static IntPtr ClientConnection()
        {
            if (ClientConnectionFunction == null)
                ClientConnectionFunction = Memory.Reader.RegisterDelegate<ClientConnectionDelegate>((IntPtr)0x005AB490);
            return ClientConnectionFunction();
        }



        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate int LuaGetArgCountDelegate
        (IntPtr parLuaState);
        private static LuaGetArgCountDelegate LuaGetArgCountFunction;
        internal static int LuaGetArgCount(IntPtr parLuaState)
        {
            if (LuaGetArgCountFunction == null)
                LuaGetArgCountFunction = Memory.Reader.RegisterDelegate<LuaGetArgCountDelegate>((IntPtr)0x006F3070);
            return LuaGetArgCountFunction(parLuaState);
        }

        [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
        private delegate void HandleSpellTerrainDelegate
        (ref XYZ parPos);
        private static HandleSpellTerrainDelegate HandleSpellTerrainFunction;
        internal static void HandleSpellTerrain(Vector3 parPos)
        {
            if (HandleSpellTerrainFunction == null)
                HandleSpellTerrainFunction = Memory.Reader.RegisterDelegate<HandleSpellTerrainDelegate>((IntPtr)0x6E60F0);
            XYZ pos = new XYZ(parPos.X, parPos.Y, parPos.Z);
            HandleSpellTerrainFunction(ref pos);
        }

        internal static double LuaToNumber(IntPtr parLuaState, int number)
        {
            return _LuaToNumber(parLuaState, number);
        }

        internal static string LuaToString(IntPtr parLuaState, int number)
        {
            IntPtr ptr = _LuaToString(parLuaState, number);
            return Memory.Reader.ReadString(ptr, Encoding.ASCII);
        }
    }
}
