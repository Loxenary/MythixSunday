using Unity.VisualScripting;
using UnityEngine;

public class AltMovement : MovementBase
{
    void Update(){
        if(!_isMoving){
            _moveX = Input.GetAxisRaw("Horizontal");
            _moveY= Input.GetAxisRaw("Vertical");
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