using System;
using System.Collections.Generic;
using System.Linq;
using ClassicFramework.Constants;
using ClassicFramework.Mem;
using ClassicFramework.Objects;

namespace ClassicFramework.Ingame
{
    internal class Inventory
    {
        /// <summary>
        /// Get free slots
        /// </summary>
        internal int FreeSlots
        {
            get
            {
                int FreeSlots = 0;
                // Itera through base bag
                for (int i = 0; i < 16; i++)
                {
                    // get guid of the item stored in current slot (i = slot number)
                    ulong tmpSlotGuid = ObjectManager.Player.GetDescriptor<ulong>(0x850 + (i * 8));
                    // current slot empty? +1 free slot
                    if (tmpSlotGuid == 0) FreeSlots++;
                }
                // List where we store guids of our equipped bags
                List<ulong> BagGuids = new List<ulong>();
                for (int i = 0; i < 4; i++)
                {
                    // read bag guid (i = bag number starting right)
                    BagGuids.Add(Memory.Reader.Read<ulong>(IntPtr.Add(new IntPtr(0xBDD060), (i * 8))));
                }
                // Filter out our bags from the item list maintained
                // by the object manager
                List<WoWItem> tmpItems = ObjectManager.Items.OfType<WoWItem>()
                        .Where(i => i.Slots != 0
                    && BagGuids.Contains(i.Guid)).ToList();

                // iterate over the bag list
                foreach (WoWItem bag in tmpItems)
                {
                    // iterate over the current bag and count free slots
                    // i = current slot
                    for (int i = 1; i < bag.Slots + 1; i++)
                    {
                        ulong tmpSlotGuid = bag.GetDescriptor<ulong>(0xC0 + i * 8);
                        if (tmpSlotGuid == 0) FreeSlots++;
                    }
                }
                // return the total free slots
                return FreeSlots;
            }
        }

        /// <summary>
        /// Get Item count
        /// </summary>
        internal int ItemCount(string ItemName)
        {
            // get all items which contain the item name provided by the function
            List<WoWItem> tmpList = ObjectManager.Items.Where(i => i.Name == ItemName).ToList();
            int tmpCount = 0;
            // iterate over them and add the stack count to the total count
            foreach (WoWItem item in tmpList)
            {
                tmpCount += item.StackCount;
            }
            return tmpCount;
        }

        /// <summary>
        /// Get durability of all equipped items in percentage
        /// </summary>
        internal int DurabilityPercentage
        {
            get
            {
                // 0x798 = descriptor to the equipped head item
                int offset = 0x798;
                List<ulong> inventoryGuids = new List<ulong>();

                // We got 19 equipped items
                for (int i = 0; i < 19; i++)
                {
                    inventoryGuids.Add(ObjectManager.Player.GetDescriptor<ulong>(offset + (0x8 * i)));
                }

                // calculate durability of equipped items in percentage
                int duraAll = 0;
                int duraMaxAll = 0;
                List<WoWItem> tmpItems = ObjectManager.Items.Where(i => inventoryGuids.Contains(i.Guid)).ToList();
                foreach (WoWItem x in tmpItems)
                {
                    duraAll += x.Durability;
                    duraMaxAll += x.MaxDurability;
                }
                if (duraMaxAll == 0) return 100;
                return (int)((float)((float)duraAll / (float)duraMaxAll) * 100);

            }
        }

        /// <summary>
        /// is mainhand enchanted
        /// </summary>
        internal bool IsMainhandEnchanted
        {
            get
            {
                Functions.DoString(Strings.IsMainhandEnchanted);
                return Functions.GetText(Strings.GT_IsMainhandEnchanted) == "1";
            }
        }

        /// <summary>
        /// is offhand enchanted
        /// </summary>
        internal bool IsOffhandEnchanted
        {
            get
            {
                Functions.DoString(Strings.IsOffhandEnchanted);
                return Functions.GetText(Strings.GT_IsOffhandEnchanted) == "1";
            }
        }

        /// <summary>
        /// enchant mainhand
        /// </summary>
        internal void EnchantMainhand(string itemName)
        {
            UseItem(itemName);
            Functions.DoString(Strings.EnchantMainhand);
        }

        /// <summary>
        /// enchant offhand
        /// </summary>
        internal void EnchantOffhand(string itemName)
        {
            UseItem(itemName);
            Functions.DoString(Strings.EnchantOffhand);
        }

        /// <summary>
        /// Use an item
        /// </summary>
        internal void UseItem(string Name)
        {
            WoWItem tmpItem = ObjectManager.Items.Where(i => String.Equals(i.Name, Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (tmpItem == null) return;
            IntPtr ptr = tmpItem.Pointer;
            //IntPtr ptr2 = tmpItem.UseItemPointer;
            Functions.UseItem(ptr);//ptr2);
        }

        internal void UseItem(WoWItem Item)
        {
            if (Item == null) return;
            //IntPtr ptr2 = tmpItem.UseItemPointer;
            Functions.UseItem(Item.Pointer);//ptr2);
        }

        internal void RepairAll()
        {
            Functions.DoString(Strings.RepairAll);
        }
    }


}
