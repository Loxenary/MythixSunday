using UnityEngine;

public class MainKey : BaseKey, IKey
{
    public bool ReduceHealth(int amount)
    {
        return false;
    }

    public override bool IsDestructable => true;

    
}