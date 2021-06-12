using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevelGrid : MonoBehaviour
{
    public GameObject[] objectsToPickFrom;
    public GameObject[] groundFloor;
    public int gridX;
    public int gridZ;


    public float gridSpaceingOffset = 1f;
    public Vector3 gridOrigin = Vector3.zero;

    private void Start()
    {
        SpawnGrid();
        SpawnObjects();
    }

    void SpawnGrid()
    {
        for(int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 spawnPosition = new Vector3(x * gridSpaceingOffset, 0, z * gridSpaceingOffset) + gridOrigin;
                PickAndSpawnGrid(spawnPosition, Quaternion.identity, true);               
            }
        }
    }

    void PickAndSpawnGrid(Vector3 positionToSpawn, Quaternion rotationToSpawn, bool floor)
    {
        int randomIndex;
        GameObject levelClone;

        if (!floor)
        {
            randomIndex = Random.Range(0, objectsToPickFrom.Length);
            levelClone = Instantiate(objectsToPickFrom[randomIndex], positionToSpawn, rotationToSpawn);
        }

        if (floor)
        {
            randomIndex = Random.Range(0, groundFloor.Length);
            levelClone = Instantiate(groundFloor[randomIndex], positionToSpawn, rotationToSpawn);
        }
    }

    /// <summary>
    /// Pick random game objects to spawn within the grid
    /// </summary>
    /// <param name="positionToSpawn"></param>
    /// <param name="rotationToSpawn"></param>
    void PickAndSpawnObjects(Vector3 positionToSpawn, Quaternion rotationToSpawn)
    {

    }

    void SpawnObjects()
    {
        for (int x = 1; x < gridX - 1; x++)
        {
            for (int z = 1; z < gridZ - 1; z++)
            {
                Vector3 spawnPosition = new Vector3(x * gridSpaceingOffset, 0f, z * gridSpaceingOffset) + gridOrigin;
                PickAndSpawnGrid(spawnPosition, Quaternion.identity,false);
            }
        }
    }

    void RespawnNewLevelGrid()
    {
        // when we complete the level, respawn a new grid 
        SpawnGrid();
    }
}
