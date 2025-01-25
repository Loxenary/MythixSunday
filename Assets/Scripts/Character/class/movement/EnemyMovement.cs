using System.Collections;
using UnityEngine;

public class EnemyMovement : MovementBase
{
    public Enemy enemyData;
    private float MoveDelay;
    private float IndicatorDuration;
    // [SerializeField] private GameObject IndicatorPrefab;

    private float _nextMoveTime;

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
            // ShowIndicator(_targetPosition);
            IndicatorManager.Instance.ShowMovementIndicator(_targetPosition, enemyData);
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

    //     private void ShowIndicator(Vector2 position)
    // {
    //     // Instantiate the indicator at the target position
    //     GameObject indicator = Instantiate(IndicatorPrefab, position, Quaternion.identity);
        

    //     // Destroy the indicator after the specified duration
    //     StartCoroutine(ChangeIndicatorColor(indicator));
    // }

    // private IEnumerator ChangeIndicatorColor(GameObject indicator){
    //     Debug.Log(indicator);
    //     SpriteRenderer indicatorRenderer = indicator.GetComponent<SpriteRenderer>();
    //     float elapsedTime = 0f;

    //     Color startColor = new Color(1f,0,0,0/5f);
    //     Color endColor = new (1f,0,0,1f);

    //      while (elapsedTime < IndicatorDuration)
    //     {
    //         // Interpolate the color based on the elapsed time
    //         float t = elapsedTime / IndicatorDuration;
    //         indicatorRenderer.color = Color.Lerp(startColor, endColor, t);

    //         // Wait for the next frame
    //         elapsedTime += Time.deltaTime;
    //         yield return null;

    //         // Ensure the final color is set
    //          if (indicator != null && indicatorRenderer != null)
    //         {
    //             indicatorRenderer.color = endColor;
    //         }
   
    //     }
        // // Destroy the indicator after the duration
        // if (indicator != null)
        // {
        //     Destroy(indicator);
        // }
//     }
}