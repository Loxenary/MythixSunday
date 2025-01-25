using Unity.VisualScripting;
using UnityEngine;

public class AltMovement : MovementBase
{
    void Update(){
        if(!_isMoving){
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            Move();
        }
        if(_isMoving){
            MoveToTarget();
        }
    }
}