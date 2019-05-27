using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterGroundedState", menuName = "Icarus/Character States/Grouned")]
public class GroundedCharacterState : BaseCharacterState
{
    [ReadOnly]
    [SerializeField]
    private Vector3 movementInput;

    public float maxSpeed = 5.0f;
    public float airSpeedDecayRate = 10.0f;

    public float maxSprintBoostMod = 1.5f;
    public float sprintBoostDecayRate = 0.5f;
    public float sprintBoostCooldownTime = 1.0f;

    [SerializeField]
    private float currentSprintBoostMod;
    private System.DateTime lastSprintBoostTime;


    public float increasedAirGravityAccel = 20.0f;

    public override void ResetState()
    {
        lastSprintBoostTime = System.DateTime.Now;
    }

    public override EStateContext CalculateMovement(MovementContext movementContext, StateWorldContext worldContext)
    {
        System.DateTime currentTimeStamp = System.DateTime.Now;

        if (worldContext.bBoostRequested
            && !worldContext.bIsInAir
            && ((float)(currentTimeStamp - lastSprintBoostTime).Seconds) > sprintBoostCooldownTime)
        {
            currentSprintBoostMod = maxSprintBoostMod;
            lastSprintBoostTime = currentTimeStamp;
        }

        if (worldContext.bIsInAir)
        {
            if (movementInput.sqrMagnitude > (maxSpeed * maxSpeed))
            {
                movementInput = Vector3.MoveTowards(movementInput, movementInput.normalized * maxSpeed, airSpeedDecayRate * movementContext.deltaTime);
            }
        }
        else
        {
            currentSprintBoostMod = Mathf.MoveTowards(currentSprintBoostMod, 1.0f, sprintBoostDecayRate * movementContext.deltaTime);

            movementInput = movementContext.input * maxSpeed * currentSprintBoostMod;
        }

        //Vector3 newVel = movementContext.input * maxSpeed * currentSprintBoostMod;
        Vector3 newVel = movementInput;
        newVel -= movementContext.rigBod.velocity;
        newVel.y = 0.0f;

        movementContext.rigBod.AddForce(newVel, ForceMode.VelocityChange);

        if (worldContext.bIsInAir)
        {
            movementContext.rigBod.AddForce(Vector3.down * increasedAirGravityAccel, ForceMode.Acceleration);
        }


        return EStateContext.Running;
    }
}
