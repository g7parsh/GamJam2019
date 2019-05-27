using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StateWorldContext
{
    public bool bIsInAir;
    public bool bBoostRequested;
}

public struct MovementContext
{
    public Rigidbody rigBod;
    public Vector3 input;
    public float deltaTime;
}

//[CreateAssetMenu(fileName ="Character Movement Config", menuName = "Icarus/Character Movement Config")]
public abstract class BaseCharacterState : ScriptableObject
{
    public enum EStateContext
    {
        Running,
        Complete
    }

    public struct StateResult
    {
        public StateResult(Vector3 result, EStateContext context)
        {
            this.result = result;
            this.context = context;

        }

        public EStateContext context;
        public Vector3 result;
    }

    System.DateTime startTimeStamp;

    public System.TimeSpan GetTimeInState()
    {
        return System.DateTime.Now - startTimeStamp;
    }

    public float GetTimeInState_Seconds()
    {
        return  (float) System.Math.Max(GetTimeInState().TotalSeconds, 0.0);
    }

    public void StartState()
    {
        ResetState();
        startTimeStamp = System.DateTime.Now;
    }

    public virtual void ResetState()
    { }

    public virtual bool StateIsValid(StateWorldContext worldContext)
    {
        return true;
    }

    public virtual EStateContext CalculateMovement(MovementContext movementContext, StateWorldContext worldContext)
    {
        return EStateContext.Complete;
    }
}
