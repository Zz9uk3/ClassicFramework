using ClassicFramework.Mem;
using ClassicFramework.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassicFramework.LuaFunctions.CustomFuncs
{
    internal static class Loot
    {
        // ##########################
        // Other stuff is copy paste lel
        // ##########################
        private static string funcName = "Loot";
        internal static bool enabled = false;

        private static int FuncBody(IntPtr parLuaState)
        {
            if (Lua.ArgsCount(parLuaState) != 0) return 0;

            if (!enabled)
            {
                Lua.Print("Starting to loot all corpses in range");
                if (ObjectManager.EnumObjects())
                {

                    List<WoWUnit> mobs = ObjectManager.Mobs;
                    lootableUnits = mobs.OfType<WoWUnit>()
                        .Where(i => i.CanBeLooted
                        && i.DistanceTo(ObjectManager.Player) < 4.20f)
                    .OrderBy(i => i.DistanceTo(ObjectManager.Player))
                    .ToList();
                    lootCounter = 0;
                    lootOpen = false;
                    enabled = true;
                    Wait.Remove("LootOpenBlacklist");
                    Wait.Remove("LootClickBlacklist");
                    AntiWarden.HookWardenMemScan.GetHack("LootPatch").Apply();
                    if (AutoLoot.enabled)
                    {
                        OnRightClickUnitHook.Remove();
                        OnRightClickObjectHook.Remove();
                    }
                    DirectX.RunInEndScene(EndScene);
                }
            }
            return 0;
        }

        private static List<WoWUnit> lootableUnits;
        private static int lootCounter;
        private static bool lootOpen;
        private static void EndScene(ref int parFrameCount)
        {
            if (lootableUnits.Count == lootCounter)
            {
                enabled = false;
                lootOpen = false;
                Wait.Remove("LootOpenBlacklist");
                Wait.Remove("LootClickBlacklist");
                DirectX.StopRunning();
                Lua.Print("We are done looting");
                if (AutoLoot.enabled)
                {
                    OnRightClickUnitHook.Apply();
                    OnRightClickObjectHook.Apply();
                }
                AntiWarden.HookWardenMemScan.GetHack("LootPatch").Remove();
            }
            else
            {
                if (Wait.For("LootUnit", 250))
                {
                    if (!Functions.IsLooting(ObjectManager.Player.Pointer))
                    {
                        if (lootOpen)
                        {
                            lootCounter++;
                            if (lootableUnits.Count == lootCounter) return;
                        }
                        Wait.Remove("LootOpenBlacklist");
                        if (Wait.For("LootClickBlacklist", 5000))
                        {
                            lootCounter++;
                            Wait.Remove("LootClickBlacklist");
                            return;
                        }
                        Functions.OnRightClickUnit(lootableUnits[lootCounter].Pointer, 0);
                        lootOpen = false;
                    }
                    else
                    {
                        lootOpen = true;
                        Wait.Remove("LootClickBlacklist");
                        Functions.LootAll();
                        if (Wait.For("LootOpenBlacklist", 5000))
                        {
                            lootCounter++;
                            Wait.Remove("LootOpenBlacklist");
                            Functions.DoString("LootCloseButton:Click()");
                            return;
                        }
                    }
                }
            }
        }
        // ##########################
        // ##########################
        // ##########################

        internal static void Init(ref uint parTrampToFunction)
        {
            FunctionHandling.Register(FuncBody, funcName, ref parTrampToFunction);
        }

        internal static void Remove(ref uint parTrampToFunction)
        {
            FunctionHandling.Remove(funcName, ref parTrampToFunction);
        }
    }
}
