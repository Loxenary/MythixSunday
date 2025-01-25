using UnityEngine;

public class AltMovement : MovementBase
{   
    public float invincibilityDuration = 2f;

    private bool isInvincible = false;
    private float invincibilityTimer = 0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                DisableInvincibility();
            }
            else
            {
                if (spriteRenderer != null)
                {
                    float flashSpeed = 10f;
                    float alpha = Mathf.PingPong(Time.time * flashSpeed, 1f);
                    spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.CompareTag("Enemy"))
        {
            if (!isInvincible)
            {
                EnemyController enemyController = entity.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    GameManager.Instance.ReduceHealth(enemyController.damage);

                    TriggerInvincibility();
                }
            }
        }
        else if (entity.CompareTag("F4"))
        {
            if (!isInvincible)
            {
                GameManager.Instance.ReduceLives(1);
                TriggerInvincibility();
            }
        }
        else if (entity.CompareTag("PowerUP"))
        {
            // TODO : Enable PowerUP
        }
    }

    public void TriggerInvincibility()
    {
        if (!isInvincible)
        {
            isInvincible = true;
            invincibilityTimer = invincibilityDuration;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    private void DisableInvincibility()
    {
        isInvincible = false;
        invincibilityTimer = 0f;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color (1f, 1f, 1f, 1f);
        }
    }
}