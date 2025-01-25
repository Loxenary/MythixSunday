using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyMovement _enemyMovement;
    private float currentHealth;
    public float damage { get; private set;}
    private Rigidbody2D rb;

    private void Start()
    {
        _enemyMovement = this.GetComponent<EnemyMovement>();
        currentHealth = _enemyMovement.enemyData.health;
        damage = _enemyMovement.enemyData.damage;
        rb = GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
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
        float gridSize = GridManager.Instance.GridSize;

        Vector2 knockbackDirection = direction.normalized;
        Vector2 newPosition = (Vector2)transform.position + knockbackDirection * gridSize;
        newPosition = SnapToGrid(newPosition);
        transform.position = newPosition;
        Debug.Log("Mundur");
    }

    protected Vector2 SnapToGrid(Vector2 position){
        Grid grid = GridManager.Instance.GetComponent<Grid>();
        Vector3Int cellPosition = grid.WorldToCell(position);
        Vector3 snappedPosition = grid.CellToWorld(cellPosition);
        return new Vector2(snappedPosition.x, snappedPosition.y);
    }

    private void Die()
    {
        Debug.Log("Musuh Mati");
        Destroy(gameObject);
    }
}