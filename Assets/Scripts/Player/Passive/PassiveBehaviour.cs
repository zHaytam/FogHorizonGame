﻿using System.Collections;
using Assets.Scripts.Player.Inventory;
using Assets.Scripts.Player.Inventory.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Player.Passive
{
    public class PassiveBehaviour : Singleton<PassiveBehaviour>
    {

        #region Fields

        [SerializeField] private Button[] _slots;
        [SerializeField] private Image[] _slotImages;
        private GameObject _child;

        #endregion

        #region Properties

        public bool IsOpen { get; private set; }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _child = transform.GetChild(0).gameObject;
            _child.SetActive(false);
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Alpha1))
                return;

            if (!IsOpen)
            {
                // Show
                _child.SetActive(true);
                IsOpen = true;

                // Set equipables
                SetWeapons();
            }
            else
            {
                Close();
            }
        }

        #endregion

        #region Private Methods

        private void SetWeapons()
        {
            var currentSlot = _slots[4];

            for (int i = 0; i < 4; i++)
            {
                var type = (EquipableItemType)i;
                var item = InventoryBehaviour.Instance.GetEquipedItem(type);

                if (InventoryBehaviour.Instance.CurrentWeapon != null && item == InventoryBehaviour.Instance.CurrentWeapon)
                    currentSlot = _slots[i];

                if (item == null)
                {
                    _slotImages[i].sprite = null;
                    _slotImages[i].enabled = false;
                }
                else
                {
                    _slotImages[i].sprite = item.Icon;
                    _slotImages[i].enabled = true;

                    // Click handle
                    _slots[i].onClick.AddListener(() =>
                    {
                        InventoryBehaviour.Instance.CurrentWeapon = item;
                        Close();
                    });
                }
            }

            // Todo: Maybe do it only once
            _slots[4].onClick.AddListener(() =>
            {
                InventoryBehaviour.Instance.CurrentWeapon = null;
                Close();
            });
    
            // Select current weapon
            currentSlot.Select();
        }

        private void Close()
        {
            // De-select
            EventSystem.current.SetSelectedGameObject(null);

            // Remove all click listeners
            foreach (var slot in _slots)
                slot.onClick.RemoveAllListeners();

            StartCoroutine(CloseNextFrame());
        }

        private IEnumerator CloseNextFrame()
        {
            yield return null;
            _child.SetActive(false);
            IsOpen = false;
        }

        #endregion

    }
}