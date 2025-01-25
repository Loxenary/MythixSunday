using UnityEngine;
using System;

public class SmoothValueAnimator<T> where T : struct, IComparable, IConvertible
{
    private T _startValue; // Starting value of the animation
    private T _targetValue; // Target value of the animation
    private T _currentValue; // Current value during the animation
    private float _duration; // Duration of the animation
    private float _elapsedTime; // Time elapsed since the animation started

    private EaseType _easeType; // Type of easing function to use

    // Events
    public event Action<T> OnUpdate; // Called every frame during the animation
    public event Action OnComplete; // Called when the animation completes

    // Easing types
    public enum EaseType
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo
    }

    // Constructor
    public SmoothValueAnimator(T startValue, T targetValue, float duration = 1, EaseType easeType = EaseType.Linear)
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
        {
            // Ensure the final value is set
            _currentValue = _targetValue;
            OnUpdate?.Invoke(_currentValue);
            OnComplete?.Invoke();
            return; // Animation is complete
        }

        _elapsedTime += Time.deltaTime;

        // Calculate the interpolation factor (0 to 1)
        float t = Mathf.Clamp01(_elapsedTime / _duration);

        // Apply easing function
        t = ApplyEasing(t, _easeType);

        // Interpolate between start and target values
        _currentValue = Lerp(_startValue, _targetValue, t);

        // Invoke the update event
        OnUpdate?.Invoke(_currentValue);
    }

    // Check if the animation is running
    public bool IsRunning()
    {
        return _elapsedTime < _duration;
    }

    // Get the current value
    public T CurrentValue => _currentValue;

    // Apply easing function to the interpolation factor
    private float ApplyEasing(float t, EaseType easeType)
    {
        switch (easeType)
        {
            case EaseType.Linear:
                return t; // No easing
            case EaseType.EaseIn:
                return t * t; // Quadratic ease-in
            case EaseType.EaseOut:
                return 1 - (1 - t) * (1 - t); // Quadratic ease-out
            case EaseType.EaseInOut:
                return t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2; // Quadratic ease-in-out
            case EaseType.EaseInCubic:
                return t * t * t; // Cubic ease-in
            case EaseType.EaseOutCubic:
                return 1 - Mathf.Pow(1 - t, 3); // Cubic ease-out
            case EaseType.EaseInOutCubic:
                return t < 0.5f ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2; // Cubic ease-in-out
            case EaseType.EaseInExpo:
                return t == 0 ? 0 : Mathf.Pow(2, 10 * t - 10); // Exponential ease-in
            case EaseType.EaseOutExpo:
                return t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t); // Exponential ease-out
            case EaseType.EaseInOutExpo:
                return t == 0 ? 0 : t == 1 ? 1 : t < 0.5f ? Mathf.Pow(2, 20 * t - 10) / 2 : (2 - Mathf.Pow(2, -20 * t + 10)) / 2; // Exponential ease-in-out
            default:
                return t; // Default to linear
        }
    }

    // Generic Lerp function for different numeric types
    private T Lerp(T a, T b, float t)
    {
        if (typeof(T) == typeof(float))
        {
            return (T)(object)Mathf.Lerp((float)(object)a, (float)(object)b, t);
        }
        else if (typeof(T) == typeof(int))
        {
            return (T)(object)(int)Mathf.Lerp((int)(object)a, (int)(object)b, t);
        }
        else if (typeof(T) == typeof(long))
        {
            return (T)(object)(long)Mathf.Lerp((long)(object)a, (long)(object)b, t);
        }
        else
        {
            throw new NotSupportedException($"Type {typeof(T)} is not supported for Lerp.");
        }
    }
}