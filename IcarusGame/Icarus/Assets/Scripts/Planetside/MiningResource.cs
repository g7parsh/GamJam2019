using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningResource : MonoBehaviour
{
    public int Quantity = 50;
    public Inventory.RESOURCE resourceType;
    public MaterialPropertyBlock block;
    private Renderer renderer;

    void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()

    {
        block = new MaterialPropertyBlock();
        renderer = GetComponentInChildren<Renderer>();
        renderer.GetPropertyBlock(block);
        switch (resourceType)
        {

            case Inventory.RESOURCE.FUEL:
                block.SetColor("_Color", Color.red);
                renderer.SetPropertyBlock(block);
                break;
            case Inventory.RESOURCE.HEALTH:
                block.SetColor("_Color", Color.green);
                renderer.SetPropertyBlock(block);
                break;
            case Inventory.RESOURCE.REPAIR:
                block.SetColor("_Color", Color.grey);
                renderer.SetPropertyBlock(block);
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerInventory>()!= null) {
            PlayerInventory inventory = other.gameObject.GetComponent<PlayerInventory>();
            inventory.AjustResource(this, Quantity);
        }
    }
}
