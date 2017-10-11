using System;
using System.Collections.Generic;
using System.Text;
using ClassicFramework.Constants;
using ClassicFramework.Mem;
using ClassicFramework.Objects;

namespace ClassicFramework.Ingame
{
    /// <summary>
    /// Class holding one spell
    /// </summary>
    class Spell
    {
        internal Spell(IntPtr parDbcEntry, int spellId)
        {
            DBCEntry = parDbcEntry;
            SpellId = spellId;
        }

        /// <summary>
        /// Pointer to the entry
        /// </summary>
        private IntPtr DBCEntry { get; set; }
        internal int SpellId { get; private set; }

        /// <summary>
        /// Name of the Spell
        /// </summary>
        internal string Name
        {
            get
            {
                IntPtr ptr = Memory.Reader.Read<IntPtr>(IntPtr.Add(DBCEntry, 0x1E0));
                return Memory.Reader.ReadString(ptr, Encoding.ASCII);
            }
        }

        internal int Rank
        {
            get
            {
                IntPtr ptr = Memory.Reader.Read<IntPtr>(IntPtr.Add(DBCEntry, 0x204));
                string tmpStr = Memory.Reader.ReadString(ptr, Encoding.ASCII);
                if (!tmpStr.Contains("Rank")) return 0;
                return Convert.ToInt32(tmpStr.Split(' ')[1].Trim());
            }
        }
    }

    internal class Spells
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal Spells()
        {
            Spellbook = new Dictionary<int, Spell>();
            _Spellbook = new Dictionary<string, List<Spell>>(StringComparer.OrdinalIgnoreCase);
            FetchAllSpells();

            // Place Wand on Actionbar 23
            Memory.Reader.Write<uint>((IntPtr)0x00BC69D8, 5019);
            // Place Attack on Actionbar 24
            Memory.Reader.Write<uint>((IntPtr)0x00BC69DC, 6603);
        }

        /// <summary>
        /// Dictionary will store all spells we learned with important informations (name, cooldown etc)
        /// </summary>
        private Dictionary<int, Spell> Spellbook;
        private Dictionary<string, List<Spell>> _Spellbook;

        /// <summary>
        /// Get all learned spells by ID and pass them to the DBC function
        /// </summary>
        private void FetchAllSpells()
        {
            List<int> tmpList = new List<int>();
            int nextSpell = 0;
            while (true)
            {
                // read spell at the base
                int tmpSpell = Memory.Reader.Read<int>((IntPtr)0xB700F0 + nextSpell);
                // break if spell id is 0 (means we are at the end
                if (tmpSpell == 0) break;

                // does our temp list already contain the spell with the given id?
                if (!tmpList.Contains(tmpSpell))
                    // add spell id to list
                    tmpList.Add(tmpSpell);
                // add offset to the next spell
                nextSpell += 4;
            }
            // got all spells from spellbook? lets get the dbc entrys!
            FetchDbcEntry(tmpList);
        }

        /// <summary>
        /// Retrieve dbc entrys for our spells
        /// </summary>
        private void FetchDbcEntry(List<int> parSpells)
        {
            // pointer to the first dbc entry
            IntPtr firstEntry = Memory.Reader.Read<IntPtr>((IntPtr)0x00C0D780);
            // offset to the next entry
            int nextEntry = 0x2B4;
            while (true)
            {
                // get the id for the current dbc entry
                int spellIdForEntry = Memory.Reader.Read<int>(firstEntry);
                // itterate until we hit the end (0)
                if (spellIdForEntry == 0) break;

                // does our dictionary contain this spell id?
                if (parSpells.Contains(spellIdForEntry))
                {
                    // add object to dictionary (spell id is the key)
                    Spell tmpEntry = new Spell((IntPtr)firstEntry, spellIdForEntry);
                    Spellbook.Add(spellIdForEntry, tmpEntry);

                    // add object to dictionary (name is the key)
                    // value is a list containing all ids resolved to the name of the key
                    if (_Spellbook.ContainsKey(tmpEntry.Name))
                    {
                        _Spellbook[tmpEntry.Name].Add(tmpEntry);
                    }
                    else
                    {
                        List<Spell> tmpList = new List<Spell>();
                        tmpList.Add(tmpEntry);
                        _Spellbook.Add(tmpEntry.Name, tmpList);
                    }
                }
                // increase to next entry
                firstEntry = IntPtr.Add(firstEntry, nextEntry);
            }
        }

        /// <summary>
        /// Fetch single spell by ID
        /// </summary>
        internal string GetName(int parId)
        {
            if (Spellbook.ContainsKey(parId)) return Spellbook[parId].Name;
            IntPtr firstEntry = Memory.Reader.Read<IntPtr>((IntPtr)0x00C0D780);
            int nextEntry = 0x2B4;
            while (true)
            {
                int spellIdForEntry = Memory.Reader.Read<int>(firstEntry);
                if (spellIdForEntry == 0) break;
                if (spellIdForEntry == parId)
                {
                    Spell tmpEntry = new Spell((IntPtr)firstEntry, spellIdForEntry);
                    Spellbook.Add(spellIdForEntry, tmpEntry);
                    return tmpEntry.Name;
                }
                firstEntry = IntPtr.Add(firstEntry, nextEntry);
            }
            return "";
        }

        /// <summary>
        /// Retrieve the ID of the spell with the given namen (highest ID)
        /// </summary>
        internal int GetId(string parName)
        {
            if (!_Spellbook.ContainsKey(parName.Trim())) return 0;
            int lastItem = _Spellbook[parName].Count;
            return _Spellbook[parName][lastItem - 1].SpellId;
        }

        /// <summary>
        /// Cast a Spell by name
        /// </summary>
        internal void Cast(string parName)
        {
            int id = GetId(parName);
            Cast(id);
        }

        /// <summary>
        /// Cast a Spell by ID
        /// </summary>
        internal void Cast(int Id)
        {
            Functions.CastSpell(Id);
        }

        /// <summary>
        /// Is a spell ready by name
        /// </summary>
        internal bool IsSpellReady(string parName)
        {
            int id = GetId(parName);
            return Functions.IsSpellReady(id);
        }

        /// <summary>
        /// Is a spell ready by Id
        /// </summary>
        internal bool IsSpellReady(int Id)
        {
            return Functions.IsSpellReady(Id);
        }

        /// <summary>
        /// Start attacking without turning it off on second call
        /// </summary>
        internal void Attack()
        {
            Functions.DoString(Strings.Attack);
            if (Wait.For("AutoAttackTimer12", 500))
            {
                WoWUnit target = ObjectManager.Target;
                if (target == null) return;
                ObjectManager.Player.DisableCtm();
                ObjectManager.Player.RightClick(target);
                ObjectManager.Player.EnableCtm();
            }
                
        }

        /// <summary>
        /// Start attacking with Wand without turning it off on second call
        /// </summary>
        internal void StartWand()
        {
            Functions.DoString(Strings.WandStart);
        }

        /// <summary>
        /// Stop wand attack without turning it back on on second call
        /// </summary>
        internal void StopWand()
        {
            Functions.DoString(Strings.WandStop);
        }
    }
}
