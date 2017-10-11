using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ClassicFramework.Ingame;
using ClassicFramework;
using ClassicFramework.Constants;
using ClassicFramework.Mem;

namespace ClassicFramework.Objects
{
    internal class LocalPlayer : WoWPlayer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal LocalPlayer(UInt64 parGuid, IntPtr parPointer)
            : base(parGuid, parPointer)
        {
            Inventory = new Inventory();
            Spells = new Spells();
        }

        /// <summary>
        /// Inventory management
        /// </summary>
        internal Inventory Inventory;

        // <summary>
        /// Spell management
        /// </summary>
        internal Spells Spells;

        /// <summary>
        /// Position of corpse
        /// </summary>
        internal Vector3 CorpsePosition
        {
            get
            {
                return new Vector3(
                    Memory.Reader.Read<float>(new IntPtr(0x00B4E284)),
                    Memory.Reader.Read<float>(new IntPtr(0x00B4E288)),
                    Memory.Reader.Read<float>(new IntPtr(0x00B4E28C)));
            }
        }

        /// <summary>
        /// Movement
        /// </summary>
        internal void StartMovement(Enums.ControlBits parBits)
        {
            Functions.SetControlBit((int)parBits, 1, Environment.TickCount);
        }

        /// <summary>
        /// Stop movement
        /// </summary>
        internal void StopMovement(Enums.ControlBits parBits)
        {
            Functions.SetControlBit((int)parBits, 0, Environment.TickCount);
        }

        /// <summary>
        /// Start a ctm movement towards
        /// </summary>
        internal void CtmTo(Vector3 parPosition)
        {
           XYZ xyz = new XYZ(parPosition.X,
                parPosition.Y,
                parPosition.Z);
           Face(parPosition);
            Functions.Ctm(this.Pointer, Enums.CtmType.Move, xyz, 0);
        }

        /// <summary>
        /// Airwalk Ctm
        /// </summary>
        internal void CtmHack(Vector3 parPosition)
        {
            ApplyAirwalk();
            XYZ xyz = new XYZ(parPosition.X,
                 parPosition.Y,
                 5000);
            Face(parPosition);
            Functions.Ctm(this.Pointer, Enums.CtmType.Move, xyz, 0);
        }

        internal void ApplyAirwalk()
        {
            ObjectManager.Player.ZAxis = 5000;
            Memory.GetHack("Superfly").Apply();
            Memory.GetHack("Antijump").Apply();

            if ((MovementState & 0x00002000) == 0x00002000)
                MovementState = 0;
        }

        internal void ApplyAirwalk(float parZ)
        {
            ObjectManager.Player.ZAxis = parZ;
            Memory.GetHack("Superfly").Apply();
            Memory.GetHack("Antijump").Apply();

            if ((MovementState & 0x00002000) == 0x00002000)
                MovementState = 0;
        }

        internal void RemoveAirwalk(float parZ)
        {
            ObjectManager.Player.ZAxis = parZ;
            ObjectManager.Player.CtmStop();
            Memory.GetHack("Superfly").Remove();
            Memory.GetHack("Antijump").Remove();
        }

        /// <summary>
        /// Stop the current ctm movement
        /// </summary>
        internal void CtmStop()
        {
            if (CtmState == 12) return;
            CtmState = 12;
            StartMovement(Enums.ControlBits.Front);
            StopMovement(Enums.ControlBits.Front);
        }

        /// <summary>
        /// Get or set the current ctm state
        /// </summary>
        private int CtmState
        {
            get
            {
                return
                    Memory.Reader.Read<int>((IntPtr)0xC4D888);
            }

            set
            {
                Memory.Reader.Write<int>((IntPtr)0xC4D888, value);
            }
        }

        internal uint MovementState
        {
            get
            {
                return ReadRelative<uint>((int)Offsets.Descriptors.movementFlags);
            }
            set
            {
                Memory.Reader.Write<uint>(IntPtr.Add(this.Pointer, (int)Offsets.Descriptors.movementFlags), value);
            }
        }

        internal void EnableCtm()
        {
            Functions.DoString(Strings.CtmOn);
        }

        internal void DisableCtm()
        {
            Functions.DoString(Strings.CtmOff);
        }

        /// <summary>
        /// Rightclick on a unit
        /// </summary>
        internal void RightClick(WoWUnit parUnit)
        {
            Functions.OnRightClickUnit(parUnit.Pointer, 1);
        }
        
        /// <summary>
        /// Set Facing
        /// </summary>
        internal void Face(WoWObject parObject)
        {
            Face(parObject.Position);
        }

        /// <summary>
        /// characters current facing
        /// </summary>
        internal float Facing
        {
            get
            {
                return ReadRelative<float>(0x9C4);
            }
        }

        /// <summary>
        /// ctm face / not used right now
        /// </summary>
        internal void CtmFace(WoWObject parObject)
        {
            Vector3 tmp = parObject.Position;
            Functions.Ctm(this.Pointer, Enums.CtmType.FaceTarget,
                new XYZ(tmp.X, tmp.Y, tmp.Z), parObject.Guid);
        }

        /// <summary>
        /// facing with coordinates instead of a passed unit
        /// </summary>
        internal void Face(Vector3 parCoordinates)
        {
            float f = (float)Math.Atan2(parCoordinates.Y - Position.Y, parCoordinates.X - Position.X);
            if (f < 0.0f)
            {
                f = f + (float)Math.PI * 2.0f;
            }
            else
            {
                if (f > (float)Math.PI * 2)
                {
                    f = f - (float)Math.PI * 2.0f;
                }
            }

            double val = Math.Round(Math.Abs(f - Facing), 2);
            if (val < 0.2f) return;
            Functions.SetFacing(IntPtr.Add(base.Pointer, 0x9A8), f);
            SendMovementUpdate((int)Enums.MovementOpCodes.setFacing);
        }

        /// <summary>
        /// Send a movement update
        /// </summary>
        internal void SendMovementUpdate(int parOpCode)
        {
            Functions.SendMovementUpdate(base.Pointer, Environment.TickCount, parOpCode);
        }

        /// <summary>
        /// Update last hardware action to avoid being flagged as AFK
        /// </summary>
        internal void AntiAfk()
        {
            Memory.Reader.Write<int>((IntPtr)Offsets.Functions.LastHardwareAction, Environment.TickCount);
        }

        /// <summary>
        /// Are we looting?
        /// </summary>
        internal bool IsLooting
        {
            get
            {
                return Functions.IsLooting(base.Pointer);
            }
        }

        /// <summary>
        /// Lets loot everything
        /// </summary>
        internal void LootAll()
        {
            Functions.LootAll();
        }

        /// <summary>
        /// How many items can we loot?
        /// </summary>
        internal int OpenLootSlots
        {
            get
            {
                return Functions.OpenLootSlots();
            }
        }

        /// <summary>
        /// the ID of the map we are on
        /// </summary>
        internal int GetMapID
        {
            get
            {
                return Functions.GetMap();
            }
        }

        /// <summary>
        /// Set the target by guid
        /// </summary>
        internal void SetTarget(WoWObject parObject)
        {
            TargetGuid = parObject.Guid;
            Functions.SetTarget(parObject.Guid);
        }

        /// <summary>
        /// Are we in LoS with object?
        /// </summary>
        internal bool InLoSWith(WoWObject parObject)
        {
            // return 1 if something is inbetween the two coordinates
            //return Functions.Intersect(Position, parObject.Position) == 0;
            return false;
        }

        internal bool IsInCC
        {
            get
            {
                return (0 == Memory.Reader.Read<int>((IntPtr)0xB4B3E4));
            }
        }

        /// <summary>
        /// guid support field to get right combopoint count
        /// </summary>
        private ulong ComboPointGuid { get; set; }
        /// <summary>
        /// Get combopoints for current mob
        /// </summary>
        internal byte ComboPoints
        {
            get
            {
                IntPtr ptr1 = base.ReadRelative<IntPtr>(0xE68);
                IntPtr ptr2 = IntPtr.Add(ptr1, 0x1029);
                if (ComboPointGuid == 0)
                    Memory.Reader.Write<int>(ptr2, 0);
                byte points = Memory.Reader.Read<byte>(ptr2);
                if (points == 0)
                {
                    ComboPointGuid = TargetGuid;
                    return points;
                }
                if (ComboPointGuid != TargetGuid)
                {
                    Memory.Reader.Write<byte>(ptr2, 0);
                    return 0;
                }
                return Memory.Reader.Read<byte>(ptr2);
            }
        }

        internal bool CanOverpower
        {
            get
            {
                return (int)ComboPoints > 0;
            }
        }


        /// <summary>
        /// Get the players pet
        /// </summary>
        internal WoWUnit Pet
        {
            get
            {
                return
                    ObjectManager.Mobs.Where(i => i.SummonedBy == this.Guid).FirstOrDefault();
            }
        }
        /// <summary>
        /// Is Eating
        /// </summary>
        internal bool IsEating
        {
            get
            {
                return
                    this.GotAura("Food");
            }
        }
        /// <summary>
        /// Is Drinking
        /// </summary>
        internal bool IsDrinking
        {
            get
            {
                return
                    this.GotAura("Drink");
            }
        }

        /// <summary>
        /// the toons class
        /// </summary>
        internal Enums.ClassIds Class
        {
            get
            {
                return (Enums.ClassIds)Memory.Reader.Read<byte>((IntPtr)0xC27E81);
            }
        }

        /// <summary>
        /// Are we dead?
        /// </summary>
        internal bool IsDead
        {
            get
            {
                return (Health == 0);
            }
        }

        /// <summary>
        /// Are we in ghost form
        /// </summary>
        internal bool InGhostForm
        {
            get
            {
                return Memory.Reader.Read<byte>((IntPtr)Offsets.Player.IsGhost) == 1;
            }
        }

        internal float ZAxis
        {
            set
            {
                Memory.Reader.Write<float>(Pointer + 0x9B8 + 8, value);
            }
            get
            {
                return ReadRelative<float>(0x9B8 + 8);
            }
        }
    }
}
