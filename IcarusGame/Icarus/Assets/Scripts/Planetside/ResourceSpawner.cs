using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public MiningResource[] resources;
    public Terrain terrain;
    /*
     0 = small
     1 = med
     2 = large
         */


    [Range(10, 100)]
    public int numToSpawn = 100;
    private int terrainWidth;
    private int terrainLength;
    private int terrainPosX;
    private int terrainPosZ;

    // Start is called before the first frame update
    void Start()
    {// terrain size x
        terrainWidth = (int)terrain.terrainData.size.x;
        // terrain size z
        terrainLength = (int)terrain.terrainData.size.z;
        // terrain x position
        terrainPosX = (int)terrain.transform.position.x;
        // terrain z position
        terrainPosZ = (int)terrain.transform.position.z;

        for (int i = 0; i <= numToSpawn; i++)
        {
            int resourceAmount = Random.Range(10, 100);
            Inventory.RESOURCE type = (Inventory.RESOURCE)Random.Range(0, 3);
            MiningResource temp;
            if (resourceAmount > 66)
            {
                temp = resources[2];
                temp.Quantity = resourceAmount;
                temp.resourceType = type;
                Instantiate(temp, SpawnLocation(), Quaternion.identity, this.transform);
            }
            else if (resourceAmount > 33)
            {

                temp = resources[1];
                temp.resourceType = type;

                temp.Quantity = resourceAmount;
                Instantiate(temp, SpawnLocation(), Quaternion.identity, this.transform);
            }
            else
            {

                temp = resources[0];
                temp.resourceType = type;

                temp.Quantity = resourceAmount;
                Instantiate(temp, SpawnLocation(), Quaternion.identity, this.transform);
            }
        }

    }
    public Vector3 SpawnLocation()
    {

        int posx = Random.Range(terrainPosX, terrainPosX + terrainWidth);
        // generate random z position
        int posz = Random.Range(terrainPosZ, terrainPosZ + terrainLength);
        // get the terrain height at the random position
        float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));
        // create new gameObject on random position


        return new Vector3(posx, posy, posz);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
