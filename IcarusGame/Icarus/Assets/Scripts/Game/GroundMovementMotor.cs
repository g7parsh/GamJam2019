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

    [Header("Character Movement States")]
    public BaseCharacterState groundedState;
    public BaseCharacterState fallState;
    public BaseCharacterState jumpState;

    HashSet<int> activeStates;

    private void Awake()
    {
        activeStates = new HashSet<int>();
        activeStates.Add(groundedState.GetInstanceID());

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
        activeStates.Remove(fallState.GetInstanceID());
        activeStates.Add(jumpState.GetInstanceID());

        jumpState.StartState();
    }

    public void SetInputJumpEnd(float test)
    {
    }

    private bool CheckIsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.05f);
    }

    private void UpdateMovement_Fixed()
    {
        // HANDLE STATE TRANSITIONS

        // in the air
        if (!CheckIsGrounded())
        {
            //not jumping
            if (!activeStates.Contains(jumpState.GetInstanceID()))
            {
                activeStates.Add(fallState.GetInstanceID());
            }
        }
        else
        {
            activeStates.Remove(fallState.GetInstanceID());
        }


        // UPDATE STATES

        Vector3 movementDelta = gameObject.transform.rotation * direc;

        System.Action<BaseCharacterState> ProcessState = state =>
        {
            if (activeStates.Contains(state.GetInstanceID()))
            {
                BaseCharacterState.EStateContext stateContext = state.CalculateMovement(ref movementDelta, Time.fixedDeltaTime);
                if (stateContext == BaseCharacterState.EStateContext.Complete)
                {
                    activeStates.Remove(state.GetInstanceID());
                }
            }
        };

        ProcessState(groundedState);
        ProcessState(jumpState);
        ProcessState(fallState);

        rigBod.velocity = movementDelta;
    }
}
