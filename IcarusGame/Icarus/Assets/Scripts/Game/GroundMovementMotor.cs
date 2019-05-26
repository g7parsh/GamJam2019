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

    public static Vector3 operator* (LerpVector3 lerpVec3, float multiplier)
    {
        return lerpVec3.realDirec * multiplier;
    }

    public void SetDesired(Vector3 direc)
    {
        //desiredDirec = direc.normalized;
        desiredDirec = direc;
    }

    public void SetDesired(float axisVal, EDirecAxis axis)
    {
        switch (axis)
        {
            case LerpVector3.EDirecAxis.Right:
                desiredDirec.x = Mathf.Clamp(axisVal, -1.0f, 1.0f);
                break;

            case LerpVector3.EDirecAxis.Up:
                desiredDirec.y = Mathf.Clamp(axisVal, -1.0f, 1.0f);
                break;

            case LerpVector3.EDirecAxis.Forward:
                desiredDirec.z = Mathf.Clamp(axisVal, -1.0f, 1.0f);
                break;

            default:
                Debug.Break();
                break;
        }
    }

public void Update()
    {

        realDirec = Vector3.MoveTowards(realDirec, desiredDirec, redirectSpeed);
    }
}



[RequireComponent(typeof(Rigidbody))]
public class GroundMovementMotor : MonoBehaviour
{
    private Rigidbody rigBod;

    public LerpVector3 direc;

    public BaseCharacterMovementState jumpState;
    bool bIsRunningJumpState = false;

    public float maxSpeed = 20;
    public float gravityVelocity = 1.0f;

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
    }

    private void FixedUpdate()
    {
        UpdateMovement_Fixed();
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

    public void SetInputJumpBegin(float test)
    {
        //Debug.Log("JUMP BEGIN");
        bIsRunningJumpState = true;
        jumpState.StartState();
    }

    public void SetInputJumpEnd(float test)
    {
        Debug.Log("JUMP END");
    }

    private void UpdateMovement_Fixed()
    {
        Vector3 movementDelta = gameObject.transform.rotation * (direc * maxSpeed);

        if (bIsRunningJumpState)
        {
            BaseCharacterMovementState.EStateContext stateContext = jumpState.CalculateMovement(ref movementDelta);
            
            if (stateContext == BaseCharacterMovementState.EStateContext.Complete)
            {
                bIsRunningJumpState = false;
            }
        }
        else
        {
            movementDelta.y = -1.0f * gravityVelocity;
        }

        rigBod.velocity = movementDelta;
    }
}
