using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterJumpState", menuName = "Icarus/Character States/Fall")]
public class FallingCharacterState : BaseCharacterState
{
    public float fallSpeed = 9.8f;
    public float terminalVelocity = 5.0f;

    public override EStateContext CalculateMovement(ref Vector3 velocity, float deltaTime)
    {
        velocity.y = -1.0f * terminalVelocity;

        return EStateContext.Running;
    }
}
