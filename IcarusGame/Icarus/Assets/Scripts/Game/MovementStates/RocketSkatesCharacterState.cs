using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterRocketSkatesState", menuName = "Icarus/Character States/Rocket Skates")]
public class RocketSkatesCharacterState : BaseCharacterState
{
    public override EStateContext CalculateMovement(MovementContext movementContext, StateWorldContext worldContext)
    {


        return EStateContext.Running;
    }
}
