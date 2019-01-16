using System;
using UnityEngine;

namespace Assets.Scripts.Player.Stats
{
    public class BaseCharacteristics
    {

        #region Fields

        private int _maxHealth;
        private int _currentHealth;

        #endregion

        #region Properties

        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                if (value > 0 && value != _maxHealth)
                {
                    float oldFraction = HealthFraction;
                    _maxHealth = value;

                    // In case the current health exceeds the maximum (e.g. un-equiping an item that
                    if (_currentHealth > _maxHealth)
                        _currentHealth = _maxHealth;

                    // Ensure the health is at the same fraction
                    else if (_currentHealth < _maxHealth && !Mathf.Approximately(oldFraction, HealthFraction))
                        _currentHealth = (int)(_maxHealth * oldFraction);

                    HealthChanged?.Invoke(oldFraction, HealthFraction);
                }
            }
        }

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                value = value < 0 ? 0 : (value > MaxHealth ? MaxHealth : value);
                if (value != _currentHealth)
                {
                    float oldFraction = HealthFraction;
                    _currentHealth = value;
                    HealthChanged?.Invoke(oldFraction, HealthFraction);
                }
            }
        }

        public float HealthFraction => (float)CurrentHealth / MaxHealth;

        #endregion

        #region Events

        public event Action<float, float> HealthChanged;

        #endregion

    }
}
