using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassicFramework.Constants;
using ClassicFramework.Mem;

namespace ClassicFramework.Objects
{
    internal class WoWUnit : WoWObject
    {
        /// <summary>
        /// Constructor taking guid aswell ptr to object
        /// </summary>
        internal WoWUnit(UInt64 parGuid, IntPtr parPointer): base(parGuid, parPointer)
        {
            
        }

        /// <summary>
        /// Vector3 position of object
        /// </summary>
        internal override Vector3 Position
        {
            get
            {
                float X = base.ReadRelative<float>(0x9B8);
                float Y = base.ReadRelative<float>(0x9B8 + 4);
                float Z = base.ReadRelative<float>(0x9B8 + 8);
                return new Vector3(X, Y, Z);
            }
        }

        /// <summary>
        /// All auras on unit (only id)
        /// </summary>
        internal List<int> Auras
        {
            get
            {
                List<int> tmpAuras = new List<int>();
                int auraBase = 0xBC;
                while (true)
                {
                    int auraId = base.GetDescriptor<int>(auraBase);
                    if (auraId == 0) break;
                    tmpAuras.Add(auraId);
                    auraBase += 4;
                }
                return tmpAuras;
            }
        }

        /// <summary>
        /// All debuffs on unit
        /// </summary>
        internal List<int> Debuffs
        {
            get
            {
                List<int> tmpAuras = new List<int>();
                int auraBase = 0x13C;
                while (true)
                {
                    int auraId = base.GetDescriptor<int>(auraBase);
                    if (auraId == 0) break;
                    tmpAuras.Add(auraId);
                    auraBase += 4;
                }
                return tmpAuras;
            }
        }

        /// <summary>
        /// Distance to another object
        /// </summary>
        internal float DistanceTo(WoWObject OtherObject)
        {
            if (OtherObject == null) return float.MaxValue;
            return DistanceTo(OtherObject.Position);
        }

        internal float DistanceTo(Vector3 parPosition)
        {
            return Position.DistanceTo2D(parPosition);
                //Vector3.Distance(this.Position, parPosition);
        }

        /// <summary>
        /// Name of object
        /// </summary>
        internal override string Name
        {
            get
            {
                IntPtr ptr1 = base.ReadRelative<IntPtr>(0xB30);
                if (ptr1 == IntPtr.Zero) return "";
                IntPtr ptr2 = Memory.Reader.Read<IntPtr>(ptr1);
                if (ptr2 == IntPtr.Zero) return "";
                return Memory.Reader.ReadString(ptr2, Encoding.ASCII, 30);
            }
        }

        /// <summary>
        /// Is the unit summoned by another unit?
        /// </summary>
        internal ulong SummonedBy
        {
            get
            {
                return base.GetDescriptor<ulong>((int)Offsets.Descriptors.SummonedByGuid);
            }
        }

        /// <summary>
        /// The ID + faction ID of the NPC
        /// </summary>
        internal int NpcID
        {
            get
            {
                return base.ReadRelative<int>(0xE74);
            }
        }
        internal int FactionID
        {
            get
            {
                return base.GetDescriptor<int>((int)Offsets.Descriptors.FactionId);
            }
        }

        /// <summary>
        /// The movement state of the Unit
        /// </summary>
        internal int MovementState
        {
            get
            {
                return base.ReadRelative<int>((int)Offsets.Descriptors.movementFlags);
            }
        }

        /// <summary>
        /// Dynamic Flags of the Unit (Is lootable / Is tapped?)
        /// </summary>
        internal int DynamicFlags
        {
            get
            {
                return base.GetDescriptor<int>((int)Offsets.Descriptors.DynamicFlags);
            }
        }
        
        internal bool IsTapped
        {
            get
            {
                return (DynamicFlags & (uint)Enums.DynamicFlags.tagged) == (uint)Enums.DynamicFlags.tagged ? true : false;
            }
        }
        internal bool CanBeLooted
        {
            get
            {
                if (Health == 0)
                {
                    return (DynamicFlags & (uint)Enums.DynamicFlags.isDeadMobMine) == (uint)Enums.DynamicFlags.isDeadMobMine ? true : false;
                }
                return false;
            }
        }

        /// <summary>
        /// Health
        /// </summary>
        internal int Health
        {
            get
            {
                return base.GetDescriptor<int>((int)Offsets.Descriptors.Health);
            }
        }

        internal int MaxHealth
        {
            get
            {
                return base.GetDescriptor<int>((int)Offsets.Descriptors.MaxHealth);
            }
        }

        internal int HealthPercent
        {
            get
            {
                return (int)((float)((float)Health / (float)MaxHealth) * 100);
            }
        }

        /// <summary>
        /// Mana
        /// </summary>
        internal int Mana
        {
            get
            {
                return base.GetDescriptor<int>((int)Offsets.Descriptors.Mana);
            }
        }

        internal int MaxMana
        {
            get
            {
                return base.GetDescriptor<int>((int)Offsets.Descriptors.MaxMana);
            }
        }

        internal int ManaPercent
        {
            get
            {
                return (int)((float)((float)Mana / (float)MaxMana) * 100);
            }
        }

        /// <summary>
        /// Rage
        /// </summary>
        internal int Rage
        {
            get
            {
                return base.GetDescriptor<int>((int)Offsets.Descriptors.Rage) / 10;
            }
        }

        /// <summary>
        /// Guid of the units target
        /// </summary>
        internal ulong TargetGuid
        {
            get
            {
                return base.GetDescriptor<ulong>((int)Offsets.Descriptors.TargetGuid);
            }
            set
            {
                base.SetDescriptor<ulong>((int)Offsets.Descriptors.TargetGuid, value);
            }
        }

        /// <summary>
        /// Id of the spell the unit is casting currently
        /// </summary>
        internal int Casting
        {
            get
            {
                return base.ReadRelative<int>(0xC8C);
            }
        }

        /// <summary>
        /// Id of the spell the unit is channeling currently
        /// </summary>
        internal int Channeling
        {
            get
            {
                return base.GetDescriptor<int>((int)Offsets.Descriptors.IsChanneling);
            }
        }

        /// <summary>
        /// Interact with target
        /// </summary>
        internal void Interact(bool parAutoLoot)
        {
            Functions.OnRightClickUnit(base.Pointer, Convert.ToInt32(parAutoLoot));
        }

        /// <summary>
        /// Units reaction to the player
        /// </summary>
        internal Enums.UnitReaction Reaction
        {
            get
            {
                return (Enums.UnitReaction)Functions.UnitReaction(ObjectManager.Player.Pointer, base.Pointer);
            }
        }

        /// <summary>
        /// Got buff?
        /// </summary>
        internal bool GotAura(string parName)
        {
            List<int> tmpAuras = Auras;
            foreach (int i in tmpAuras)
            {
                bool tmpBool = String.Equals(ObjectManager.Player.Spells.GetName(i), 
                    parName, 
                    StringComparison.OrdinalIgnoreCase);
                if (tmpBool) return true;
            }
            return false;
        }

        /// <summary>
        /// Got debuff?
        /// </summary>
        internal bool GotDebuff(string parName)
        {
            List<int> tmpAuras = Debuffs;
            foreach (int i in tmpAuras)
            {
                bool tmpBool = String.Equals(ObjectManager.Player.Spells.GetName(i),
                    parName,
                    StringComparison.OrdinalIgnoreCase);
                if (tmpBool) return true;
            }
            return false;
        }

        // Is the unit a critter?
        internal bool IsCritter
        {
            get
            {
                IntPtr addrToRow = Functions.DbQueryCreatureCache(this.NpcID);
                int isCritter = Memory.Reader.Read<int>(IntPtr.Add(addrToRow, 24));
                return (8 == isCritter);
            }
        }
    }
}
