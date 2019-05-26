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

    public float groundAlignmentSlop = 0.05f;

    private void Awake()
    {
        activeStates = new HashSet<int>();
        activeStates.Add(groundedState.GetInstanceID());

        rigBod = GetComponent<Rigidbody>();
        physCollider = GetComponent<CapsuleCollider>();
        sphercastRay = new Ray(physCollider.bounds.center, Vector3.down);

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

    private bool CheckIsGrounded(out RaycastHit hitInfo)
    {
        sphercastRay.origin = physCollider.bounds.center;
        return Physics.SphereCast(sphercastRay, physCollider.radius, out hitInfo, physCollider.height / 2.0f);
        //return Physics.SphereCast(physCollider.bounds.center, physCollider.bounds.extents, physCollider.bounds.)

        return Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.05f);
    }

    private void UpdateMovement_Fixed()
    {
        // HANDLE STATE TRANSITIONS

        RaycastHit floorHitInfo;
        bool bIsInAir = !CheckIsGrounded(out floorHitInfo);



        // TODO: this ain't gonna work, we need propery slope detection and slope following
        // probably want to do here, make generic so we don't rely on specific states to implement their own logic



        // account for slopes causing negligable perturbations in height
        /*float distanceFormCollisionBottom = (physCollider.height / 4.0f) - floorHitInfo.distance;
        if (distanceFormCollisionBottom >= groundAlignmentSlop)
        {
            Vector3 adjustedPos = rigBod.position;
            adjustedPos.y -= distanceFormCollisionBottom;
            rigBod.MovePosition(adjustedPos);

            bIsInAir = false;
        }*/



        Vector3 movementDelta = gameObject.transform.rotation * direc;


        // figure out if we should be jumping or not
        if (bIsInAir)
        {
            //not jumping
            if (!activeStates.Contains(jumpState.GetInstanceID()))
            {
                activeStates.Add(fallState.GetInstanceID());
            }

            //rigBod.useGravity = false;
        }
        else
        {
            activeStates.Remove(fallState.GetInstanceID());

            //rigBod.useGravity = true;

            Quaternion floorSlopeDeltaRot = Quaternion.FromToRotation(Vector3.up, floorHitInfo.normal);

            movementDelta = floorSlopeDeltaRot * movementDelta;
        }


        // UPDATE STATES


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

        // we already commited to ground movement, adjust for slopes
        if (!bIsInAir)
        {
            //rigBod.useGravity = true;

            /*float distanceFormCollisionBottom = (physCollider.height / 4.0f) - floorHitInfo.distance;
            if (distanceFormCollisionBottom >= groundAlignmentSlop)
            {
                Vector3 adjustedPos = rigBod.position;
                adjustedPos.y -= distanceFormCollisionBottom;
                rigBod.MovePosition(adjustedPos);

                bIsInAir = false;
            }*/
        }

        ProcessState(jumpState);
        ProcessState(fallState);


        

        rigBod.velocity = movementDelta;
    }
}
