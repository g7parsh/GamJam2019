using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
struct InputAxisMapping
{
    string axisName;
    UnityEvent inputEvent;
}


[RequireComponent(typeof(GroundMovementMotor))]
public class ActorController : MonoBehaviour
{
    List<InputAxisMapping> inputMappings;

    private GroundMovementMotor m_movementMotor;

    private void Awake()
    {
        m_movementMotor = GetComponent<GroundMovementMotor>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
