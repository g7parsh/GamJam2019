using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject PlayerControllerToSpawn = null;
    private SphereCollider TriggerVolume;

    // Start is called before the first frame update
    void Start()
    {
        TriggerVolume = GetComponentInChildren<SphereCollider>();
        GameObject NewPlayer = Instantiate<GameObject>(PlayerControllerToSpawn, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
