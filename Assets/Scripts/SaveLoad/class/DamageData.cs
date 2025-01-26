using UnityEngine;

public class DamageData : ISaveData
{
    public string FileName => "DamageData";

    public ResourcesSaveData resourcesSaveData;
    public float DamageDealt;
    
}