using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public float zSpawn = 0;
    public float tileLength = 30;
    public int numberOfTiles = 5;
    private List<GameObject> activeTiles = new List<GameObject>();
    // private int lastTileIndex = -1;   // Index of the last spawned tile
    // private bool lastWasObstacle = false;  // Track if the last spawned tile was an obstacle
    private bool lastWasObstacle = false;  // Track if the last spawned tile was an obstacle
    public Transform playerTransform;
    void Start()
    {
        // // Spawn the initial set of tiles
        // for (int i = 0; i < numberOfTiles; i++)
        // {
        //     if (i == 0)
        //         SpawnTile(0); // Always spawn the first tile as index 0
        //     else
        //         // SpawnTile(Random.Range(0, tilePrefabs.Length));
        //         SpawnTile(GetNextTileIndex());  // Spawn random tiles, ensuring no consecutive duplicates
        // }


        // Always spawn the first tile as a road (index 0)
        SpawnTile(0);  // Road prefab at index 0
        lastWasObstacle = false;  // First tile is a road, so the next can be an obstacle

        // Spawn the rest of the initial set of tiles
        for (int i = 1; i < numberOfTiles; i++)
        {
            SpawnTile(GetNextTileIndex());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we need to spawn a new tile
        if (playerTransform.position.z - 35 > zSpawn - (numberOfTiles * tileLength))
        {
            // SpawnTile(Random.Range(0, tilePrefabs.Length));
            SpawnTile(GetNextTileIndex());
            DeleteTile();
        }
    }

    // Spawns a tile at the current zSpawn position
    public void SpawnTile(int tileIndex)
    {
        GameObject go;

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

        activeTiles.Add(go);
        zSpawn += tileLength;

        // // Spawns a tile at the current zSpawn position
        // public void SpawnTile(int tileIndex)
        // {
        //     GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        //     activeTiles.Add(go);
        //     zSpawn += tileLength;
    }

    // Deletes the oldest tile
    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    // Get the next tile index, ensuring it's not the same as the last spawned one
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

        // int tileIndex = Random.Range(0, tilePrefabs.Length);

        // // Ensure that the new tile is not the same as the last one
        // while (tileIndex == lastTileIndex)
        // {
        //     tileIndex = Random.Range(0, tilePrefabs.Length);
        // }

        // // Update the lastTileIndex to the current tile index
        // lastTileIndex = tileIndex;

        // return tileIndex;
    }
}
