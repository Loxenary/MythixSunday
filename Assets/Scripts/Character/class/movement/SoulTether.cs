using UnityEngine;

public class SoulTether : MonoBehaviour
{
    public Transform altCharacter;
    public Transform f4Character;

    [Header("Tether Settings")]
    public float maxDistance = 5f;
    public float gridSize = 1f; // Define the size of the grid for snapping
    public Color tetherColor = Color.blue;
    public float tetherWidth = 0.1f;

    private LineRenderer lineRenderer;
    public float moveSpeed = 5f; // Add movement speed

    void Start()
    {
        // Initialize LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = tetherColor;
        lineRenderer.endColor = tetherColor;
        lineRenderer.startWidth = tetherWidth;
        lineRenderer.endWidth = tetherWidth;
    }

    void Update()
    {
        if (altCharacter != null && f4Character != null)
        {
            // Update the positions of the LineRenderer
            lineRenderer.SetPosition(0, altCharacter.position);
            lineRenderer.SetPosition(1, f4Character.position);
            // Calculate the distance between characters
            float distance = Vector3.Distance(altCharacter.position, f4Character.position);
            // If distance exceeds maxDistance, reposition F4Character with grid snapping
            if (distance > maxDistance)
            {
                Vector3 direction = (f4Character.position - altCharacter.position).normalized;
                Vector3 targetPosition = altCharacter.position + direction * maxDistance;
                Vector3 snappedPosition = SnapToGrid(targetPosition, gridSize);
                
                // Smoothly move F4Character towards the snapped position
                f4Character.position = Vector3.Lerp(f4Character.position, snappedPosition, moveSpeed * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// Snaps a given position to the nearest grid point based on gridSize.
    /// </summary>
    /// <param name="position">The original position.</param>
    /// <param name="gridSize">The size of the grid for snapping.</param>
    /// <returns>The snapped position.</returns>
    Vector3 SnapToGrid(Vector3 position, float gridSize)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = Mathf.Round(position.y / gridSize) * gridSize;
        float z = Mathf.Round(position.z / gridSize) * gridSize;
        return new Vector3(x, y, z);
    }
}