using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemies/Enemy Data")]
public class Enemy : ScriptableObject
{
    public string enemyName;
    public float moveDelay;
    public float damage;
    public float health;
    public int maxCoinDrop;
    
}