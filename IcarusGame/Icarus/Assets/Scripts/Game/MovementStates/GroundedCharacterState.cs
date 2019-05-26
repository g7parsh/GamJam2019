using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterGroundedState", menuName = "Icarus/Character States/Grouned")]
public class GroundedCharacterState : BaseCharacterState
{
    public float maxSpeed = 5.0f;

    public override EStateContext CalculateMovement(ref Vector3 input, float deltaTime)
    {
        float deltaPos = maxSpeed;

        input *= deltaPos;

        return EStateContext.Running;
    }
}
