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
}