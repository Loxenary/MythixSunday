using UnityEngine;

public class MainKey : BaseKey, IKey
{
    public MainKey(Character character) : base(character)
    {
    }

    public override bool IsDestructable => true;
}