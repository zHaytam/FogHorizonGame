using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.ContextMenu
{
    public class ContextMenuBehaviour : Singleton<ContextMenuBehaviour>
    {

        #region Fields

        [SerializeField] private Button _itemButtonPrefab;
        private RectTransform _panelRectTransform;
        private Selectable _lastSelectableObject;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _mInstance = this;
            _panelRectTransform = GetComponent<RectTransform>();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            // Hide menu if Escape is pressed
            if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }
        }

        #endregion

        #region Public Methods

        public void Show(List<ContextMenuItem> items, Vector2 position)
        {
            if (gameObject.activeSelf || items.Count == 0)
                return;

            _lastSelectableObject = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
            Button firstBtn = null;

            // Set new items
            foreach (var item in items)
            {
                var btn = Instantiate(_itemButtonPrefab);
                btn.GetComponentInChildren<Text>().text = item.Text;

                if (firstBtn == null)
                    firstBtn = btn;

                // Add a listener to the button's click event to call the item's action, then hide the menu
                btn.onClick.AddListener(() =>
                {
                    item.Action();
                    Debug.Log("Hiding ContextMenu after action.");
                    Hide();
                });

                btn.transform.SetParent(gameObject.transform);
            }

            // Show menu in position
            _panelRectTransform.anchoredPosition = position;
            gameObject.SetActive(true);

            // Select first button after showing menu
            firstBtn.Select();
        }

        public void Hide()
        {
            if (!gameObject.activeSelf)
                return;

            // Clear items
            foreach (Transform child in gameObject.transform)
                Destroy(child.gameObject);

            // Hide the next frame
            StartCoroutine(CloseNextFrame());

            // Re-select previous object
            _lastSelectableObject?.Select();
        }

        #endregion

        #region Private Methods

        private IEnumerator CloseNextFrame()
        {
            yield return null;
            gameObject.SetActive(false);
        }

        #endregion

    }
}