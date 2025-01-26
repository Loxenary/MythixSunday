using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance{
        get;
        private set;
    }


    [Header("Spawner Settings")]
    private List<SpawnPoint> spawnPoints; // List of spawn points
    
    public List<SpawnPoint> SpawnPoints{
        get{
            return spawnPoints;
        }
        set{
            spawnPoints = value;
        }
    }

    public int NumberOfSpawnPoints = 20;
    public GameObject SpawnPointPrefab;
    
    public float spawnInterval = 10f; // Time between spawns

    private List<Enemy> enemyDataList;

    private int amountOfSpawned = 5;
    private float timer;

    private void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
        spawnPoints = new();
    }

    private void Start()
    {
        // Load all EnemyData from the Resources folder
        LoadEnemyData();

        // Initialize the timer
        timer = spawnInterval;
        
    }

    private void Update()
    {
        if(spawnPoints == null){
            return;    
        }
        // Count down the timer
        timer -= Time.deltaTime;

        // Spawn an enemy when the timer reaches zero
        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval; // Reset the timer
        }
    }

    private void LoadEnemyData()
    {
        // Load all EnemyData assets from the Resources/EnemyData folder
        Enemy[] loadedData = Resources.LoadAll<Enemy>("Enemies");

        if (loadedData != null && loadedData.Length > 0)
        {
            enemyDataList = new List<Enemy>(loadedData);
            Debug.Log($"Loaded {enemyDataList.Count} enemy data entries.");
        }
        else
        {
            Debug.LogError("No EnemyData found in Resources/EnemyData folder!");
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        if (enemyDataList.Count == 0)
        {
            Debug.LogError("No enemy data loaded!");
            return;
        }

        DifficultyLevel difficulty = GameManager.Instance.Score.Difficulty();

        List<Enemy> unlockedEnemies = ScaledEnemyDifficulties().FindAll(e => e.unlockDifficulty <= difficulty);

        // Randomly select a spawn point
        foreach (SpawnPoint spawn in spawnPoints){
            Enemy enemyData = unlockedEnemies[Random.Range(0, unlockedEnemies.Count)];            
            StartCoroutine(StartSpawning(1, spawn,enemyData.enemyPrefab));
            Debug.Log($"Spawned {enemyData.enemyName} at {spawn.spawnPosition.position}");    
        }      
    }

    private IEnumerator StartSpawning(float interval,SpawnPoint spawnPoint, GameObject enemyPrefab){
        for(int i = 0 ; i < amountOfSpawned; i++){
            spawnPoint.SpawnEnemy(enemyPrefab);
            yield return new WaitForSeconds(interval);
        }
    }

    private Enemy ScaleEnemyStats(Enemy enemyData , DifficultyLevel difficulty){
        Enemy scaledEnemy = Instantiate(enemyData);

        // Scale stats based on difficulty
        scaledEnemy.health *= enemyData.healthMultiplier * (int)difficulty;
        scaledEnemy.damage *= enemyData.damageMultiplier * (int)difficulty;
        scaledEnemy.moveDelay *= enemyData.moveDelayMultiplier / (int)difficulty; // Faster enemies at higher difficulty
        scaledEnemy.maxCoinDrop = (int)(enemyData.maxCoinDrop * enemyData.coinDropMultiplier * (int)difficulty);
        scaledEnemy.gainScore *= enemyData.scoreGainMultiplier * (int)difficulty;

        return scaledEnemy;
    }

    private List<Enemy> ScaledEnemyDifficulties(){
        List<Enemy> enemies = new();
        foreach(Enemy enemy in enemyDataList){
            Enemy scaledEnemy = ScaleEnemyStats(enemy, GameManager.Instance.Score.Difficulty());
            enemies.Add(scaledEnemy);
        }
        return enemies;
    }
}