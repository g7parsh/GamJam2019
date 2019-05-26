using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterJumpState", menuName = "Icarus/Character States/Fall")]
public class FallingCharacterState : BaseCharacterState
{
    public float fallSpeed = 9.8f;
    public float terminalVelocity = 5.0f;

    public override EStateContext CalculateMovement(ref Vector3 input, float deltaTime)
    {
        input.y = Mathf.MoveTowards(input.y, -1.0f * terminalVelocity, fallSpeed);


        //float fallSpeedDelta = fallAccel * deltaTime;

        //input.y -= fallSpeedDelta;
        //input.y = Mathf.Clamp(input.y, -1.0f * terminalVelocity, Mathf.Infinity);

        return EStateContext.Running;
    }
}
