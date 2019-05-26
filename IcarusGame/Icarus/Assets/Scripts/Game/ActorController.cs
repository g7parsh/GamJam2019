using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEvent_Float : UnityEvent<float> { }

[System.Serializable]
public struct InputAxisMapping
{
    /*[System.NonSerialized]
    public bool bDirty;*/

    public string axisName;
    public UnityEvent_Float inputEvent;
}

[System.Serializable]
public struct InputManager
{
    public List<InputAxisMapping> inputMappings;

    public void Update()
    {
        foreach (InputAxisMapping axisMapping in inputMappings)
        {
            axisMapping.inputEvent.Invoke(Input.GetAxisRaw(axisMapping.axisName));

            /*if (axisMapping.bDirty)
            {
                axisMapping.inputEvent.Invoke();
            }*/
        }
    }
}


[RequireComponent(typeof(GroundMovementMotor))]
public class ActorController : MonoBehaviour
{
    private GroundMovementMotor m_movementMotor;

    //[DisplayName("Input Manager")]
    public InputManager m_inputManager;

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
        m_inputManager.Update();

        //m_movementMotor.
    }
}
