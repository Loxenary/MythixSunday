using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    private FloatHealth _health;
    private Slider _sliderHealth;
    private TextMeshProUGUI _healthPreview;

    [SerializeField] private float AnimationDuration;
 
    private void Awake()
    {
        _sliderHealth = GetComponentInChildren<Slider>();
        _healthPreview = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        _health = GameManager.Instance.playerHealth;
        _sliderHealth.value = _health.Value;
        _health.OnValueChanged += UpdateHealthUI;
    }

    private void UpdateHealthUI(float newValue)
    {
        StopAllCoroutines();
        Debug.Log("TEST: " + newValue);
        StartCoroutine(AnimateHealth(newValue));
    }

    private IEnumerator AnimateHealth(float newValue)
    {
        SmoothValueAnimator healthAnimator = new SmoothValueAnimator(_sliderHealth.value, newValue, AnimationDuration);

        while (healthAnimator.IsRunning())
        {
            healthAnimator.Update();
            _sliderHealth.value = healthAnimator.CurrentValue;
            _healthPreview.text = healthAnimator.CurrentValue.ToString();
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        _sliderHealth.value = newValue;
        _healthPreview.text = newValue.ToString();
    }

    private void OnDestroy()
    {
        _health.OnValueChanged -= UpdateHealthUI;
    }
}