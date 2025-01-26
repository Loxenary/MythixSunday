using UnityEngine;

public class ExplosionPowerUp : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadius = 2f;
    public float explosionDamage = 100f;
    public LayerMask enemyLayer;
    public string explosionPoolTag = "Explosion"; // Ensure this tag matches the ObjectPool setup

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("ALT"))
        {
            // Trigger the explosion at the player's position
            TriggerExplosion(other.transform.position);

            // Return the power-up to the pool or destroy it
            // If pooling power-ups, return to pool instead of destroying
            ObjectPool.Instance.ReturnToPool("ExplosionPowerUp", gameObject);
            // If not pooling, use: Destroy(gameObject);
        }
    }

    /// <summary>
    /// Triggers an explosion effect using the Object Pool.
    /// </summary>
    /// <param name="position">Position where the explosion occurs.</param>
    private void TriggerExplosion(Vector2 position)
    {
        GameObject explosion = ObjectPool.Instance.GetFromPool(explosionPoolTag, position, Quaternion.identity);
        if(explosion != null)
        {
            // Initialize explosion if necessary
            // For example, reset animations or play sound effects
            // The Explosion script attached to the explosion prefab will handle the effects
        }
        else
        {
            Debug.LogWarning($"Failed to retrieve '{explosionPoolTag}' from the pool.");
        }
    }
}