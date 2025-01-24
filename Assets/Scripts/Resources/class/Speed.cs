using UnityEngine;

public class Speed : Resource<int>
{
    private int _defaultSpeed;
    public Speed(int initialValue) : base(initialValue)
    {
        _defaultSpeed = initialValue;
    }

    public void ResetSpeed(){
        Set(_defaultSpeed);
    }
}