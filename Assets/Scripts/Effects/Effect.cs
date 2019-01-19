using System;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    [Serializable]
    public class Effect
    {

        #region Fields

        [SerializeField] private EffectType _type;
        [SerializeField] private float _value;

        #endregion

        #region Properties

        public EffectType Type => _type;
        public float Value => _value;

        #endregion
    }
}
