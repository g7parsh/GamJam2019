using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    [SerializeField]
    private List<MiningResource> resources = new List<MiningResource>();

    void AddResource(MiningResource resource, int amount)
    {
        if (resources.Contains(resource))
        {
            MiningResource temp = resources.Find(x => { return x == resource; });
            temp.Quantity += amount;
        }
        else
        {
            resources.Add(resource);
            MiningResource temp = resources.Find(x => { return x == resource; });
            temp.Quantity += amount;
        }

        print(resources);
    }
}
