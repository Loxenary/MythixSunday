using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class NormalKey : BaseKey, IKey
{

    public bool IsAttached = false;

    public bool ReduceHealth(int amount)
    {   
        ReducingHealth(amount);
        if(Character.Health.Value == 0){
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}