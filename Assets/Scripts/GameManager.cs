using UnityEngine;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    public FloatHealth playerHealth;
    public IntHealth playerLives;
    public Coins coins;

    private void Awake()
    {   
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // TODO : Initialize playerHealth and playerLives and coins
    }

    public void AddCoins(int amount)
    {
        coins.Add(amount);
    }

    public void SpendCoin(int amount)
    {
        if (coins.Value >= amount)
        {
            coins.Reduce(amount);
        }
    }

    public void ReduceHealth(float amount)
    {
        playerHealth.Reduce(amount);
    }

    public void ReduceLives(int amount)
    {
        playerLives.Reduce(amount);
    }
}