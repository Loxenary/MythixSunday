using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy enemyData;
    private float currentHealth;
    public float damage { get; private set;}
    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = enemyData.health;
        damage = enemyData.damage;
        rb = GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            // rb.isKinematic = false;
        }
    }

    public void TakeDamage(float damageAmount, Vector2 knockbackDirection, float knockbackForce)
    {
        currentHealth -= damageAmount;
        Debug.Log($"took : {damageAmount}, remaining: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            ApplyKnockback(knockbackDirection, knockbackForce);
        }
    }

    private void ApplyKnockback(Vector2 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        Debug.Log("Mundur");
    }

    private void Die()
    {
        Debug.Log("Musuh Mati");
        Destroy(gameObject);
    }
}