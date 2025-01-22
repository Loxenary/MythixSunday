using UnityEngine;

public class SmoothValueAnimator
{
    private float _startValue; // Starting value of the animation
    private float _targetValue; // Target value of the animation
    private float _currentValue; // Current value during the animation
    private float _duration; // Duration of the animation
    private float _elapsedTime; // Time elapsed since the animation started

    private EaseType _easeType; // Type of easing function to use

    // Easing types
    public enum EaseType
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut
    }

    // Constructor
    public SmoothValueAnimator(float startValue, float targetValue, float duration = 1, EaseType easeType = EaseType.Linear)
    {
        _startValue = startValue;
        _targetValue = targetValue;
        _currentValue = startValue;
        _duration = duration;
        _elapsedTime = 0f;
        _easeType = easeType;
    }

    // Update the animation
    public void Update()
    {
        if (_elapsedTime >= _duration)
            return; // Animation is complete

        _elapsedTime += Time.deltaTime;

        // Calculate the interpolation factor (0 to 1)
        float t = Mathf.Clamp01(_elapsedTime / _duration);

        // Apply easing function
        t = ApplyEasing(t, _easeType);

        // Interpolate between start and target values
        _currentValue = Mathf.Lerp(_startValue, _targetValue, t);
    }

    // Check if the animation is running
    public bool IsRunning()
    {
        return _elapsedTime < _duration;
    }

    // Get the current value
    public float CurrentValue => _currentValue;

    // Apply easing function to the interpolation factor
    private float ApplyEasing(float t, EaseType easeType)
    {
        switch (easeType)
        {
            case EaseType.Linear:
                return t; // No easing
            case EaseType.EaseIn:
                return t * t; // Ease-in (quadratic)
            case EaseType.EaseOut:
                return 1 - (1 - t) * (1 - t); // Ease-out (quadratic)
            case EaseType.EaseInOut:
                return t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2; // Ease-in-out (quadratic)
            default:
                return t; // Default to linear
        }
    }
}