using System.Collections.Generic;
using Assets.Scripts.ContextMenu;
using Assets.Scripts.Player.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Player.Inventory
{
    public class ItemSlotBehaviour : Selectable
    {

        #region Fields

        [SerializeField] protected Image _icon;
        protected Item _item;

        #endregion

        #region Properties

        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                OnItemSet();
            }
        }

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (_item == null || currentSelectionState != SelectionState.Highlighted)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ContextMenuBehaviour.Instance.Show(GetContextMenuItems(), transform.position);
            }
        }

        #endregion

        #region Protected Methods

        protected virtual void OnItemSet()
        {
            if (_item == null)
            {
                _icon.enabled = false;
            }
            else
            {
                _icon.sprite = _item.Icon;
                _icon.enabled = true;
            }
        }

        protected virtual List<ContextMenuItem> GetContextMenuItems()
        {
            var items = new List<ContextMenuItem>();

            if (_item is EquipableItem equipableItem)
            {
                items.Add(new ContextMenuItem("Equip", () => InventoryBehaviour.Instance.EquipItem(equipableItem)));
            }

            // Todo: handle other types when needed (UsableItem)

            items.Add(new ContextMenuItem("Drop", DropItem));
            return items;
        }

        protected void DropItem()
        {

        }

        #endregion

        #region Public Methods

        public void ResetIconLocalPosition() => _icon.transform.localPosition = Vector3.zero;

        #endregion

    }
}
