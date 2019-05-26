using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public enum RESOURCE
    {
        FUEL,
        REPAIR,
        HEALTH

    }
    [SerializeField]
    protected int FuelQuantity = 0;
    [SerializeField]
    protected int RepairsQuantity = 0;
    [SerializeField]
    protected int HealthQuantity = 0;

    public void UpdateResource(RESOURCE resource, int amount)
    {
        switch (resource)
        {
            case RESOURCE.FUEL:
                FuelQuantity += amount;
                if (FuelQuantity <= 0) FuelQuantity = 0;
                break;
            case RESOURCE.REPAIR:
                RepairsQuantity += amount;

                if (RepairsQuantity <= 0) RepairsQuantity = 0;
                break;
            case RESOURCE.HEALTH:
                HealthQuantity += amount;

                if (HealthQuantity <= 0) HealthQuantity = 0;
                break;
            default:
                break;
        }
    }
    public void TransferAllItems(Inventory other)
    {
        other.FuelQuantity += FuelQuantity;
        other.RepairsQuantity += RepairsQuantity;
        other.HealthQuantity += HealthQuantity;
    }


}
