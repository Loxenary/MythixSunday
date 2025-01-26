using UnityEngine;

public class PricesData : ISaveData
{
    public float DamagePrice;
    public float LivesPrice;
    public float HealthPrice;
    public string FileName => "PricesData";
}