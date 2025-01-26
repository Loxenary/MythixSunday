using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float radius = 2f;
    public float damage = 100f;
    public LayerMask enemyLayer;
    public float effectDuration = 0.5f; // Time before the explosion object is returned to the pool

    private void OnEnable()
    {
        // Trigger the explosion effect immediately upon activation
        //Explode();
    }

    /// <summary>
    /// Handles the explosion logic: detecting and damaging nearby enemies.
    /// </summary>
    private void Explode()
    {
        // Play explosion animation or particles here if any

        // Detect all enemies within the radius
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if(enemyController != null)
            {
                // Apply damage to the enemy
                enemyController.TakeDamage(damage, Vector2.zero, 0f);
            }
        }

        // Optionally, play explosion sound here

        // Schedule the explosion object to be returned to the pool after the effect duration
        Invoke(nameof(ReturnToPool), effectDuration);
    }

    /// <summary>
    /// Returns the explosion GameObject to the pool.
    /// </summary>
    private void ReturnToPool()
    {
        ObjectPool.Instance.ReturnToPool("Explosion", gameObject);
    }

    // Optional: Visual representation in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}