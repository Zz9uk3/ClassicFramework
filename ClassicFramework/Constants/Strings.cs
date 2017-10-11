
namespace ClassicFramework.Constants
{
    internal class Strings
    {
        /// <summary>
        /// Const for Attack and Wand functions
        /// </summary>
        internal const string WandStop = "if IsAutoRepeatAction(23) == 1 then CastSpellByName('Shoot') end";
        internal const string WandStart = "if IsAutoRepeatAction(23) == nil then CastSpellByName('Shoot') end";
        internal const string Attack = "if IsCurrentAction('24') == nil then CastSpellByName('Attack') end";

        /// <summary>
        /// Const to check enchant on main or offhand
        /// </summary>
        internal const string IsMainhandEnchanted = "mainhand1 = GetWeaponEnchantInfo()";
        internal const string GT_IsMainhandEnchanted = "mainhand1";
        internal const string IsOffhandEnchanted = "_, _, _, offhand1 = GetWeaponEnchantInfo()";
        internal const string GT_IsOffhandEnchanted = "offhand1";

        /// <summary>
        /// Const to check if vendor window is open
        /// </summary>
        internal const string IsVendorOpen = "if MerchantFrame:IsVisible() then vendorSh1 = 'true' else vendorSh1 = 'false' end";
        internal const string GT_IsVendorOpen = "vendorSh1";

        internal const string RepairAll = "if MerchantRepairAllButton:IsVisible() then MerchantRepairAllButton:Click() end";

        /// <summary>
        /// Const to pickup main and offhand
        /// </summary>
        internal const string EnchantMainhand = "PickupInventoryItem(16)";
        internal const string EnchantOffhand = "PickupInventoryItem(17)";

        internal const string PosInfos = "px,py=GetPlayerMapPosition('player') posInfos = format('%s %i/%i', GetZoneText(),px *100,py *100)";
        internal const string GT_PosInfos = "posInfos";

        internal const string SkipGossip = "arg = { GetGossipOptions() }; count = 1; typ = 2; while true do if arg[typ] ~= nil then if arg[typ] == 'vendor' then SelectGossipOption(count); break; else count = count + 1; typ = typ + 2; end else break end end";
    
        internal const string CtmOn = "ConsoleExec('Autointeract 1')";
        internal const string CtmOff = "ConsoleExec('Autointeract 0')";
    }
}
