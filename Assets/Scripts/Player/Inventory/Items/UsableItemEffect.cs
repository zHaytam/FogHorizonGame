using System;
using UnityEngine;

namespace Assets.Scripts.Player.Inventory.Items
{
    [Serializable]
    public class UsableItemEffect
    {

        #region Fields

        [SerializeField] private UsableItemEffectType _type;
        [SerializeField] private float _value;

        #endregion

        #region Properties

        public UsableItemEffectType Type => _type;
        public float Value => _value;

        #endregion

    }
}
