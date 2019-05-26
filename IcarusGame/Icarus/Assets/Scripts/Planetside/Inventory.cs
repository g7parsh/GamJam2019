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

    public void TransferAllItems(Inventory other)
    {
        other.FuelQuantity += FuelQuantity;
        other.RepairsQuantity += RepairsQuantity;
        other.HealthQuantity += HealthQuantity;
    }


}
