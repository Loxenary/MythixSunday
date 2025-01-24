using UnityEngine;

public class MainKey : NormalKey, IKey
{
    public override void Delete()
    {
        //Do Nothing
    }

    public override bool IsDestructable => true;
}