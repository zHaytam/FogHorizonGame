using Assets.Scripts.Effects;
using UnityEngine;

namespace Assets.Scripts.Player.Inventory.Items
{
    [CreateAssetMenu(menuName = "Items/Usable Item")]
    public class UsableItem : Item
    {

        #region Fields

        [SerializeField] private Effect[] _effects;

        #endregion

        #region Properties

        public Effect[] Effects => _effects;

        #endregion

    }
}
