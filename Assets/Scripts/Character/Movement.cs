using System.Runtime.Serialization.Json;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private bool isMovingRight = true;

    [SerializeField] private float maxSpeed = 10f;

    public bool GetMovingDirection() => isMovingRight;

    private float _xMovement;

    private Rigidbody2D _rb;

    private void Start(){
        _rb = GetComponent<Rigidbody2D>();

    }

    private void Update(){
        if(Input.GetKey(KeyCode.Dollar)){
            Debug.Log("TESTINGOSIEJ");
        }
        if(!CanMove()){
            return;
        }
        _xMovement = 2;
        HandleMove();
    }

    private bool CanMove(){
        if(transform.childCount > 1){
            return true;
        }
        return false;
    }

    private void HandleMove()
    {
        float targetVelocityX = _xMovement * maxSpeed;
        float velocityChangeX = targetVelocityX - _rb.linearVelocityX;

        Vector3 moveVector = isMovingRight ? Vector3.right : Vector3.left; 
        _rb.AddForce(velocityChangeX * moveVector, ForceMode2D.Force);

        float clampedVelocityX = Mathf.Clamp(_rb.linearVelocityX, -maxSpeed, maxSpeed);
        _rb.linearVelocity = new Vector2(clampedVelocityX, 0);
    }

}
