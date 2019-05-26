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

    public enum ESendChannel
    {
        Continuous,
        ButtonDown,
        ButtonUp,
        ButtonHeld
    }

    public string axisName;
    public ESendChannel sendChannel;
    public UnityEvent_Float inputEvent;

    public void HandleInovkes()
    {
        switch (sendChannel)
        {
            case ESendChannel.Continuous:
                inputEvent.Invoke(Input.GetAxisRaw(axisName));
                break;

            case ESendChannel.ButtonDown:
                if (Input.GetButtonDown(axisName))
                {
                    inputEvent.Invoke(Input.GetAxisRaw(axisName));
                }
                break;

            case ESendChannel.ButtonUp:
                if (Input.GetButtonUp(axisName))
                {
                    inputEvent.Invoke(Input.GetAxisRaw(axisName));
                }
                break;
            case ESendChannel.ButtonHeld:
                if (Input.GetButton(axisName))
                {
                    inputEvent.Invoke(Input.GetAxisRaw(axisName));
                }
                break;
        }
    }
}

[System.Serializable]
public struct InputManager
{
    public List<InputAxisMapping> inputMappings;

    public void Update()
    {
        foreach (InputAxisMapping axisMapping in inputMappings)
        {
            axisMapping.HandleInovkes();
            //axisMapping.inputEvent.Invoke(Input.GetAxisRaw(axisMapping.axisName));
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
    }
}
