using UnityEngine;

public  class BaseKey : MonoBehaviour, IKey
{    
    [SerializeField] private Character character;
    // =============================================
    private bool isAttached = false;
    // =============================================

    public Character Character{
        get{ return character; }
        set{ character = value; }
    }

    // =============================================
    public bool IsAttached
    {
        get { return isAttached; }
        set { isAttached = value; }
    }
    // =============================================

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