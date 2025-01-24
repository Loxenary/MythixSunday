using UnityEngine;
[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{
    public KeyCode KeyCode;

    public Sprite LeftDirectionIcon;
    public Sprite RightDirectionIcon;

    public IntHealth Health;
    private float _moveSpeed = 1; 
    private float _defaultMoveSpeed =  1;

    public float MovementSpeed{
        get{return _moveSpeed;}
        set{
            _moveSpeed = value;
        }
    }

    public void ResetSpeed(){
        _moveSpeed = _defaultMoveSpeed;
    }
}