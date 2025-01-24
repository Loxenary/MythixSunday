using System;

[Serializable]
public class IntHealth : Resource<int>
{
    public IntHealth(int initialValue) : base(initialValue)
    {
    }
}


[Serializable]
public class FloatHealth : Resource<float>
{
    public FloatHealth(float initialValue) : base(initialValue)
    {
    }
}