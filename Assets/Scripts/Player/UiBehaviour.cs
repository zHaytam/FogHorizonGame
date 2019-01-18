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
        [SerializeField] private Text _manaText;
        [SerializeField] private float _manaLerpTime;
        private Coroutine _lerpManaCoroutine;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            // Stats
            PlayerBehaviour.Instance.Stats.HealthChanged += Stats_HealthChanged;
            PlayerBehaviour.Instance.Stats.ManaChanged += Stats_ManaChanged;

            // Initial texts
            _healthText.text = $"{PlayerBehaviour.Instance.Stats.CurrentHealth} / {PlayerBehaviour.Instance.Stats.MaxHealth}";
            _manaText.text = $"{PlayerBehaviour.Instance.Stats.CurrentMana} / {PlayerBehaviour.Instance.Stats.MaxMana}";

            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // Todo: Only for tests
            if (Input.GetKeyDown(KeyCode.T))
            {
                PlayerBehaviour.Instance.Stats.CurrentHealth -= 20;
                PlayerBehaviour.Instance.Stats.CurrentMana -= 15;
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                PlayerBehaviour.Instance.Stats.CurrentHealth += 15;
                PlayerBehaviour.Instance.Stats.CurrentMana += 15;
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                PlayerBehaviour.Instance.Stats.MaxHealth += 50;
                PlayerBehaviour.Instance.Stats.MaxMana += 15;
            }
        }

        #endregion

        #region Event Handlers

        private void Stats_HealthChanged(float oldFraction, float newFraction)
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

        private void Stats_ManaChanged(float oldFraction, float newFraction)
        {
            _manaText.text = $"{PlayerBehaviour.Instance.Stats.CurrentMana} / {PlayerBehaviour.Instance.Stats.MaxMana}";

            // Stop previous coroutine if needed
            if (_lerpManaCoroutine != null)
            {
                StopCoroutine(_lerpManaCoroutine);
                _manaBar.fillAmount = oldFraction;
                _lerpManaCoroutine = null;
            }

            _lerpManaCoroutine = StartCoroutine(Lerp.OverTime(oldFraction, newFraction, _manaLerpTime, val =>
            {
                // Overtime action
                _manaBar.fillAmount = val;
            }));
        }

        #endregion

    }
}
