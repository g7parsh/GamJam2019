using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterJumpState", menuName = "Icarus/Character States/Jump")]
public class JumpCharacterMovementState : BaseCharacterMovementState
{
    public AnimationCurve jumpArc;

    public override EStateContext CalculateMovement(ref Vector3 input)
    {
        float timeInState = GetTimeInState_Seconds();

        float oldPos = jumpArc.Evaluate(Mathf.Max(timeInState - Time.fixedDeltaTime, 0.0f));
        float newPos = jumpArc.Evaluate(timeInState);

        input.y += (newPos - oldPos);

        EStateContext retContext = timeInState >= jumpArc.keys[jumpArc.length - 1].time ? EStateContext.Complete : EStateContext.Running;

        return retContext;

        //return new StateResult(input, retContext);
    }
}
