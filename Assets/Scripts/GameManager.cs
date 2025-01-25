using UnityEngine;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    public FloatHealth playerHealth;
    public IntHealth playerLives;
    public Coins coins;

    public Score Score;

    public Action OnGameOver;

    private void OnEnable()
    {
        playerLives.OnValueChanged += HandleLivesChanged;
        playerHealth.OnValueChanged += HandleHealthChanged;
        coins.OnValueChanged += HandleCoinsChanged;
    }

    private void OnDisable()
    {
        playerLives.OnValueChanged -= HandleLivesChanged;
        playerHealth.OnValueChanged -= HandleHealthChanged;
        coins.OnValueChanged -= HandleCoinsChanged;
    }

    private void Awake()
    {   
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // TODO : Initialize playerHealth and playerLives and coins based on saved data
    }

    private void Start()
    {
        RestartGame();
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

    public void RestartGame(){
        playerLives.Set(3);
        playerHealth.Set(100f);
        Score.Set(0);
    }

    private void HandleLivesChanged(int newLives)
    {
        Debug.Log($"nyawa barumu bung: {newLives}");
        if (newLives <= 0)
        {
            Debug.Log($"Kamu mati bung, sayang sekali");
            OnGameOver?.Invoke();
        }
    }

    private void HandleHealthChanged(float newHealth)
    {
        Debug.Log($"Health baru: {newHealth}");
    }

    private void HandleCoinsChanged(int newCoins)
    {
        Debug.Log($"Uang Anda saat ini: {newCoins}T");
    }
}