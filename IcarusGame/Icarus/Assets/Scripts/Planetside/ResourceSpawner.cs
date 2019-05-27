using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public MiningResource[] resources;
    /*
     0 = small
     1 = med
     2 = large
         */

    [Range(10, 100)]
    public int numToSpawn = 100;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= numToSpawn; i++)
        {
            MiningResource temp;
            int resourceAmount = Random.Range(10, 100);
            Inventory.RESOURCE type = (Inventory.RESOURCE)Random.Range(0, 2);
            if (resourceAmount > 75)
            {
                temp = resources[2];
                temp.resourceType = type;
                Instantiate(temp, Random.onUnitSphere, Quaternion.identity, this.transform);
            }
            else if (resourceAmount > 50)
            {

                temp = resources[1];
                temp.resourceType = type;
                Instantiate(temp, Random.onUnitSphere, Quaternion.identity, this.transform);
            }
            else
            {

                temp = resources[0];
                temp.resourceType = type;
                Instantiate(temp, Random.onUnitSphere, Quaternion.identity, this.transform);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
