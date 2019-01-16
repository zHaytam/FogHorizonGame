using System;
using Assets.Scripts.Player.Inventory.Items;

namespace Assets.Scripts.Player.Inventory
{
    public static class InventoryHelper
    {

        public static string ToSlotName(this EquipableItemType type)
        {
            switch (type)
            {
                case EquipableItemType.Weapon:
                    return "left_hand_weapon";
                case EquipableItemType.Shield:
                    return "right_hand_weapon";
                default:
                    throw new Exception("Unhandled item object type.");
            }
        }

    }
}
