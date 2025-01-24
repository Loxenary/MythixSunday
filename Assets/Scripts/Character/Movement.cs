using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private BaseKey character;

    public BaseKey Key{
        get{return character;}
        set{
            character = value;
        }
    }

    private int currentPointIndex = 0; // Current target point index

    private List<Transform> _pathPoints;
    private Vector3 targetPosition;    // Current target position

    public void Initialize(List<Transform> pathPoints){
        if (pathPoints == null || pathPoints.Count == 0)
        {
            Debug.LogError("Path points are not assigned!");
            enabled = false; // Disable the script if no path points are assigned
            return;
        }

        _pathPoints = pathPoints;

        // Set the initial target position
        gameObject.transform.localPosition = _pathPoints[0].position;
        targetPosition = _pathPoints[currentPointIndex].position;
    }

    private void Update()
    {
        if (!CanMove())
        {
            return;
        }

        MoveAlongPath();
    }

    private bool CanMove()
    {
        // Add any conditions for movement here (e.g., game state, key state)
        if(_pathPoints == null || _pathPoints.Count == 0){
            return false;
        }
        return true;
    }

    private void MoveAlongPath()
    {  
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, character.Character.MovementSpeed * Time.deltaTime);

        // Check if the key has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Move to the next point in the path
            currentPointIndex = (currentPointIndex + 1) % _pathPoints.Count;
            targetPosition = _pathPoints[currentPointIndex].position;
        }
    }
}