using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string Name;
    public float MoveDelay;
    public float Damage;
    public float Health;
    public int MaxDropCoin;
}