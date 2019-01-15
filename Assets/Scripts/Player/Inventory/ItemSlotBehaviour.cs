using Assets.Scripts.Player.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Player.Inventory
{
    public class ItemSlotBehaviour : MonoBehaviour, IPointerClickHandler
    {

        #region Fields

        [SerializeField] private Image _icon;
        private Item _item;

        #endregion

        #region Properties

        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
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
        }

        #endregion

        #region Unity Methods

        protected virtual void OnValidate()
        {
            if (_icon != null)
                return;

            // Since the icon is a child
            _icon = transform.GetChild(0).GetComponent<Image>();
            _icon.preserveAspect = true;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if ((eventData.button != PointerEventData.InputButton.Right || eventData.clickCount != 1) && 
                (eventData.button != PointerEventData.InputButton.Left || eventData.clickCount != 2))
                return;

            if (_item is EquipableItem equipableItem)
            {
                InventoryBehaviour.Instance.EquipItem(equipableItem);
            }
        }

        #endregion

    }
}
