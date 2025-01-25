using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    private FloatHealth _health;
    private Slider _sliderHealth;

    [SerializeField] private float AnimationDuration;
 
    private void Awake()
    {
        _sliderHealth = GetComponent<Slider>();
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
        StartCoroutine(AnimateHealth(newValue));
    }

    private IEnumerator AnimateHealth(float newValue)
    {
        SmoothValueAnimator<float> healthAnimator = new (_sliderHealth.value, newValue, AnimationDuration);

        while (healthAnimator.IsRunning())
        {
            healthAnimator.Update();
            _sliderHealth.value = healthAnimator.CurrentValue;
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        _sliderHealth.value = newValue;
    }

    private void OnDestroy()
    {
        _health.OnValueChanged -= UpdateHealthUI;
    }
}