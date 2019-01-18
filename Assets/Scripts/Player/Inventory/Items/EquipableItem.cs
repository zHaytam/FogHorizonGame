using System.Collections.Generic;
using Assets.Scripts.ContextMenu;
using UnityEngine;

namespace Assets.Scripts.Player.Inventory.Items
{
    [CreateAssetMenu(menuName = "Items/Equipable Item")]
    public class EquipableItem : UsableItem
    {

        #region Fields

        [SerializeField] private EquipableItemType _type;

        #endregion

        #region Properties

        public EquipableItemType Type => _type;

        #endregion

    }
}
