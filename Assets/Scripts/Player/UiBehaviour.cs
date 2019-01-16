using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
namespace Assets.Scripts.Player
{
    public class UiBehaviour : MonoBehaviour
    {

        #region Fields

        [Header("Health")]
        [SerializeField] private Image _healthBar;
        [SerializeField] private Text _healthText;
        [SerializeField] private float _healthLerpTime;
        private Coroutine _lerpHealthCoroutine;

        [Header("Mana")]
        [SerializeField] private Image _manaBar;
        [SerializeField] private float _manaLerpTime;
        private Coroutine _lerpManaCoroutine;


        #endregion

        #region Unity Methods

        private void Awake()
        {
            // Stats
            // -- Health
            PlayerBehaviour.Instance.Stats.CurrentHealthChanged += Stats_CurrentHealthChanged;
            PlayerBehaviour.Instance.Stats.MaxHealthChanged += Stats_MaxHealthChanged;
            PlayerBehaviour.Instance.Stats.CurrentManaChanged += LerpManaBar;

            _healthText.text = $"{PlayerBehaviour.Instance.Stats.CurrentHealth} / {PlayerBehaviour.Instance.Stats.MaxHealth}";
        }

        private void Update()
        {
            // Todo: Only for tests
            if (Input.GetKeyDown(KeyCode.T))
            {
                PlayerBehaviour.Instance.Stats.CurrentHealth -= 15;
                PlayerBehaviour.Instance.Stats.CurrentMana -= 15;
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                PlayerBehaviour.Instance.Stats.CurrentHealth += 15;
                PlayerBehaviour.Instance.Stats.CurrentMana += 15;
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                PlayerBehaviour.Instance.Stats.MaxHealth += 15;
                PlayerBehaviour.Instance.Stats.MaxMana += 15;
            }
        }

        #endregion

        #region Event Handlers

        private void Stats_CurrentHealthChanged(float oldFraction, float newFraction)
        {
            _healthText.text = $"{PlayerBehaviour.Instance.Stats.CurrentHealth} / {PlayerBehaviour.Instance.Stats.MaxHealth}";

            // Stop previous coroutine if needed
            if (_lerpHealthCoroutine != null)
            {
                StopCoroutine(_lerpHealthCoroutine);
                _healthBar.fillAmount = oldFraction;
                _lerpHealthCoroutine = null;
            }

            _lerpHealthCoroutine = StartCoroutine(Lerp.OverTime(oldFraction, newFraction, _healthLerpTime, val =>
            {
                // Overtime action
                _healthBar.fillAmount = val;
            }));
        }

        private void Stats_MaxHealthChanged(float oldValue, float newFraction)
        {
            _healthText.text = $"{PlayerBehaviour.Instance.Stats.CurrentHealth} / {PlayerBehaviour.Instance.Stats.MaxHealth}";
        }

        private void LerpManaBar(float oldFraction, float newFraction)
        {
            // Stop previous coroutine if needed
            if (_lerpManaCoroutine != null)
            {
                StopCoroutine(_lerpManaCoroutine);
                _manaBar.fillAmount = oldFraction;
                _lerpManaCoroutine = null;
            }

            // Took damage?
            if (newFraction < oldFraction)
            {
                _lerpManaCoroutine = StartCoroutine(Lerp.OverTime(oldFraction, newFraction, _manaLerpTime, val =>
                {
                    // Overtime action
                    _manaBar.fillAmount = val;
                }));
            }
        }

        #endregion

    }
}
