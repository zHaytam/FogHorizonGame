using System;

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
                    float oldValue = _maxHealth;
                    _maxHealth = value;
                    MaxHealthChanged?.Invoke(oldValue, _maxHealth);
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
                    CurrentHealthChanged?.Invoke(oldFraction, HealthFraction);
                }
            }
        }

        public float HealthFraction => (float)CurrentHealth / MaxHealth;

        #endregion

        #region Events

        public event Action<float, float> MaxHealthChanged;
        public event Action<float, float> CurrentHealthChanged;

        #endregion

    }
}
