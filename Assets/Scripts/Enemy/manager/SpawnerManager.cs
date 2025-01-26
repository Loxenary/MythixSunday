using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }

    [Header("Spawner Settings")]
    [Tooltip("List of spawn points.")]
    private List<SpawnPoint> spawnPoints = new List<SpawnPoint>(); // Initialize to prevent null references

    public List<SpawnPoint> SpawnPoints
    {
        get { return spawnPoints; }
        set { spawnPoints = value; }
    }

    [Tooltip("Number of spawn points to instantiate.")]
    public int NumberOfSpawnPoints = 20;

    [Tooltip("Prefab for spawn points.")]
    public GameObject SpawnPointPrefab;

    [Tooltip("Time interval between spawns.")]
    public float spawnInterval = 10f; // Time between spawns

    [Tooltip("List of enemy data.")]
    public List<Enemy> enemyDataList; // Assign via Inspector

    [Tooltip("Number of enemies to spawn per spawn cycle.")]
    public int amountOfSpawned = 5;

    private float timer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeSpawnPoints();
    }

    private void Start()
    {
        // Optionally, start a spawning loop
        StartCoroutine(SpawnEnemiesLoop());
    }

    /// <summary>
    /// Initializes spawn points either by finding existing ones or instantiating new ones.
    /// </summary>
    private void InitializeSpawnPoints()
    {
        if (SpawnPointPrefab == null)
        {
            Debug.LogError("SpawnPointPrefab is not assigned in SpawnerManager.");
            return;
        }

        for (int i = 0; i < NumberOfSpawnPoints; i++)
        {
            Vector3 position = GetRandomSpawnPosition();
            GameObject spawnPointObj = Instantiate(SpawnPointPrefab, position, Quaternion.identity);
            SpawnPoint spawnPoint = spawnPointObj.GetComponent<SpawnPoint>();

            if (spawnPoint != null)
            {
                spawnPoints.Add(spawnPoint);
                Debug.Log($"Initialized SpawnPoint at {position}");
            }
            else
            {
                Debug.LogWarning("SpawnPointPrefab does not have a SpawnPoint component.");
            }
        }
    }

    /// <summary>
    /// Generates a random position for spawn points.
    /// </summary>
    /// <returns>Random Vector3 position.</returns>
    private Vector3 GetRandomSpawnPosition()
    {
        // Define your spawn area boundaries here
        float x = Random.Range(-10f, 10f);
        float y = Random.Range(-10f, 10f);
        return new Vector3(x, y, 0f);
    }

    /// <summary>
    /// Coroutine that handles the spawning loop.
    /// </summary>
    /// <returns>IEnumerator for coroutine.</returns>
    private IEnumerator SpawnEnemiesLoop()
    {
        while (true)
        {
            SpawnEnemies();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    /// <summary>
    /// Spawns enemies at each spawn point.
    /// </summary>
    private void SpawnEnemies()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points available to spawn enemies.");
            return;
        }

        if (enemyDataList == null || enemyDataList.Count == 0)
        {
            Debug.LogError("Enemy data list is empty or not assigned.");
            return;
        }

        DifficultyLevel currentDifficulty = GameManager.Instance.Score.Difficulty();

        foreach (SpawnPoint spawn in spawnPoints)
        {
            Enemy baseEnemy = enemyDataList[Random.Range(0, enemyDataList.Count)];
            Enemy scaledEnemy = ScaleEnemyStats(baseEnemy, currentDifficulty);

            if (scaledEnemy.enemyPrefab != null)
            {
                
                spawn.SpawnEnemy(scaledEnemy.enemyPrefab);

                Debug.Log($"Spawned {scaledEnemy.enemyName} at {spawn.spawnPosition.position} WITH HEALTH {scaledEnemy.health}");
            }
            else
            {
                Debug.LogWarning($"Enemy prefab for {scaledEnemy.enemyName} is not assigned.");
            }
        }
    }

    /// <summary>
    /// Coroutine to spawn a specified number of enemies with a delay.
    /// </summary>
    /// <param name="interval">Delay between each spawn.</param>
    /// <param name="spawnPoint">Spawn point reference.</param>
    /// <param name="enemyPrefab">Enemy prefab to spawn.</param>
    /// <returns>IEnumerator for coroutine.</returns>

    /// <summary>
    /// Scales enemy stats based on the current difficulty.
    /// </summary>
    /// <param name="enemyData">Base enemy data.</param>
    /// <param name="difficulty">Current game difficulty.</param>
    /// <returns>Scaled Enemy instance.</returns>
    private Enemy ScaleEnemyStats(Enemy enemyData, DifficultyLevel difficulty)
    {
        if (enemyData == null)
        {
            Debug.LogError("Enemy data is null. Cannot scale stats.");
            return null;
        }

        Enemy scaledEnemy = Instantiate(enemyData);

        // Ensure base stats are used for scaling
        scaledEnemy.health = enemyData.health  * (int)difficulty + 1;
        scaledEnemy.damage = enemyData.damage * (int)difficulty + 1;
        scaledEnemy.moveDelay = enemyData.moveDelay; // Prevent division by zero
        scaledEnemy.maxCoinDrop = (int)(enemyData.maxCoinDrop *( (int)difficulty) + 1);
        scaledEnemy.gainScore *= (int)difficulty + 1;

        Debug.Log($"Scaled Enemy '{scaledEnemy.enemyName}': Health={scaledEnemy.health}, Damage={scaledEnemy.damage}, MoveDelay={scaledEnemy.moveDelay}, MaxCoinDrop={scaledEnemy.maxCoinDrop}, GainScore={scaledEnemy.gainScore}");

        return scaledEnemy;
    }
}