using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterGroundedState", menuName = "Icarus/Character States/Grouned")]
public class GroundedCharacterState : BaseCharacterState
{
    public float maxSpeed = 5.0f;

    public override EStateContext CalculateMovement(MovementContext movementContext)
    {
        Vector3 newVel = movementContext.input * maxSpeed;
        newVel -= movementContext.rigBod.velocity;
        newVel.y = 0.0f;

        movementContext.rigBod.AddForce(newVel, ForceMode.VelocityChange);

        return EStateContext.Running;
    }
}
