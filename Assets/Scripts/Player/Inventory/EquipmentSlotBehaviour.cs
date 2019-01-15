using Assets.Scripts.Player.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player.Inventory
{
    public class EquipmentSlotBehaviour : ItemSlotBehaviour
    {

        #region Fields

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

        public override void OnPointerClick(PointerEventData eventData)
        {
            if ((eventData.button != PointerEventData.InputButton.Right || eventData.clickCount != 1) &&
                (eventData.button != PointerEventData.InputButton.Left || eventData.clickCount != 2))
                return;

            if (Item != null)
            {
                InventoryBehaviour.Instance.UnequipItem(Type);
            }
        }

        #endregion

    }
}
