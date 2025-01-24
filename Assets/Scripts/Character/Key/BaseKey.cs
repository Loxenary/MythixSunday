using UnityEngine;

public  class BaseKey : MonoBehaviour, IKey
{    
    [SerializeField] private Character character;

    public Character Character{
        get{ return character; }
        set{ character = value; }
    }

    public void IncreaseSpeed(float speed){
        Character.MovementSpeed = speed;
    }

    public virtual bool IsDestructable => false;
    public KeyCode Key => Character.KeyCode;

    protected void ReducingHealth(int amount){
        character.Health.Reduce(amount);   
    }

    public virtual bool ReduceHealth(int amount){
        return false;
    }

    public BaseKey(Character character){
        this.character = character;
    }
}