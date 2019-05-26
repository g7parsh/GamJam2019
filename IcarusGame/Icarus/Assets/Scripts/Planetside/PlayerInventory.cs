using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    [SerializeField]
    public Dictionary<string, int> resources = new Dictionary<string, int>();

    public void AddResource(MiningResource resource, int amount)
    {
        if (resources.ContainsKey(resource.resourceType.ToString()))
        {
            resources[resource.resourceType.ToString()] += amount;
        }
        else
        {
            resources.Add(resource.resourceType.ToString(), amount);
        }
        foreach (var item in resources.Keys)
        {
            print(item + ":" + resources[item]);
        }
    }
}
