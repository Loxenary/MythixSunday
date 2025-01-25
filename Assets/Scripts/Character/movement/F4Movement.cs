using Unity.VisualScripting;
using UnityEngine;

public class F4Movement : MovementBase
{
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
        }
        else if (entity.CompareTag("ALT"))
        {
            // GameManager.Instance.ReduceLives(1);
        }
    }
}