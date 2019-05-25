using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LerpVector3
{
    [ReadOnly]
    [SerializeField]
    private Vector3 desiredDirec;

    [ReadOnly]
    [SerializeField]
    private Vector3 realDirec;

    public float redirectSpeed;

    public static implicit operator Vector3(LerpVector3 direc)
    {
        return direc.realDirec;
    }

    public void SetDesired(Vector3 direc)
    {
        desiredDirec = direc;
    }

    public void Update()
    {
        Vector3.MoveTowards(realDirec, desiredDirec, redirectSpeed);
    }
}


[RequireComponent(typeof(Rigidbody))]
public class GroundMovementMotor : MonoBehaviour
{
    private Rigidbody rigBod;

    public LerpVector3 direc;

    private void Awake()
    {
        rigBod = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direc.Update();

        rigBod.AddForce(direc);
    }
}
