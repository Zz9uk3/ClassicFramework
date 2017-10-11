
namespace ClassicFramework.Constants
{
    internal static class Enums
    {
        /// <summary>
        /// Enums for WoW, no commenting needed
        /// </summary>

        internal enum ErrorCodes : int
        {
            CastOutOfRange = 0x131,
            AttackOutOfRange = 0xD8,
            SpellFailed = 0x2c,
            OutOfMana = 0x11F,
            CantCarryMoreOfThose = 0x12
        }

        internal enum ObjTypes : byte
        {
            OT_NONE = 0,
            OT_ITEM = 1,
            OT_CONTAINER = 2,
            OT_UNIT = 3,
            OT_PLAYER = 4,
            OT_GAMEOBJ = 5,
            OT_DYNOBJ = 6,
            OT_CORPSE = 7,
        }

        internal enum DynamicFlags : uint
        {
            tagged = 0x4,
            isDeadMobMine = 0x1
        }

        internal enum MovementFlags : uint
        {
            None = 0x00000000,
            Forward = 0x00000001,
            Back = 0x00000002,
            TurnLeft = 0x00000010,
            TurnRight = 0x00000020,
            Stunned = 0x00001000,
            Swimming = 0x00200000,
        }

        internal enum ControlBits : uint
        {
            Front = 0x10,
            CtmWalk = 0x1,
            AutoRun = 0x1000,
            Right = 0x200,
            Left = 0x100,
            Back = 0x20
        }

        internal enum ChatType : int
        {
            Say = 0,
            Yell = 5,
            Channel = 14,
            Group = 1,
            Guild = 3,
            Whisper = 7
        }

        internal enum LoginState
        {
            login,
            charselect
        }

        internal enum UnitReaction : uint
        {
            Neutral = 3,
            Friendly = 4,
            Hostile = 1,
        }

        internal enum ClassIds : byte
        {
            Warrior = 1,
            Paladin = 2,
            Hunter = 3,
            Rogue = 4,
            Priest = 5,
            Shaman = 7,
            Mage = 8,
            Warlock = 9,
            Druid = 11
        }

        internal enum MovementOpCodes : uint
        {
            stopTurn = 0xBE,
            turnLeft = 0xBC,
            turnRight = 0xBD,

            moveStop = 0xB7,
            moveFront = 0xB5,
            moveBack = 0xB6,

            setFacing = 0xDA,

            heartbeat = 0xEE,

            strafeLeft = 0xB8,
            strafeRightStart = 0xB9,
            strafeStop = 0xBA,
        }

        internal enum ItemQuality : int
        {
            Grey = 0,
            White = 1,
            Green = 2,
            Blue = 3,
            Epic = 4
        }

        internal enum CtmType : uint
        {
            FaceTarget = 0x1,
            Face = 0x2,
            /// <summary>
            /// Will throw a UI error. Have not figured out how to avoid it!
            /// </summary>
            // ReSharper disable InconsistentNaming
            Stop_ThrowsException = 0x3,
            // ReSharper restore InconsistentNaming
            Move = 0x4,
            NpcInteract = 0x5,
            Loot = 0x6,
            ObjInteract = 0x7,
            FaceOther = 0x8,
            Skin = 0x9,
            AttackPosition = 0xA,
            AttackGuid = 0xB,
            ConstantFace = 0xC,
            None = 0xD,
            Attack = 0x10,
            Idle = 0x13,
        }
    }
}
