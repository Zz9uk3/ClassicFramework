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
    internal static class UseSpell
    {
        // ##########################
        // Other stuff is copy paste lel
        // ##########################
        private static string funcName = "UseSpell";
        private static bool enabled = false;

        private static int FuncBody(IntPtr parLuaState)
        {
            if (!enabled)
            {
                if (Lua.ArgsCount(parLuaState) != 1 || !Lua.IsString(parLuaState, 1))
                {
                    Lua.Print("Usage is UseSpell(\\\"name\\\")");
                    return 0;
                }
                string spell = Lua.ToString(parLuaState, 1);

                if (!ObjectManager.EnumObjects()) return 0;
                target = ObjectManager.Target;
                if (target == null) return 0;

                ObjectManager.Player.Spells.Cast(spell);
                enabled = true;
                DirectX.RunInEndScene(EndScene);
                Wait.Remove("SpellTimeout");
            }
            return 0;
        }

        private static WoWUnit target;
        private static void EndScene(ref int parFrameCount)
        {
            if (Wait.For("UseSpellTimer", 100))
            {
                if (Memory.Reader.Read<int>((IntPtr)0x00CECAC0) == 64)
                {
                    Functions.HandleSpellTerrain(target.Position);
                    enabled = false;
                    DirectX.StopRunning();
                }
                else if (Wait.For("SpellTimeout", 250))
                {
                    enabled = false;
                    DirectX.StopRunning();
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
