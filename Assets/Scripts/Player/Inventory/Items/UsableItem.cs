using System.Collections.Generic;
using Assets.Scripts.ContextMenu;
using UnityEngine;

namespace Assets.Scripts.Player.Inventory.Items
{
    [CreateAssetMenu(menuName = "Items/Usable Item")]
    public class UsableItem : Item
    {

        #region Fields

        [SerializeField] private UsableItemEffect[] _effects;

        #endregion

        #region Properties

        public UsableItemEffect[] Effects => _effects;

        #endregion

    }
}
