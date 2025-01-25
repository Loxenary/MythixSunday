using UnityEngine;

public class MovementBase : MonoBehaviour
{
    private Vector2 _targetPosition;

    protected bool _isMoving = false;

    protected float _moveX  = 0f;
    protected float _moveY = 0f;

    private void Start(){
        _targetPosition = SnapToGrid(transform.position);
    }  

    protected void Move(){
        float gridSize = GridManager.Instance.GridSize;
        if(_moveX != 0 || _moveY != 0){
            Vector2 newPosition = new Vector2(transform.position.x + _moveX * gridSize, transform.position.y + _moveY * gridSize);
                
            _targetPosition = newPosition;
            _isMoving = true;   
        }
    } 

    private Vector2 SnapToGrid(Vector2 position){
        float gridSize = GridManager.Instance.GridSize;
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector2(snappedX, snappedY);
    }   

    protected void MoveToTarget(){
        float gridSize = GridManager.Instance.GridSize;
        transform.position = Vector2.MoveTowards(transform.position,_targetPosition, gridSize * Time.deltaTime * 10f);

        if((Vector2) transform.position == _targetPosition){
            _isMoving = false;
        }
    }
}