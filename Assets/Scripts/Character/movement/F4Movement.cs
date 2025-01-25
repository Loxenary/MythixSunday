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
}