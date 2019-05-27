using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterJumpState", menuName = "Icarus/Character States/Fall")]
public class FallingCharacterState : BaseCharacterState
{
    public float fallSpeed = 9.8f;
    public float terminalVelocity = 5.0f;

    public override bool StateIsValid(StateWorldContext worldContext)
    {
        return worldContext.bIsInAir;
    }

    public override EStateContext CalculateMovement(MovementContext movementContext)
    {
        //velocity.y = -1.0f * terminalVelocity;

        return EStateContext.Running;
    }
}
