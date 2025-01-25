using System.Collections;
using UnityEngine;

public class EnemyMovement : MovementBase
{
    public Enemy enemyData;
    private float MoveDelay;
    private float IndicatorDuration;
    // [SerializeField] private GameObject IndicatorPrefab;

    private float _nextMoveTime;

    private Indicator _enemyIndicator;

    private float _gridSize; 

    private void Start(){
        _nextMoveTime = Time.time + MoveDelay;
        _gridSize =  GridManager.Instance.GridSize;
        IndicatorDuration = enemyData.movementIndicatorDuration;
        MoveDelay = enemyData.moveDelay;
    }

    void Update()
    {
        if (Time.time >= _nextMoveTime && !_isMoving)
        {
            
        // Choose a random direction
            Vector2 randomDirection = GetRandomDirection();
            Vector2 newPosition = new Vector2(
                transform.position.x + randomDirection.x * _gridSize,
                transform.position.y + randomDirection.y * _gridSize
            );

            // Snap the new position to the grid
            _targetPosition = SnapToGrid(newPosition);

            // Show the red indicator at the target position
            
            ShowIndicator(_targetPosition);
            // IndicatorManager.Instance.ShowMovementIndicator(_targetPosition, enemyData);
            // StartCoroutine(ChangeIndicatorColor(enemyData.movementIndicatorPrefab));

            // Start moving after the indicator duration
            Invoke(nameof(StartMoving), IndicatorDuration);

            // Schedule the next move
            _nextMoveTime = Time.time + MoveDelay + IndicatorDuration;
        }

        if (_isMoving)
        {
            MoveToTarget();
        }
    }

    private void StartMoving()
    {
        _isMoving = true;
    }

    private Vector2 GetRandomDirection()
    {
        int direction = Random.Range(0, 4);
        return direction switch
        {
            0 => Vector2.up,// Up
            1 => Vector2.down,// Down
            2 => Vector2.left,// Left
            3 => Vector2.right,// Right
            _ => Vector2.zero,
        };
    }

        private void ShowIndicator(Vector2 position)
    {
        Debug.Log("IS THIS CALLED");
        GameObject obj = Instantiate(enemyData.movementIndicatorPrefab, position, Quaternion.identity);
        _enemyIndicator = obj.GetComponent<Indicator>();

        // Destroy the indicator after the specified duration
        if (_enemyIndicator != null)
        {
            StartCoroutine(_enemyIndicator.Initialize(enemyData.movementIndicatorColor, enemyData.movementIndicatorDuration, enemyData.movementIndicatorPrefab));
        }
        else
        {
            Debug.LogWarning("Movement Indicator Prefab does not have an Indicator script attached.");
        }
    }
}