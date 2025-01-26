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
        ResourcesSaveData userData = SaveLoadManager.Load<ResourcesSaveData>();
        playerLives = new IntHealth(0);
        playerHealth = new FloatHealth(0);
        if(userData == null || userData == default || userData.Lives == 0 || userData.Health == 0 ){
            playerLives.Set(1);
            playerHealth.Set(100);
        }else{
            Debug.Log(userData.Lives);

            playerLives.Set(userData.Lives);
            playerHealth.Set(userData.Health);
        }

        coins = new Coins(0);
        Score = new Score(0);
    }

    private void HandleLivesChanged(int newLives)
    {
        Debug.Log($"nyawa barumu bung: {newLives}");
        if (newLives <= 0)
        {
            Debug.Log($"Kamu mati bung, sayang sekali");
            OnGameOver?.Invoke();
            AudioManager.Instance.PlayMusic("GameOverBGM",true,1,true);
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