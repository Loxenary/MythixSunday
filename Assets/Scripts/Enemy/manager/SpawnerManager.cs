using UnityEngine;
using System.Collections.Generic;

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
    
    public float spawnInterval = 5f; // Time between spawns

    private List<Enemy> enemyDataList;
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

    public void LoadSpawnPoints(){

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

        // Randomly select a spawn point
        foreach (SpawnPoint spawn in spawnPoints){
            Enemy enemyData = enemyDataList[Random.Range(0, enemyDataList.Count)];
            spawn.SpawnEnemy(enemyData.enemyPrefab);
            Debug.Log($"Spawned {enemyData.enemyName} at {spawn.spawnPosition.position}");    
        }      
    }
}