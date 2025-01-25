using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    protected Animator _animator;

    private void Start(){
        _animator = GetComponent<Animator>();
    }   
}