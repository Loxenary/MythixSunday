using UnityEngine;

public abstract class SpesialKey : NormalKey, IKey
{
    protected SpesialKey(Character character) : base(character)
    {
    }

    public abstract void ApplySpecialEffect();
}