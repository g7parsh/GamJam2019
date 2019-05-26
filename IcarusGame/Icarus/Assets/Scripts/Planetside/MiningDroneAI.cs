using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiningDroneAI : MonoBehaviour
{
    [System.Serializable]
    public struct AIParameters
    {
        public float MovementSpeed;
        public float ApproachRadius;       // how close they can get to a target object
    }

    public enum DroneState
    {
        STOPPED,
        MOVING,
        MINING
    };

    private static MiningResource[] Resources = null;

    [SerializeField]
    private static float TimeToMine = 3.0f;
    private DroneState State = DroneState.STOPPED;
    [SerializeField]
    private GameObject Body = null;
    private MiningResource CurrentTarget = null;
    private NavMeshAgent NavAgent = null;
    private int CurrentTargetIndex = 0;

    [SerializeField]
    private AIParameters Params;
    private void Awake()
    {
        if (Resources == null)
        {
            Resources = FindObjectsOfType<MiningResource>();
        }
        NavAgent = GetComponentInChildren<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Resources != null)
        {
            CurrentTargetIndex = 0;
            CurrentTarget = Resources[CurrentTargetIndex];
            NavAgent.SetDestination(CurrentTarget.transform.position);
            transform.LookAt(CurrentTarget.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DirectTowardResource();
        if (State != DroneState.MINING)
        {
            if (CurrentTarget == null)
            {
                State = DroneState.STOPPED;
            }
            else
            {
                State = DroneState.MOVING;
            }
        }
        Vector3 agentTranslation = NavAgent.transform.position;
        agentTranslation.y = Mathf.Max(Body.transform.position.y, NavAgent.transform.position.y);
        Body.transform.position = agentTranslation;
    }

    void DirectTowardResource()
    {
        if (CurrentTarget != null)
        {
            Debug.DrawLine(transform.position, CurrentTarget.transform.position);
            if (Vector3.Distance(transform.position, CurrentTarget.transform.position) <= Params.ApproachRadius)
            {
                StartCoroutine("Mine");
            }
        }
    }

    private IEnumerator Mine()
    {
        State = DroneState.MINING;
        CurrentTarget = null;
        yield return new WaitForSeconds(TimeToMine);
        State = DroneState.MOVING;
        CurrentTargetIndex = ++CurrentTargetIndex % Resources.Length;
        CurrentTarget = Resources[CurrentTargetIndex];
        NavAgent.SetDestination(CurrentTarget.transform.position);
    }
}
