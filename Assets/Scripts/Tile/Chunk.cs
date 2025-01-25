using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chunk {

    private readonly float playerSafeRadius;

    public Vector2Int chunkPosition; // Position of the chunk in chunk coordinates
    public GameObject chunkObject; // GameObject to hold the Tilemap and objects
    public Tilemap tilemap; // Tilemap for floor tiles

    private readonly int chunkSize = 10; // Number of tiles per chunk (e.g., 10x10)
    private readonly float tileSize = 1f; // Size of each tile

    private readonly Tile[] floorTiles; // Array of floor tile assets
    private readonly Tile deepFloorTile; // Deep floor tile asset
    private readonly GameObject[] objectPrefabs; // Array of object prefabs

    private readonly float objectSpawnChance = 0.1f; // Chance to spawn an object on a tile
    private readonly float deepFloorThreshold = 0.3f; // Perlin noise threshold for deep floor

    private readonly float perlinScale = 0.1f; // Scale for Perlin noise
    private Vector2 perlinOffset; // Offset for Perlin noise
    private GameObject player;

    private Dictionary<GameObject, float> objectSpawnChances = new Dictionary<GameObject, float>();

    public Chunk(Vector2Int position, Tile[] floorTiles, Tile deepFloorTile, GameObject[] objects, Vector2 offset, float playerSafeRadius) {
        chunkPosition = position;
        this.floorTiles = floorTiles;
        this.deepFloorTile = deepFloorTile;
        this.objectPrefabs = objects;
        this.perlinOffset = offset;
        this.playerSafeRadius = playerSafeRadius;
        player = GameObject.FindWithTag("ALT");
        
        GenerateChunk();
         // Assign spawn chances based on object size
        foreach (var obj in objects) {
            float size = obj.GetComponent<Collider2D>().bounds.size.magnitude;
            objectSpawnChances[obj] = Mathf.Clamp(1f / size, objectSpawnChance, 1f); 
        }
        
    }

     private void GenerateChunk() {
        // Create a parent GameObject for this chunk
        chunkObject = new GameObject($"Chunk_{chunkPosition.x}_{chunkPosition.y}");
        chunkObject.transform.position = new Vector3(chunkPosition.x * chunkSize * tileSize, 0, chunkPosition.y * chunkSize * tileSize);

        // Add a Tilemap component to the chunk
        tilemap = chunkObject.AddComponent<Tilemap>();
        TilemapRenderer renderer = chunkObject.AddComponent<TilemapRenderer>();

        // Generate tiles and objects
        for (int x = 0; x < chunkSize; x++) {
            for (int y = 0; y < chunkSize; y++) {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                Vector3 worldPosition = tilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0);

                // Skip object placement within the safe radius
                if (Vector3.Distance(worldPosition, player.transform.position) < playerSafeRadius) {
                    continue;
                }

                // Calculate Perlin noise value for this tile
                float perlinValue = PerlinNoise.GetPerlinValue(
                    chunkPosition.x * chunkSize + x,
                    chunkPosition.y * chunkSize + y,
                    perlinScale,
                    perlinOffset.x,
                    perlinOffset.y
                );

                // Set floor tile based on Perlin noise
                Tile floorTile = floorTiles[Mathf.FloorToInt(perlinValue * floorTiles.Length)];
                tilemap.SetTile(tilePosition, floorTile);

                // Set deep floor tile if Perlin noise is below the threshold
                if (perlinValue < deepFloorThreshold) {
                    tilemap.SetTile(tilePosition, deepFloorTile);
                } else {
                    // Spawn objects (if not deep floor)
                   
                        GameObject objectPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
                        if(objectPrefab == null){
                            continue;
                        }
                        
                        if (objectSpawnChances.TryGetValue(objectPrefab, out var spawnChance) && Random.value < spawnChance) {
                            
                            if (CanPlaceObject(objectPrefab, worldPosition)) {
                                GameObject obj = Object.Instantiate(objectPrefab, worldPosition, Quaternion.identity, chunkObject.transform);
                            }
                        }
                    
                }
            }
        }

    }

    private bool CanPlaceObject(GameObject objectPrefab, Vector3 position) {
        // Check if the object can be placed without overlapping others
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, objectPrefab.GetComponent<Collider2D>().bounds.size, 0);
        return colliders.Length == 0; // No overlapping colliders
    }

    public void DestroyChunk() {
        if (chunkObject != null) {
            Object.Destroy(chunkObject);
        }
    }
}