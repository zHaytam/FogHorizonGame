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
                    float oldFraction = HealthFraction;
                    _maxHealth = value;
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
