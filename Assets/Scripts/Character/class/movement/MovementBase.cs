using UnityEngine;

public class MovementBase : MonoBehaviour
{
    protected Vector2 _targetPosition;
    protected bool _isMoving = false;
    protected float _moveX = 0f;
    protected float _moveY = 0f;

    [Header("Collision Settings")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float collisionCheckRadius = 0.4f; 

    private void Start()
    {
        _targetPosition = SnapToGrid(transform.position);
        transform.position = _targetPosition;
    }

    public void ResetMovement()
    {
        _moveX = 0f;
        _moveY = 0f;
    }

    protected void Move()
    {
        float gridSize = GridManager.Instance.GridSize;

        if (_moveX != 0 && _moveY != 0)
        {
            // Prioritize horizontal movement over vertical movement
            _moveX = 0f;
        }

        if (_moveX != 0 || _moveY != 0)
        {
            Vector2 newPosition = new(transform.position.x + _moveX * gridSize, transform.position.y + _moveY * gridSize);

            // Check for collisions before moving
            if (!CheckForCollision(newPosition))
            {
                _targetPosition = SnapToGrid(newPosition);
                _isMoving = true;
            }
            else
            {
                Debug.Log("Collision detected! Movement blocked.");
            }
        }
    }

    public Vector2 GetMovementDirection()
    {
        return new Vector2(_moveX, _moveY);
    }

    protected Vector2 SnapToGrid(Vector2 position)
    {
        Grid grid = GridManager.Instance.Grid;
        Vector3Int cellPosition = grid.WorldToCell(position);
        Vector3 snappedPosition = grid.CellToWorld(cellPosition);
        return new Vector2(snappedPosition.x, snappedPosition.y);
    }

    protected void MoveToTarget()
    {
        float gridSize = GridManager.Instance.GridSize;
        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, gridSize * Time.deltaTime * 10f);

        if ((Vector2)transform.position == _targetPosition)
        {
            _isMoving = false;
        }
    }

    private bool CheckForCollision(Vector2 targetPosition)
    {
        // Check for collisions at the target position
        Collider2D hit = Physics2D.OverlapCircle(targetPosition, collisionCheckRadius, obstacleLayer);
        return hit != null; // Return true if a collision is detected
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the collision check radius in the editor for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_targetPosition, collisionCheckRadius);
    }
}