using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private Transform[] pathPoints;   // Array of points defining the path

    private int currentPointIndex = 0; // Current target point index
    private Vector3 targetPosition;    // Current target position

    private void Start()
    {
        if (pathPoints == null || pathPoints.Length == 0)
        {
            Debug.LogError("Path points are not assigned!");
            enabled = false; // Disable the script if no path points are assigned
            return;
        }

        // Set the initial target position
        targetPosition = pathPoints[currentPointIndex].position;
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
        return true;
    }

    private void MoveAlongPath()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, character.MovementSpeed * Time.deltaTime);

        // Check if the key has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Move to the next point in the path
            currentPointIndex = (currentPointIndex + 1) % pathPoints.Length;
            targetPosition = pathPoints[currentPointIndex].position;
        }
    }
}