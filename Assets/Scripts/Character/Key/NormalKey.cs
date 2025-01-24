public class NormalKey : BaseKey, IKey
{

    // public bool IsAttached = false;

    public override bool ReduceHealth(int amount)
    {   
        ReducingHealth(amount);
        if(Character.Health.Value == 0){
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    public NormalKey(Character character) : base(character){

    }
    
}