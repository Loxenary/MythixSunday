using System;
using Unity.VisualScripting;

public abstract class Resource<TValue>  {
    public TValue Value { get; private set; }

    public event Action<TValue> OnValueChanged; // Event triggered when the value changes

    protected Resource(TValue initialValue)
    {
        Value = initialValue;
    }

    // Increase the value of the resource
    public void Add(TValue amount)
    {
        dynamic currentValue = Value;
        dynamic delta = amount;
        Value = currentValue + delta;

        OnValueChanged?.Invoke(Value); // Trigger the event
    }

    // Decrease the value of the resource
    public void Reduce(TValue amount)
    {
        dynamic currentValue = Value;
        dynamic delta = amount;
        Value = currentValue - delta;

        OnValueChanged?.Invoke(Value); // Trigger the event
    }

    // Set the value of the resource directly
    public void Set(TValue value)
    {
        Value = value;
        OnValueChanged?.Invoke(Value); // Trigger the event
    }

    public virtual void OnModify(TValue value){

    }
}