using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public FloatHealth health = new(100);
    private Slider _sliderHealth;

    [SerializeField] private float AnimationDuration;
 
    private void Awake()
    {
        _sliderHealth = GetComponent<Slider>();
    }

    private void Start()
    {
        _sliderHealth.value = health.Value;
        health.OnValueChanged += UpdateHealthUI;
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
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        _sliderHealth.value = newValue;
    }

    private void OnDestroy()
    {
        health.OnValueChanged -= UpdateHealthUI;
    }
}