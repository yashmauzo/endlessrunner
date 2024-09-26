using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    // Array of tile prefabs to be spawned, consisting of road and obstacle prefabs
    public GameObject[] tilePrefabs;
    public float zSpawn = 0;    // The Z position where the next tile will be spawned
    public float tileLength = 30;   // Length of each tile
    public int numberOfTiles = 5;   // Number of active tiles in the scene at any time
    private List<GameObject> activeTiles = new List<GameObject>();  // List of currently active tiles
    private bool lastWasObstacle = false;  // Track if the last spawned tile was an obstacle
    public Transform playerTransform;      // Reference to the player's transform for spawning logic

    void Start()
    {
        // Always spawn the first tile as a road (index 0, representing road prefab)
        SpawnTile(0);  // Road prefab at index 0
        lastWasObstacle = false;  // First tile is a road, so the next can be an obstacle

        // Spawn the remaining initial set of tiles
        for (int i = 1; i < numberOfTiles; i++)
        {
            SpawnTile(GetNextTileIndex());  // Determine next tile to spawn (alternating between road and obstacle)
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player has moved far enough to spawn a new tile
        if (playerTransform.position.z - 35 > zSpawn - (numberOfTiles * tileLength))
        {
            SpawnTile(GetNextTileIndex());      // Spawn a new tile at the end of the active tiles
            DeleteTile();       // Remove the oldest tile to maintain the number of active tiles
        }
    }

    // Spawns a tile at the current zSpawn position
    public void SpawnTile(int tileIndex)
    {
        GameObject go;

        // Check if the last tile was an obstacle to ensure alternating pattern
        if (!lastWasObstacle)
        {
            // Spawn a random obstacle (non-road prefab, index 1 to 3)
            go = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
            lastWasObstacle = true;  // Mark that an obstacle was just spawned
        }
        else
        {
            // Spawn the road prefab (index 0)
            go = Instantiate(tilePrefabs[0], transform.forward * zSpawn, transform.rotation);
            lastWasObstacle = false;  // Mark that a road was spawned
        }

        activeTiles.Add(go);    // Add the new tile to the list of active tiles
        zSpawn += tileLength;   // Update the zSpawn position for the next tile
    }

    // Deletes the oldest tile
    private void DeleteTile()
    {
        Destroy(activeTiles[0]);    // Destroy the first tile in the list
        activeTiles.RemoveAt(0);    // Remove it from the active tiles list
    }

    // Get the next tile index, ensuring that road and obstacle tiles alternate
    private int GetNextTileIndex()
    {
        int tileIndex;

        // If the last spawned tile was an obstacle, the next one must be a road (index 0)
        if (lastWasObstacle)
        {
            tileIndex = 0;  // Road prefab at index 0
        }
        else
        {
            // Pick a random obstacle prefab (index 1 to 3)
            tileIndex = Random.Range(1, tilePrefabs.Length);
        }

        return tileIndex;
    }
}
