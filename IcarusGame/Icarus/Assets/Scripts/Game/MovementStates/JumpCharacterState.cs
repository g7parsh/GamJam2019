using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterJumpState", menuName = "Icarus/Character States/Jump")]
public class JumpCharacterState : BaseCharacterState
{
    public AnimationCurve jumpArc;
    public float jumpHeightScale = 20.0f;

    /*private float maxHeightTime = 0.0f;
    private const float scanningTimeStep = 0.05f;

    private void Awake()
    {
        float prevBestHeight = jumpArc.Evaluate(maxHeightTime);

        for (float time = 0; time <= 1.0f; time += scanningTimeStep)
        {
            float candidateHeight = jumpArc.Evaluate(time)
            if (candidateHeight > prevBestHeight)
            {
                maxHeightTime = time;
                prevBestHeight = candidateHeight;
            }
        }
    }*/

    public override EStateContext CalculateMovement(ref Vector3 velocity, float deltaTime)
    {
        float timeInState = GetTimeInState_Seconds();

        float oldPos = jumpArc.Evaluate(Mathf.Max(timeInState - deltaTime, 0.0f));
        float newPos = jumpArc.Evaluate(Mathf.Max(timeInState, 0.0f));

        // divide by deltaTime to convert to an acceleartion.
            // This lets us drive the position by writing to the RigidBody's velocity
        float deltaHeight = ((newPos - oldPos) * jumpHeightScale) / deltaTime;

        velocity.y += deltaHeight;

        EStateContext retContext = timeInState >= jumpArc.keys[jumpArc.length - 1].time ? EStateContext.Complete : EStateContext.Running;


        return retContext;
    }
}
