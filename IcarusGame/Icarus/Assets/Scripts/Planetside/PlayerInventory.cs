using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    private void Awake()
    {

    }

    public void AddResource(MiningResource resource, int amount)
    {
        UpdateResource(resource.resourceType, amount);

    }
}
