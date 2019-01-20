using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ContextMenu;
using Assets.Scripts.Player.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player.Inventory
{
    public class InventoryBehaviour : Singleton<InventoryBehaviour>
    {

        #region Fields

        private Item[] _availableItems;
        [SerializeField] private List<Item> _items; // Todo: Only for debug
        private List<EquipableItem> _equipedItems;
        private EquipableItem _currentWeapon;

        // Items
        [SerializeField] private Transform _itemsGrid;
        [SerializeField] private ItemSlotBehaviour[] _itemSlots;

        // Equipements
        [SerializeField] private Transform _equipmentsPanel;
        [SerializeField] private EquipmentSlotBehaviour[] _equipmentSlots;

        private GameObject _childUi;

        #endregion

        #region Properties

        public bool IsOpen { get; private set; }

        public EquipableItem CurrentWeapon
        {
            get => _currentWeapon;
            set
            {
                if (value == _currentWeapon)
                    return;

                string attachmentName = null;
                if (value != null)
                {
                    var equipedWeapon = GetEquipedItem(value.Type);
                    if (equipedWeapon == null || equipedWeapon != value)
                        throw new Exception("Weapon not equiped.");

                    attachmentName = equipedWeapon.AttachmentName;
                }

                _currentWeapon = value;
                PlayerBehaviour.Instance.ChangeSlotAttachment("left_hand_weapon", attachmentName);
            }
        }

        public bool HasWeapon => CurrentWeapon != null;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            // Load available items and list of items
            LoadItems();
            _childUi = transform.GetChild(0).gameObject;
            _childUi.SetActive(false);
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.I))
                return;

            _childUi.SetActive(!_childUi.activeSelf);
            IsOpen = _childUi.activeSelf;

            // Select first slot
            if (IsOpen)
            {
                _itemSlots[0].Select();
            }
            // De-select last slot
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
            }

            if (!_childUi.activeSelf)
            {
                // Reset the icons positions to not leave items floating
                foreach (var itemSlot in _itemSlots)
                {
                    itemSlot.ResetIconLocalPosition();
                }

                // Hide the context menu
                ContextMenuBehaviour.Instance.Hide();
            }
        }

        private void OnValidate()
        {
            // Todo: Should be removed
            // Item slots
            if (_itemsGrid != null)
                _itemSlots = _itemsGrid.GetComponentsInChildren<ItemSlotBehaviour>();

            // Equipment slots
            if (_equipmentsPanel != null)
                _equipmentSlots = _equipmentsPanel.GetComponentsInChildren<EquipmentSlotBehaviour>();

            RefreshItemSlots();
            RefreshEquipmentSlots();
        }

        #endregion

        #region Public Methods

        public bool HasItem(int id) => GetItem<Item>(id) != null;

        public T GetItem<T>(int id) where T : Item => (T)_items.FirstOrDefault(i => i.Id == id);

        public Item AddItem(int id)
        {
            if (IsFull())
                return null;

            var item = GetAvailableItem<Item>(id);
            if (item == null)
                throw new Exception($"Item not found {{ id: {id}}}");

            _items.Add(item);
            RefreshItemSlots();
            return item;
        }

        public Item RemoveItem(int id)
        {
            var item = GetItem<Item>(id);
            if (item == null)
                return null;

            _items.Remove(item);
            RefreshItemSlots();
            return item;
        }

        public bool IsTypeEquiped(EquipableItemType type) => GetEquipedItem(type) != null;

        public EquipableItem GetEquipedItem(EquipableItemType type) => _equipedItems.FirstOrDefault(e => e.Type == type);

        public bool EquipItem(EquipableItem item)
        {
            // Remove item from inventory
            var removedItem = RemoveItem(item.Id);
            if (removedItem == null)
                throw new Exception("Trying to equip an item that isn't in the inventory.");

            // Check if an item with the same type is already equiped
            UnequipItem(item.Type);

            // Equip the new item
            _equipedItems.Add(item);
            RefreshEquipmentSlots();

            // Activate the attachement (if not a weapon)
            if (!item.Type.IsWeapon())
                PlayerBehaviour.Instance.ChangeSlotAttachment(item.Type.ToSlotName(), item.AttachmentName);

            return true;
        }

        public bool UnequipItem(EquipableItemType type)
        {
            var item = GetEquipedItem(type);
            if (item == null)
                return false;

            if (IsFull())
                return false;

            // Put it back in the inventory
            _equipedItems.Remove(item);
            _items.Add(item);

            RefreshEquipmentSlots();
            RefreshItemSlots();

            // De-activate the attachement
            PlayerBehaviour.Instance.ChangeSlotAttachment(item.Type.ToSlotName(), null);
            return true;
        }

        public bool IsFull() => _items.Count >= _itemSlots.Length;

        public T GetAvailableItem<T>(int id) where T : Item => (T)_availableItems.FirstOrDefault(i => i.Id == id);

        #endregion

        #region Private Methods

        private void LoadItems()
        {
            _availableItems = Resources.LoadAll<Item>("Items");
            Debug.LogFormat("There are {0} available items.", _availableItems.Length);

            _items = new List<Item>(_itemSlots.Length);
            _equipedItems = new List<EquipableItem>(_equipmentSlots.Length);

            // Todo: Only for testing
            AddItem(0);
            AddItem(1);
        }

        private void RefreshItemSlots()
        {
            if (_itemSlots == null || _items == null)
                return;

            int i = 0;

            // Set the items we have
            for (; i < _items.Count && i < _itemSlots.Length; i++)
            {
                _itemSlots[i].Item = _items[i];
            }

            // Hide the rest
            for (; i < _itemSlots.Length; i++)
            {
                _itemSlots[i].Item = null;
            }
        }

        private void RefreshEquipmentSlots()
        {
            // Todo: Maybe only change the equiped/unequiped slot.

            if (_equipmentSlots == null || _equipedItems == null)
                return;

            foreach (var equipmentSlot in _equipmentSlots)
            {
                equipmentSlot.Item = GetEquipedItem(equipmentSlot.Type);
            }
        }

        #endregion

    }
}
