using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Transform spawnPosition;
    public float spawnRange = 10f; // Range within which the player must be to spawn enemies

    private GameObject player; // Reference to the player

    private void Start()
    {
        // Find the player object (assuming it has the tag "Player")
        player = GameObject.FindWithTag("ALT");

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the tag 'Player'.");
        }
    }

    public void SpawnEnemy(GameObject enemyPrefab)
    {
        
        if (spawnPosition != null && enemyPrefab != null)
        {
            // Check if the player is within the spawn range
            if (IsPlayerInRange())
            {
                Instantiate(enemyPrefab, spawnPosition.position, Quaternion.identity);
                Debug.Log($"Spawned enemy at {spawnPosition.position}");
            }
            else
            {
                Debug.Log("Player is not in range. Enemy not spawned.");
            }
        }
        else
        {
            Debug.LogError("Spawn position or enemy prefab is not set!");
        }
    }

    private bool IsPlayerInRange()
    {
        if (player == null)
        {
            return false;
        }

        // Calculate the distance between the spawn point and the player
        float distance = Vector2.Distance(spawnPosition.position, player.transform.position);

        // Return true if the player is within the spawn range
        return distance <= spawnRange;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the spawn range in the editor for debugging
        if (spawnPosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(spawnPosition.position, spawnRange);
        }
    }
}