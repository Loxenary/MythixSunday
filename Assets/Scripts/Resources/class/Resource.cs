using System;

[Serializable]
public abstract class Resource<TValue> where TValue : IComparable<TValue> {
    public TValue Value { get; private set; }

    public event Action<TValue> OnValueChanged; // Event triggered when the value changes

    protected Resource(TValue initialValue)
    {
        Value = initialValue;
    }

    // Increase the value of the resource
    public void Add(TValue amount)
    {
        
        Value = NumberHelper.Add(Value, amount);
        OnValueChanged?.Invoke(Value); // Trigger the event
    }

    // Decrease the value of the resource
    public void Reduce(TValue amount)
    {
        Value = NumberHelper.Subtract(Value, amount);
        OnValueChanged?.Invoke(Value); // Trigger the event
    }

    // Set the value of the resource directly
    public void Set(TValue value)
    {
        Value = value;
        OnValueChanged?.Invoke(Value); // Trigger the event
    }
}