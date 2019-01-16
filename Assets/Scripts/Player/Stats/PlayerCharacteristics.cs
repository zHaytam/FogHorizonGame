using System;

namespace Assets.Scripts.Player.Stats
{
    public class PlayerCharacteristics : BaseCharacteristics
    {

        #region Fields

        private int _maxMana;
        private int _currentMana;

        #endregion

        #region Properties

        public int MaxMana
        {
            get => _maxMana;
            set
            {
                if (value > 0 && value != _maxMana)
                {
                    float oldFraction = ManaFraction;
                    _maxMana = value;
                    ManaChanged?.Invoke(oldFraction, ManaFraction);
                }
            }
        }

        public int CurrentMana
        {
            get => _currentMana;
            set
            {
                value = value < 0 ? 0 : (value > MaxMana ? MaxMana : value);
                if (value != _currentMana)
                {
                    float oldFraction = ManaFraction;
                    _currentMana = value;
                    ManaChanged?.Invoke(oldFraction, ManaFraction);
                }
            }
        }

        public float ManaFraction => (float)CurrentMana / MaxMana;

        #endregion

        #region Events

        public event Action<float, float> ManaChanged;

        #endregion

    }
}
