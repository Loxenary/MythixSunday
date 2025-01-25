using UnityEngine;

public class F4Movement : MovementBase
{
    void Update()
    {
        // Check for key presses (not held down)
        if (!_isMoving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _moveX = 0f;
                _moveY = 1f; // Move up
                Move();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _moveX = 0f;
                _moveY = -1f; // Move down
                Move();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _moveX = -1f; // Move left
                _moveY = 0f;
                Move();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _moveX = 1f; // Move right
                _moveY = 0f;
                Move();
            }
        }

        // Move towards the target position
        if (_isMoving)
        {
            MoveToTarget();
            
        }
    }

    private void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.CompareTag("Enemy"))
        {
            // TODO : Attack Enemy based on F4 damage
            EnemyController enemyController = entity.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                Vector2 knockbackDirection = (entity.transform.position - transform.position).normalized;
                enemyController.TakeDamage(attackDamage, knockbackDirection, knockbackForce);
                Debug.Log("Kena");
            }
            else
            {
                Debug.LogWarning("EnemyController Not Found.");
            }
        }
        else if (entity.CompareTag("ALT"))
        {
            GameManager.Instance.ReduceLives(1);
        }
    }
}