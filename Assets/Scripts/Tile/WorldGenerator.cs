using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour {
    public Tile[] floorTiles; // Assign in Inspector
    public Tile deepFloorTile; // Assign in Inspector
    public GameObject[] objectPrefabs; // Assign in Inspector

    public int chunkSize = 10; // Number of tiles per chunk
    public int renderDistance = 2; // Number of chunks to load around the player

    private Dictionary<Vector2Int, Chunk> loadedChunks = new Dictionary<Vector2Int, Chunk>();
    private Queue<Chunk> chunkPool = new Queue<Chunk>(); // Pool of reusable chunks

    public float playerSafeRadius = 5f;

    private Vector2Int lastPlayerChunkPosition;
    private Transform player;

    private Vector2 perlinOffset; // Offset for Perlin noise

    void Start() {
        player = GameObject.FindGameObjectWithTag("ALT").transform;
        lastPlayerChunkPosition = GetChunkPosition(player.position);

        // Randomize Perlin noise offset
        perlinOffset = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));

        LoadChunksAroundPlayer();
    }

    void Update() {
        Vector2Int currentPlayerChunkPosition = GetChunkPosition(player.position);

        // Check if the player has moved to a new chunk
        if (currentPlayerChunkPosition != lastPlayerChunkPosition) {
            lastPlayerChunkPosition = currentPlayerChunkPosition;
            LoadChunksAroundPlayer();
        }
    }

    private Vector2Int GetChunkPosition(Vector3 worldPosition) {
        int chunkX = Mathf.FloorToInt(worldPosition.x / (chunkSize));
        int chunkY = Mathf.FloorToInt(worldPosition.z / (chunkSize));
        return new Vector2Int(chunkX, chunkY);
    }

    private void LoadChunksAroundPlayer() {
        HashSet<Vector2Int> chunksToKeep = new HashSet<Vector2Int>();

        for (int x = -renderDistance; x <= renderDistance; x++) {
            for (int y = -renderDistance; y <= renderDistance; y++) {
                Vector2Int chunkPosition = new Vector2Int(lastPlayerChunkPosition.x + x, lastPlayerChunkPosition.y + y);

                if (!loadedChunks.ContainsKey(chunkPosition)) {
                    Chunk chunk = GetChunkFromPool();
                    chunk = new Chunk(chunkPosition, floorTiles, deepFloorTile, objectPrefabs, perlinOffset, playerSafeRadius);
                    loadedChunks.Add(chunkPosition, chunk);
                }

                chunksToKeep.Add(chunkPosition);
            }
        }

        // Return unused chunks to the pool
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var chunk in loadedChunks) {
            if (!chunksToKeep.Contains(chunk.Key)) {
                chunk.Value.DestroyChunk();
                chunkPool.Enqueue(chunk.Value);
                chunksToRemove.Add(chunk.Key);
            }
        }

        foreach (var chunkPosition in chunksToRemove) {
            loadedChunks.Remove(chunkPosition);
        }
    }

    private Chunk GetChunkFromPool() {
        if (chunkPool.Count > 0) {
            return chunkPool.Dequeue();
        }
        return null; // Create a new chunk if the pool is empty
    }

    
}