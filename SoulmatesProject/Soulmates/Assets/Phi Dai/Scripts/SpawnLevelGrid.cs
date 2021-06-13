using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevelGrid : MonoBehaviour
{
    public GameObject[] objectsToPickFrom;
    //int objIndex = 0;
    public GameObject[] groundFloor;
    public GameObject playerObject;
    public static Vector3 playerStartPositionRef;
    Vector3 playerSpawnPosition;


    //GameObject[] tempStoredObjects = new GameObject[5];
    //int tempIndex = 0;
    public int gridX;
    public int gridZ;

    public int holesLimit;
    public int unbreakableTombstoneLimit;
    public int breakableTombstoneLimit;
    public int spikeLimit;
    public int portalLimit; 
    int holes;
    int unbreakableTombstoneCounter;
    int breakableTombstoneCounter;
    int spikeCounter;
    int spawnPortals;

    public float gridSpaceingOffset = 1f;
    public Vector3 gridOrigin = Vector3.zero;

    private void Start()
    {
        SpawnGrid();
        SpawnObjects();
       // SpawnPlayer();
        playerStartPositionRef = playerSpawnPosition;
    }

    void SpawnGrid()
    {
        for(int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 spawnPosition = new Vector3(x * gridSpaceingOffset, 0, z * gridSpaceingOffset) + gridOrigin;
                //if (x != gridX / 2 && z != gridZ / 2)
                    PickAndSpawnGrid(spawnPosition, Quaternion.identity, true);   
                
            }
        }
    }

    void SpawnObjects()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 spawnPosition = new Vector3(x * gridSpaceingOffset, 0f, z * gridSpaceingOffset) + gridOrigin;
                if(x != gridX/2 && z != gridZ/2)
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

            if ((objectsToPickFrom[randomIndex].name == "Tombstone_01" || objectsToPickFrom[randomIndex].name == "Tombstone_02") && breakableTombstoneCounter < breakableTombstoneLimit)
            {
                breakableTombstoneCounter++;
                levelClone = Instantiate(objectsToPickFrom[randomIndex], positionToSpawn, rotationToSpawn);

            }

            if ( (objectsToPickFrom[randomIndex].name == "Tombstone_03" || objectsToPickFrom[randomIndex].name == "Pillar" || objectsToPickFrom[randomIndex].name == "Fire") && unbreakableTombstoneCounter < unbreakableTombstoneLimit)
            {
                unbreakableTombstoneCounter++;
                levelClone = Instantiate(objectsToPickFrom[randomIndex], positionToSpawn, rotationToSpawn);

            }

            if (objectsToPickFrom[randomIndex].name == "Spikes" && spikeCounter < spikeLimit)
            {
                spikeCounter++;
                levelClone = Instantiate(objectsToPickFrom[randomIndex], positionToSpawn, rotationToSpawn);

            }

            if (objectsToPickFrom[randomIndex].name == "Exit" && spawnPortals < portalLimit)
            {
                spawnPortals++;
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
                //Debug.Log("holes: " + holes);
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

    void SpawnPlayer()
    {
        int xMidpoint = gridX / 2;
        int zMidpoint = gridZ / 2;

        GameObject playerClone;
        playerSpawnPosition = new Vector3(xMidpoint * gridSpaceingOffset, 1f, zMidpoint * gridSpaceingOffset) + gridOrigin;
        playerClone = Instantiate(playerObject, playerSpawnPosition, Quaternion.identity);
    }
}
