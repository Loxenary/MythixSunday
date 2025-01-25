using UnityEngine;

public class AltMovement : MovementBase
{   

    void Update()
    {
        // Check for key presses (not held down)
        if (!_isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _moveX = 0f;
                _moveY = 1f; // Move up
                Move();
            }
            else if (Input.GetKeyDown(KeyCode.S) )
            {
                _moveX = 0f;
                _moveY = -1f; // Move down
                Move();
            }
            else if (Input.GetKeyDown(KeyCode.A) )
            {
                _moveX = -1f; // Move left
                _moveY = 0f;
                Move();
            }
            else if (Input.GetKeyDown(KeyCode.D) )
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
            // TODO : Reduce health based on enemy attack value
            EnemyController enemyController = entity.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                GameManager.Instance.ReduceHealth(enemyController.damage);
            }
        }
        else if (entity.CompareTag("PowerUP"))
        {
            // TODO : Enable PowerUP
        }
    }
}