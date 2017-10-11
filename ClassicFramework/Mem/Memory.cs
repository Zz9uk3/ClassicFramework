using System;
using System.Diagnostics;
using ClassicFramework.AntiWarden;
using System.Collections;
using System.Windows.Forms;
using Binarysharp.Assemblers.Fasm;
using System.Text;
using ClassicFramework.Helpers.GreyMagic;

namespace ClassicFramework.Mem
{
    static class Memory
    {
        /// <summary>
        /// Memory Reader Instance
        /// </summary>
        internal static InProcessMemoryReader Reader;
        private static FasmNet Asm;

        /// <summary>
        /// Initialise InternalMemoryReader
        /// </summary>
        internal static void Init()
        {
            Reader = new InProcessMemoryReader(Process.GetCurrentProcess());

            Asm = new FasmNet();

            // Init the object manager
            ObjectManager.Init();
            // Apply no collision hack with trees
            Hack Collision1 = new Hack((IntPtr)0x6ABC5A, new byte[] { 0x0F, 0x85, 0x1B, 0x01, 0x00, 0x00 }, "Collision1");
            HookWardenMemScan.AddHack(Collision1);

            Hack Collision2 = new Hack((IntPtr)0x006A467B, new byte[] { 0x90, 0x90 }, "Collision2");
            HookWardenMemScan.AddHack(Collision2);

            Hack Collision3 = new Hack((IntPtr)0x006ABF13, new byte[] { 0xEB ,0x69 }, "Collision3");
            HookWardenMemScan.AddHack(Collision3);

            ////DisableCollision.Apply();

            // wallclimb hack yay :)
            Hack Wallclimb = new Hack((IntPtr)0x0080DFFC, new byte[] { 0x00, 0x00, 0x00, 0x00 }, "Wallclimb");
            HookWardenMemScan.AddHack(Wallclimb);
            
            // Loot patch
            Hack LootPatch = new Hack((IntPtr)0x004C21C0, new byte[] { 0xEB }, "LootPatch");
            HookWardenMemScan.AddHack(LootPatch);
            //LootPatch.Apply();

            // Lua Unlock
            Hack LuaUnlock = new Hack((IntPtr)0x494a57, new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xc3 }, "LuaUnlock");
            HookWardenMemScan.AddHack(LuaUnlock);
            LuaUnlock.Apply();

            Hack Superfly = new Hack((IntPtr)0x006341BC, new byte[] { 0x90, 0x90 }, "Superfly");
            HookWardenMemScan.AddHack(Superfly);
            Hack Antijump = new Hack((IntPtr)0x007C625F, new byte[] { 0xEB }, "Antijump");
            HookWardenMemScan.AddHack(Antijump);

            // See all levels & no language barrier
            Hack SeeAllLevels = new Hack((IntPtr)0x5EC720, new byte[] { 0xC2, 0x08, 0x00 }, new byte[] { 0x55, 0x8b, 0xec }, "SeeAllLevels");
            HookWardenMemScan.AddHack(SeeAllLevels);
            SeeAllLevels.Apply();
            // crasher

            Hack UnderstandAll = new Hack((IntPtr)0x518062, new byte[] { 0xEB }, new byte[] { 0x7F }, "UnderstandAll");
            HookWardenMemScan.AddHack(UnderstandAll);
            UnderstandAll.Apply();
        }

        internal static Hack GetHack(string parName)
        {
            return HookWardenMemScan.GetHack(parName);
        }

        //internal static void InjectAsm(uint parPtr, string[] parInstructions, string parPatchName = "")
        //{
        //    if (Asm == null) Asm = new FasmNet();

        //    Asm.Clear();
        //    IntPtr start = new IntPtr(parPtr);
        //    Asm.AddLine("use32");
        //    foreach (string x in parInstructions)
        //    {
        //        Asm.AddLine(x);
        //    }

        //    byte[] byteCode = new byte[0];
        //    try
        //    {
        //        byteCode = Asm.Assemble(start);
        //    }
        //    catch (FasmAssemblerException ex)
        //    {
        //        MessageBox.Show(String.Format("Error definition: {0}; Error code: {1}; Error line: {2}; Error offset: {3}; Mnemonics: {4}",
        //            ex.ErrorCode, (int)ex.ErrorCode, ex.ErrorLine, ex.ErrorOffset, ex.Mnemonics));
        //    }
        //    byte[] originalBytes = Memory.Reader.ReadBytes((IntPtr)start, byteCode.Length);
        //    Memory.Reader.WriteBytes(start, byteCode);

        //    if (parPatchName != "")
        //    {
        //        Hack parHack = new Hack(start,
        //            byteCode,
        //            originalBytes, parPatchName);
        //        HookWardenMemScan.AddHack(parHack);
        //    }
        //}

        //[Obfuscation(Feature = "virtualization", Exclude = false)]
        internal static IntPtr InjectAsm(string[] parInstructions, string parPatchName)
        {
            if (Asm == null) Asm = new FasmNet();

            Asm.Clear();
            Asm.AddLine("use32");
            foreach (var x in parInstructions)
            {
                Asm.AddLine(x);
            }

            var byteCode = new byte[0];
            try
            {
                byteCode = Asm.Assemble();
            }
            catch (FasmAssemblerException ex)
            {
                MessageBox.Show(
                    $"Error definition: {ex.ErrorCode}; Error code: {(int)ex.ErrorCode}; Error line: {ex.ErrorLine}; Error offset: {ex.ErrorOffset}; Mnemonics: {ex.Mnemonics}");
            }

            var start = Reader.Alloc(byteCode.Length);
            Asm.Clear();
            Asm.AddLine("use32");
            foreach (var x in parInstructions)
            {
                Asm.AddLine(x);
            }
            byteCode = Asm.Assemble(start);

            HookWardenMemScan.RemoveHack(start);
            HookWardenMemScan.RemoveHack(parPatchName);
            var originalBytes = Reader.ReadBytes(start, byteCode.Length);
            if (parPatchName != "")
            {
                var parHack = new Hack(start,
                    byteCode,
                    originalBytes, parPatchName);
                HookWardenMemScan.AddHack(parHack);
                Console.WriteLine($"Protecting {parHack.Name} from Warden at {parHack.address.ToString("X")}");
                parHack.Apply();
            }
            else
            {
                Reader.WriteBytes(start, byteCode);
            }
            return start;
        }

        //[Obfuscation(Feature = "virtualization", Exclude = false)]
        internal static void InjectAsm(uint parPtr, string parInstructions, string parPatchName)
        {
            Asm.Clear();
            Asm.AddLine("use32");
            Asm.AddLine(parInstructions);
            var start = new IntPtr(parPtr);

            byte[] byteCode;
            try
            {
                byteCode = Asm.Assemble(start);
            }
            catch (FasmAssemblerException ex)
            {
                MessageBox.Show(
                    $"Error definition: {ex.ErrorCode}; Error code: {(int)ex.ErrorCode}; Error line: {ex.ErrorLine}; Error offset: {ex.ErrorOffset}; Mnemonics: {ex.Mnemonics}");
                return;
            }

            HookWardenMemScan.RemoveHack(start);
            HookWardenMemScan.RemoveHack(parPatchName);
            var originalBytes = Reader.ReadBytes(start, byteCode.Length);
            if (parPatchName != "")
            {
                var parHack = new Hack(start,
                    byteCode,
                    originalBytes, parPatchName);
                HookWardenMemScan.AddHack(parHack);
                Console.WriteLine($"Protecting {parHack.Name} from Warden at {parHack.address.ToString("X")}");
                parHack.Apply();
            }
            else
            {
                Reader.WriteBytes(start, byteCode);
            }
        }
    }
}
