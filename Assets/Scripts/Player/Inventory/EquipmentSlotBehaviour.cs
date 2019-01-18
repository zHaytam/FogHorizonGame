using System.Collections.Generic;
using Assets.Scripts.ContextMenu;
using Assets.Scripts.Player.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player.Inventory
{
    public class EquipmentSlotBehaviour : ItemSlotBehaviour
    {

        #region Fields

        [SerializeField] private Sprite _placeHolder;
        [SerializeField] private EquipableItemType _type;

        #endregion

        #region Properties

        public EquipableItemType Type => _type;

        #endregion

        #region Unity Methods

        protected override void OnValidate()
        {
            base.OnValidate();
            gameObject.name = $"{Type}Slot";
        }

        #endregion

        #region Protected Methods

        protected override void OnItemSet()
        {
            if (_item == null)
            {
                _icon.sprite = _placeHolder;
            }
            else
            {
                _icon.sprite = _item.Icon;
            }
        }

        protected override List<ContextMenuItem> GetContextMenuItems()
        {
            return new List<ContextMenuItem>
            {
                new ContextMenuItem("Unequip", () => InventoryBehaviour.Instance.UnequipItem(Type)),
                new ContextMenuItem("Drop", DropItem)
            };
        }

        #endregion

    }
}
