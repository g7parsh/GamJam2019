using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningDroneAI : MonoBehaviour
{
    private static MiningResource[] Resources = null;

    private MiningResource CurrentTarget = null;
    private int CurrentTargetIndex;

    [System.Serializable]
    public struct AIParameters
    {
        public float MovementSpeed;
        public float ApproachRadius;       // how close they can get to a target object
    }
    [SerializeField]
    private AIParameters Params;
    private void Awake()
    {
        if (Resources == null)
        {
            Resources = FindObjectsOfType<MiningResource>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Resources != null)
        {
            CurrentTargetIndex = 0;
            CurrentTarget = Resources[CurrentTargetIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(CurrentTarget.transform);

        Vector3 newPosition = transform.position + transform.forward * Time.deltaTime * Params.MovementSpeed;

        if (Vector3.Distance(newPosition, CurrentTarget.transform.position) <= Params.ApproachRadius)
        {
            CurrentTargetIndex = ++CurrentTargetIndex % Resources.Length;
            CurrentTarget = Resources[CurrentTargetIndex];
            transform.LookAt(CurrentTarget.transform);
            newPosition = transform.position + transform.forward * Time.deltaTime * Params.MovementSpeed;
        }
        transform.position = newPosition;
    }
}
