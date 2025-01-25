using UnityEngine;

public class ResourcesSaveData : ISaveData
{
    public string FileName => "ResourcesData.json";

    public int Coins;
    public int Lives;
    public float Health;

    public void SetData(int coins, int lives, float health){
        Coins = coins;
        Lives = lives;
        Health = health;
    }
}