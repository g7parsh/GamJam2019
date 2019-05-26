using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    private void Awake()
    {

    }

    public void AjustResource(MiningResource resource, int amount)
    {
        UpdateResource(resource.resourceType, amount);

    }
}
