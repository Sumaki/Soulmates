using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevelGrid : MonoBehaviour
{
    public GameObject[] objectsToPickFrom;
    int objIndex = 0;
    public GameObject[] groundFloor;
    
    GameObject[] tempStoredObjects = new GameObject[5];
    int tempIndex = 0;
    public int gridX;
    public int gridZ;

    public int holesLimit;
    public int unbreakableTombstoneLimit;
    public int breakableTombstoneLimit;
    public int spikeLimit;
    int holes;
    int unbreakableTombstoneCounter;
    int breakableTombstoneCounter;
    int spikeCounter;

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

    void PickAndSpawnGrid(Vector3 positionToSpawn, Quaternion rotationToSpawn, bool floor)
    {
        int randomIndex;
        GameObject levelClone;

        if (!floor)
        {
            randomIndex = Random.Range(0, objectsToPickFrom.Length);

            if (objectsToPickFrom[randomIndex].name == "Tombstone_01" && breakableTombstoneCounter < breakableTombstoneLimit)
            {
                breakableTombstoneCounter++;
                levelClone = Instantiate(objectsToPickFrom[randomIndex], positionToSpawn, rotationToSpawn);

            }

            if ( (objectsToPickFrom[randomIndex].name == "Tombstone_02" || objectsToPickFrom[randomIndex].name == "Tombstone_03" || objectsToPickFrom[randomIndex].name == "Pillar") && unbreakableTombstoneCounter < unbreakableTombstoneLimit)
            {
                unbreakableTombstoneCounter++;
                levelClone = Instantiate(objectsToPickFrom[randomIndex], positionToSpawn, rotationToSpawn);

            }

            if (objectsToPickFrom[randomIndex].name == "Spikes" && spikeCounter < spikeLimit)
            {
                spikeCounter++;
                levelClone = Instantiate(objectsToPickFrom[randomIndex], positionToSpawn, rotationToSpawn);

            }

            //if (objectsToPickFrom[randomIndex].name != "Tombstone_01" || objectsToPickFrom[randomIndex].name != "Tombstone_02" || objectsToPickFrom[randomIndex].name != "Spikes" || objectsToPickFrom[randomIndex].name != "Tombstone_03")
            //    levelClone = Instantiate(objectsToPickFrom[randomIndex], positionToSpawn, rotationToSpawn);

        }

        if (floor)
        {
            randomIndex = Random.Range(0, groundFloor.Length);

            if (groundFloor[randomIndex].name == "Empty_ObjectFloor" && holes < holesLimit)
            {
                holes++;
                Debug.Log("holes: " + holes);
                levelClone = Instantiate(groundFloor[randomIndex], positionToSpawn, rotationToSpawn);

            }
            else
            {
                randomIndex = Random.Range(0,2);
                levelClone = Instantiate(groundFloor[randomIndex], positionToSpawn, rotationToSpawn);
            }

            

        }
    }


    void RespawnNewLevelGrid()
    {
        // when we complete the level, respawn a new grid, objects and reset limits
        SpawnGrid();
        SpawnObjects();
    }
}
