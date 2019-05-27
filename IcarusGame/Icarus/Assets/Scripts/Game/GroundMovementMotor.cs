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
[RequireComponent(typeof(CapsuleCollider))]
public class GroundMovementMotor : MonoBehaviour
{
    private Rigidbody rigBod;
    private CapsuleCollider physCollider;
    private Ray sphercastRay;

    public LerpVector3 direc;
    

    [Header("Character Movement States")]
    public BaseCharacterState groundedState;
    public BaseCharacterState fallState;
    public BaseCharacterState jumpState;

    private HashSet<int> activeStates;

    private StateWorldContext currentContext;

    //public float groundAlignmentSlop = 0.05f;

    private void Awake()
    {
        activeStates = new HashSet<int>();
        activeStates.Add(groundedState.GetInstanceID());
        groundedState.StartState();

        rigBod = GetComponent<Rigidbody>();
        physCollider = GetComponent<CapsuleCollider>();
        sphercastRay = new Ray(physCollider.bounds.center, Vector3.down);
    }

    void Update()
    {
        direc.Update();

        UpdateStateTransitions();
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
        if (activeStates.Contains(fallState.GetInstanceID())
            || activeStates.Contains(jumpState.GetInstanceID())
            || currentContext.bIsInAir)
        {
            return;
        }

        activeStates.Remove(fallState.GetInstanceID());
        activeStates.Add(jumpState.GetInstanceID());

        jumpState.StartState();
    }

    public void SetInputJumpEnd(float test)
    {
    }

    public void SetInputPushSkates(float val)
    {
        currentContext.bBoostRequested = true;

        /*System.DateTime currentTimeStamp = System.DateTime.Now;

        if (currentContext.bIsInAir
            || ((float)(currentTimeStamp - lastSprintBoostTime).Seconds) < sprintBoostCooldownTime)
        {
            return;
        }

        currentSprintBoostMod = maxSprintBoostMod;
        lastSprintBoostTime = currentTimeStamp;*/
    }

    private bool CheckIsGrounded(out RaycastHit hitInfo)
    {
        sphercastRay.origin = physCollider.bounds.center;
        return Physics.SphereCast(sphercastRay, physCollider.radius, out hitInfo, physCollider.height / 2.0f);
    }

    private void UpdateStateTransitions()
    {
        System.Func<BaseCharacterState, StateWorldContext, bool> EvaluateStateValidity = (state, context) =>
        {
            if (activeStates.Contains(state.GetInstanceID()))
            {
                if (!state.StateIsValid(context))
                {
                    activeStates.Remove(state.GetInstanceID());

                    return false;
                }
                return true;
            }
            return false;
        };


        RaycastHit floorHitInfo;
        currentContext.bIsInAir = !CheckIsGrounded(out floorHitInfo);

        Vector3 movementDelta = gameObject.transform.rotation * direc;


        EvaluateStateValidity(groundedState, currentContext);
        EvaluateStateValidity(jumpState, currentContext);
    }

    private void UpdateMovement_Fixed()
    {
        // UPDATE STATES

        MovementContext newMovementContext;
        newMovementContext.rigBod = rigBod;
        newMovementContext.input = (gameObject.transform.rotation * direc);
        newMovementContext.deltaTime = Time.fixedDeltaTime;

        System.Action<BaseCharacterState> ProcessState = state =>
        {
            if (activeStates.Contains(state.GetInstanceID()))
            {
                BaseCharacterState.EStateContext stateContext = state.CalculateMovement(newMovementContext, currentContext);
            }
        };

        ProcessState(jumpState);
        ProcessState(groundedState);
        //ProcessState(fallState);

        currentContext.bBoostRequested = false;
    }
}
