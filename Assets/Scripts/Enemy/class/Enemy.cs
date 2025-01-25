using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemies/Enemy Data")]
public class Enemy : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName;
    public float moveDelay;
    public float damage;
    public float health;
    public int maxCoinDrop;

    public GameObject enemyPrefab;

    [Header("Grid-Based Mechanics")]
    public int movementRange; // Number of tiles the enemy can move per turn
    public AttackType attackType;
    public int attackRange; // Range in tiles for attacks
    public bool canTeleport;
    public int teleportRange; // Number of tiles for teleportation

    [Header("Special Abilities")]
    public SpecialAbilityType specialAbility;
    public int abilityRange; // Range in tiles for special abilities

    [Header("Indicator Settings")]
    public GameObject movementIndicatorPrefab;
    public Color movementIndicatorColor = Color.green;
    public float movementIndicatorDuration = 1f;

    // public GameObject attackIndicatorPrefab;
    // public Color attackIndicatorColor = Color.red;
    // public float attackIndicatorDuration = 1f;
}

public enum AttackType
{
    None,
    ProjectileStraight
}

public enum SpecialAbilityType
{
    None
}