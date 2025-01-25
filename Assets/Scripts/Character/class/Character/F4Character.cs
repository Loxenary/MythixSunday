using UnityEngine;

public class F4Character : BaseCharacter
{    
    private F4Movement _movement;

    private bool _isIdle = true;

    [SerializeField] private float idleTimerDefault = 0.2f;
    private float _idleTimer;

    private void Start(){
        _animator = GetComponent<Animator>();
        _movement = GetComponent<F4Movement>();
    }

    private void Update(){
        ApplyAnimation();
    }

    private void ApplyAnimation(){
        Vector2 movementInput  = _movement.GetMovementDirection();
        
        if(movementInput != Vector2.zero){
            Debug.Log("Direction : " + movementInput);
            _animator.SetFloat("Horizontal", movementInput.x);
            _animator.SetFloat("Vertical", movementInput.y);
            _movement.ResetMovement();
            _isIdle = false;
        }else{
            _isIdle = true;
            
        }
        ApplyIdleAnimation();   


    }

    private void ApplyIdleAnimation(){
        if(_isIdle){
            _idleTimer -= Time.deltaTime;
            if(_idleTimer <= 0f){
                _animator.SetBool("IsIdle",  true);     
            }
        }else{
            _animator.SetBool("IsIdle", false);
            _idleTimer = idleTimerDefault;
        }
    }

    
}