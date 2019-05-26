using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LerpVector3
{
    public enum EDirecAxis
    {
        Forward,
        Right,
        Up,
    }

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
        desiredDirec = direc.normalized;
    }

    public void SetDesired(float axisVal, EDirecAxis axis)
    {
        switch (axis)
        {

            case LerpVector3.EDirecAxis.Right:
                desiredDirec.x = Mathf.Clamp01(axisVal) * Mathf.Sign(axisVal);
                break;

            case LerpVector3.EDirecAxis.Up:
                desiredDirec.y = Mathf.Clamp01(axisVal) * Mathf.Sign(axisVal);
                break;

            case LerpVector3.EDirecAxis.Forward:
                desiredDirec.z = Mathf.Clamp01(axisVal) * Mathf.Sign(axisVal);
                break;

            default:
                Debug.Break();
                break;
        }
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

    public void SetDesiredInput(Vector3 inputDir)
    {
        direc.SetDesired(inputDir);
    }

    public void SetDesiredInput(float axisVal, LerpVector3.EDirecAxis axis)
    {
        direc.SetDesired(axisVal, axis);
    }

    public void SetDesiredInputForward(float axisVal) { SetDesiredInput(axisVal, LerpVector3.EDirecAxis.Forward); }
    public void SetDesiredInputRight(float axisVal) { SetDesiredInput(axisVal, LerpVector3.EDirecAxis.Right); }
}
