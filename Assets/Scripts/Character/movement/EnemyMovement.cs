using UnityEngine;

public class EnemyMovement : MovementBase
{
    public float MoveDelay = 1f;
    public float IndicatorDuration = 1f;
    public GameObject IndicatorPrefab;

    private Vector2 _targetPosition;
    private bool _isMoving = false;
    private float _nextMoveTime;

    private void Start(){
        _nextMoveTime = Time.time + MoveDelay;
        
    }


}