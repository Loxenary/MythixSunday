using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    [Header("Tilemap Settings")]
    public Tilemap tilemap; // Reference to the Tilemap component
    public TileBase[] tiles; // Array of tiles to use for generation

    [Header("Chunk Settings")]
    public int chunkSize = 4; // Size of each chunk (e.g., 16x16 tiles)
    public int loadRadius = 2; // Number of chunks to load around the player

    private Vector2Int _currentChunk; // Current chunk the player is in
    private Vector2Int _lastChunk; // Last chunk the player was in

    private void Start()
    {
        // Initialize the player's starting chunk
        _currentChunk = GetChunkFromPosition(transform.position);
        _lastChunk = _currentChunk;

        // Generate the initial chunks around the player
        UpdateChunks();
    }

    private void Update()
    {
        // Check if the player has moved to a new chunk
        _currentChunk = GetChunkFromPosition(transform.position);

        if (_currentChunk != _lastChunk)
        {
            // Update chunks if the player has moved to a new chunk
            UpdateChunks();
            _lastChunk = _currentChunk;
        }
    }

    private void UpdateChunks()
    {
        // Loop through all chunks within the load radius
        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++)
            {
                Vector2Int chunk = new Vector2Int(_currentChunk.x + x, _currentChunk.y + y);

                // Generate the chunk if it hasn't been generated yet
                if (!IsChunkGenerated(chunk))
                {
                    GenerateChunk(chunk);
                }
            }
        }
    }

    private void GenerateChunk(Vector2Int chunk)
    {
        // Calculate the start position of the chunk
        Vector3Int startPosition = new Vector3Int(chunk.x * chunkSize, chunk.y * chunkSize, 0);

        // Loop through each tile in the chunk
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                // Calculate the tile position
                Vector3Int tilePosition = startPosition + new Vector3Int(x, y, 0);

                // Generate a tile using Perlin Noise
                float noiseValue = Mathf.PerlinNoise((float)tilePosition.x / chunkSize, (float)tilePosition.y / chunkSize);

                if (noiseValue < 0.3f)
                {
                    // Water biome
                    tilemap.SetTile(tilePosition, tiles[0]);
                }
                else if (noiseValue < 0.6f)
                {
                    // Grass biome
                    tilemap.SetTile(tilePosition, tiles[1]);
                }
                else
                {
                    // Mountain biome
                    tilemap.SetTile(tilePosition, tiles[2]);
                }
            }
        }

        // Mark the chunk as generated (optional for optimization)
        MarkChunkAsGenerated(chunk);
    }

    private bool IsChunkGenerated(Vector2Int chunk)
    {
        // Check if the chunk has already been generated
        // For now, we'll assume chunks are always regenerated
        return false;
    }

    private void MarkChunkAsGenerated(Vector2Int chunk)
    {
        // Mark the chunk as generated (optional for optimization)
    }

    private Vector2Int GetChunkFromPosition(Vector3 position)
    {
        // Convert world position to chunk coordinates
        int chunkX = Mathf.FloorToInt(position.x / chunkSize);
        int chunkY = Mathf.FloorToInt(position.y / chunkSize);
        return new Vector2Int(chunkX, chunkY);
    }
}