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
        Instantiate<GameObject>(PlayerControllerToSpawn, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
