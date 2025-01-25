using Unity.VisualScripting;
using UnityEngine;

public class F4Movement : MovementBase
{
    public float attackDamage = 10f;
    public float knockbackForce = 5f;
    void Update(){
        if(!_isMoving){
            _moveX = Input.GetAxisRaw("HorizontalArrow");
            _moveY = Input.GetAxisRaw("VerticalArrow");

            Move();
        }
        if(_isMoving){
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