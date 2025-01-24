using UnityEngine;

public class NormalKey : MonoBehaviour, IKey
{
    public Character Character{
        get; 
        private set;
    }

    public void IncreaseSpeed(float speed){
        Character.MovementSpeed = speed;
    }

    public virtual bool IsDestructable => false;
    public KeyCode Key => Character.KeyCode;

    public virtual void Delete(){
        Destroy(gameObject);
    }
}