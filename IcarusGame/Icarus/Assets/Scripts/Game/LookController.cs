using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookController : MonoBehaviour
{
    public GameObject cameraGameObject;

    public float lookSensitivity = 1.0f;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCamLookHorizontal(float delta)
    {
        //Quaternion deltaRot = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        //Quaternion.AngleAxis()

        Quaternion lookDelta = Quaternion.AngleAxis(delta * lookSensitivity, Vector3.up);
        gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, gameObject.transform.rotation * lookDelta, 360.0f);
    }

    public void AddCamLookVertical(float delta)
    {
        //Quaternion lookDelta = Quaternion.AngleAxis(delta * lookSensitivity, Vector3.up);
        //gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, gameObject.transform.rotation * lookDelta, 360.0f);
    }


    public Quaternion GetLookDirection()
    {
        return gameObject.transform.rotation;
    }
}
