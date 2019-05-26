using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiningDroneAI : MonoBehaviour
{
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
    private SphereCollider MiningTrigger = null;
    private int CurrentTargetIndex = 0;
    [SerializeField]
    private float MaxDistance = 1000.0f;

    private void Awake()
    {
        if (Resources == null)
        {
            Resources = FindObjectsOfType<MiningResource>();
        }
        NavAgent = GetComponentInChildren<NavMeshAgent>();
        MiningTrigger = GetComponent<SphereCollider>();
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
        Vector3 bodyTranslation = NavAgent.transform.position;
        bodyTranslation.y = Body.transform.position.y;
        if (CurrentTarget != null)
        {
            Vector3 dirToDestination = CurrentTarget.transform.position - Body.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(Body.transform.position, dirToDestination, out hit, MaxDistance))
            {
                if (hit.collider.gameObject.GetComponent<MiningResource>() != null)
                {
                    Body.transform.LookAt(CurrentTarget.transform.position);
                }
                else
                {
                    Body.transform.rotation = Quaternion.LookRotation(NavAgent.velocity, Vector3.up);
                }
            }
        }
        bodyTranslation.y += Body.transform.forward.y * NavAgent.speed * Time.deltaTime;
        bodyTranslation.y = Mathf.Max(bodyTranslation.y, NavAgent.transform.position.y + NavAgent.height);
        Body.transform.position = bodyTranslation;
        MiningTrigger.center = Body.transform.localPosition;

        DebugDraw();
    }

    private void OnTriggerEnter(Collider other)
    {
        MiningResource resource = other.GetComponent<MiningResource>();
        if (resource != null)
        {
            NavAgent.isStopped = true;
            StartMining(resource);
        }
    }

    void DebugDraw()
    {
        if (CurrentTarget != null)
        {
            Debug.DrawLine(transform.position, CurrentTarget.transform.position);
            Debug.DrawRay(Body.transform.position, Body.transform.forward, Color.red);
        }
        print(State);
    }

    void StartMining(MiningResource resource)
    {
        StartCoroutine("Mine", resource);
    }

    private IEnumerator Mine(MiningResource resource)
    {
        State = DroneState.MINING;
        CurrentTarget = null;
        yield return new WaitForSeconds(TimeToMine);
        State = DroneState.MOVING;
        CurrentTargetIndex = ++CurrentTargetIndex % Resources.Length;
        CurrentTarget = Resources[CurrentTargetIndex];
        NavAgent.SetDestination(CurrentTarget.transform.position);
        NavAgent.isStopped = false;
    }
}
