using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject PlayerControllerToSpawn = null;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate<GameObject>(PlayerControllerToSpawn, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
