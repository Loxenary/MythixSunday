using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapAndObjectGenerator : MonoBehaviour
{
    [Header("Tilemap Settings")]
    public Tilemap tilemap; // Reference to the Tilemap
    public TileBase[] tileVariants; // Array of your 3 tile variants (assign in Inspector)
    public int mapWidth = 50; // Width of the map
    public int mapHeight = 50; // Height of the map
    public Vector2 cellSize = new Vector2(2, 2); // Tilemap cell size (2x2 in your case)

    [Header("Perlin Noise Settings")]
    public float noiseScale = 0.1f; // Controls the "zoom" of the noise
    public float[] noiseThresholds = { 0.3f, 0.6f }; // Thresholds for tile variants

    [Header("Object Placement Settings")]
    public GameObject[] objectPrefabs; // Objects to place on tiles
    public int maxObjects = 100; // Maximum number of objects to place
    public float minDistanceMultiplier = 1f; // Minimum distance between objects (1 times area)
    public int skipTilesRandomly = 5; // Skip tiles randomly to add variability
    private List<GameObject> placedObjects = new List<GameObject>(); // Track placed objects

    public Transform objectsContainer;
    

    [Header("Spawn Point Settings")]

    private GameObject ALTPlayer;
    private GameObject F4Player;
    public Transform spawnPointsContainer;

    void Start()
    {
        ALTPlayer = GameObject.FindWithTag("ALT");
        F4Player = GameObject.FindWithTag("F4");

        GenerateMap();
        PlacePlayerRandomly();
        PlaceObjectsOnTiles();
        CreateSpawnPoints();
    }

    void GenerateMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Use Perlin noise to determine tile type
                float noiseValue = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);
                TileBase tileToPlace = ChooseTileBasedOnNoise(noiseValue);

                // Place the tile
                tilemap.SetTile(tilePosition, tileToPlace);
            }
        }

        Debug.Log($"Generated {mapWidth}x{mapHeight} map with {tileVariants.Length} tile variants.");
    }

    TileBase ChooseTileBasedOnNoise(float noiseValue)
    {
        if (noiseValue < noiseThresholds[0])
        {
            return tileVariants[0]; // Tile variant 1
        }
        else if (noiseValue < noiseThresholds[1])
        {
            return tileVariants[1]; // Tile variant 2
        }
        else
        {
            return tileVariants[2]; // Tile variant 3
        }
    }

    void CreateSpawnPoints()
    {
        if (SpawnerManager.Instance.SpawnPointPrefab == null)
        {
            Debug.LogError("Spawn point prefab is not assigned!");
            return;
        }

        List<Vector3Int> validTilePositions = new List<Vector3Int>();

        // Find all valid tile positions
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(tilePosition))
                {
                    validTilePositions.Add(tilePosition);
                }
            }
        }

        // Randomly select positions for spawn points
        for (int i = 0; i < SpawnerManager.Instance.NumberOfSpawnPoints; i++)
        {
            if (validTilePositions.Count == 0)
            {
                Debug.LogWarning("Not enough valid tile positions to place spawn points!");
                break;
            }

            int randomIndex = Random.Range(0, validTilePositions.Count);
            Vector3Int spawnPosition = validTilePositions[randomIndex];
            validTilePositions.RemoveAt(randomIndex); // Ensure no duplicate spawn points

            // Instantiate the spawn point
            Vector3 worldPosition = tilemap.GetCellCenterWorld(spawnPosition);
            GameObject spawnPoint = Instantiate(SpawnerManager.Instance.SpawnPointPrefab, worldPosition, Quaternion.identity, spawnPointsContainer);

            // Add the spawn point to the SpawnerManager
            if (spawnPoint.TryGetComponent<SpawnPoint>(out SpawnPoint sp))
            {
                SpawnerManager.Instance.SpawnPoints.Add(sp);
            }
        }

        Debug.Log($"Created {SpawnerManager.Instance.NumberOfSpawnPoints} spawn points.");
    }

    private void PlacePlayerRandomly(){
        Vector3 worldPos = tilemap.GetCellCenterWorld(new Vector3Int(Random.Range(0, mapWidth), Random.Range(0, mapHeight),0));
        ALTPlayer.transform.position = worldPos;
        F4Player.transform.position = worldPos + new Vector3(4,4,0);

    }

    void PlaceObjectsOnTiles()
    {
        int objectsPlaced = 0;

        // Iterate over each tile systematically
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // Skip tiles randomly to add variability
                if (Random.Range(0, skipTilesRandomly) != 0)
                {
                    continue;
                }

                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Check if there's a tile at this position
                if (tilemap.HasTile(tilePosition))
                {
                    // Randomly select an object prefab
                    GameObject objectPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

                    // Calculate the object's area (assuming the object has a Collider2D)
                    float objectArea = CalculateObjectArea(objectPrefab);

                    // Calculate the minimum required distance to other objects
                    float minDistance = minDistanceMultiplier * objectArea;

                    // Convert tile position to world position (accounting for cell size)
                    Vector3 worldPosition = tilemap.GetCellCenterWorld(tilePosition);

                    // Check if the object can be placed without violating the distance constraint
                    if (IsFarEnoughFromOtherObjects(worldPosition, minDistance))
                    {
                        // Place the object
                        GameObject newObject = Instantiate(objectPrefab, worldPosition, Quaternion.identity, objectsContainer);
                        placedObjects.Add(newObject);
                        objectsPlaced++;

                        // Stop if we've placed the maximum number of objects
                        if (objectsPlaced >= maxObjects)
                        {
                            return;
                        }
                    }
                }
            }
        }

        Debug.Log($"Placed {objectsPlaced} objects on the map.");
    }

    bool IsFarEnoughFromOtherObjects(Vector3 position, float minDistance)
    {
        foreach (GameObject obj in placedObjects)
        {
            if (Vector3.Distance(position, obj.transform.position) < minDistance)
            {

                return false;
            }
        }
        return true;
    }

    float CalculateObjectArea(GameObject obj)
    {
        // Get the Collider2D component
        Collider2D collider = obj.GetComponent<Collider2D>();
        if (collider != null)
        {
            // Calculate the area of the object's base (width * height)
            if (collider is BoxCollider2D boxCollider)
            {
                return Random.Range(1,6) *  boxCollider.size.y;
            }
            else if (collider is CircleCollider2D circleCollider)
            {
                return Mathf.PI * circleCollider.radius * circleCollider.radius;
            }
            else if (collider is PolygonCollider2D)
            {
                // For PolygonCollider2D, approximate the area using bounds
                Bounds bounds = collider.bounds;
                return bounds.size.x * bounds.size.y;
            }
        }

        // Default area if no collider is found
        return 1f;
    }
}