using Unity.VisualScripting;
using UnityEngine;

public class MainKey : BaseKey, IKey
{
    public MainKey(Character character) : base(character)
    {
    }

    private Troops _troops;

    private void OnDestroy()
    {
        _troops.OnSpeedDecreaseEvent -= MultiplySpeed;
        _troops.OnSpeedDecreaseEvent -= DeMultiplySpeed;
    }

    private void MultiplySpeed(float speedMultiplyer){
        float speed = Mathf.Max(Character.MovementSpeed, 1);
        Character.MovementSpeed = speed * speedMultiplyer;
    }

    private void DeMultiplySpeed(float speedDemultiplyer){
        float speed = Mathf.Max(Character.MovementSpeed, 1) / speedDemultiplyer;
        Character.MovementSpeed = speed < 1 ? Character.DefaultMovementSpeed : speed;
    }


    public void SetTroops(Troops troops){
        _troops = troops;
        _troops.OnSpeedIncreaseEvent += MultiplySpeed;
        _troops.OnSpeedDecreaseEvent += DeMultiplySpeed;  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("MainKey")){
            //TODO: Implement game Over
        }
        else if(collision.gameObject.CompareTag("NormalKey")){
            NormalKey normalKey = collision.gameObject.GetComponent<NormalKey>();
            if(!normalKey){
                Debug.LogError("Collided object with tag 'NormalKey' does not have a NormalKey component.");
                return;
            }
            if(_troops == null){
                Debug.LogError("MainKey is not part of a Troop instanced");
                return;
            }
            _troops.AttachNormalKey(normalKey);
        }
    }

    private void Update(){
        if(_troops == null){
            return;
        }
        _troops.UpdateAttachedKeys(transform.position);
    }

    public override bool IsDestructable => true;
}