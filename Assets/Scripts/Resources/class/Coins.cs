using System;

[Serializable]
public class Coins : Resource<int>
{
    public Coins(int initialValue) : base( initialValue)
    {

    }
}