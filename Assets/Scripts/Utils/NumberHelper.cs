using System;
using Unity.VisualScripting;

public static class NumberHelper
{
    // Add two values
    public static T Add<T>(T a, T b) where T : IComparable<T>
    {
        return PerformOperation(a, b, (x, y) => x + y, (x, y) => x + y, (x, y) => x + y);
    }

    public static T Multiply<T>(T a, T b) where T: IComparable<T>{
        return PerformOperation(a,b, (x,y) => x * y, (x, y) => x * y, (x,y) => x* y);
    }

    // Subtract two values
    public static T Subtract<T>(T a, T b) where T : IComparable<T>
    {
        return PerformOperation(a, b, (x, y) => x - y, (x, y) => x - y, (x, y) => x - y);
    }

     private static T PerformOperation<T>(
        T a, T b,
        Func<int, int, int> intOperation,
        Func<float, float, float> floatOperation,
        Func<double, double, double> doubleOperation)
        where T : IComparable<T>
    {
        if (typeof(T) == typeof(int))
        {
            return (T)(object)intOperation((int)(object)a, (int)(object)b);
        }
        else if (typeof(T) == typeof(float))
        {
            return (T)(object)floatOperation((float)(object)a, (float)(object)b);
        }
        else if (typeof(T) == typeof(double))
        {
            return (T)(object)doubleOperation((double)(object)a, (double)(object)b);
        }
        else
        {
            throw new NotSupportedException($"Type {typeof(T).Name} is not supported.");
        }
    }
}