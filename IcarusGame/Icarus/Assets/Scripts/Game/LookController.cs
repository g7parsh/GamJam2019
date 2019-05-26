using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookController : MonoBehaviour
{
    public GameObject cameraGameObject;

    public float lookSensitivity = 1.0f;

    //private Plane neckBreakTestPlane;

    private void Awake()
    {
        //neckBreakTestPlane = new Plane(Vector3.forward, cameraGameObject.transform.position);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCamLookHorizontal(float delta)
    {
        Quaternion lookDelta = Quaternion.AngleAxis(delta * lookSensitivity, Vector3.up);
        gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, gameObject.transform.rotation * lookDelta, 360.0f);
    }

    public void AddCamLookVertical(float delta)
    {
        Quaternion currentRot = gameObject.transform.rotation;

        Quaternion lookDelta = Quaternion.AngleAxis(delta * lookSensitivity, Vector3.right);
        Quaternion newRot = Quaternion.RotateTowards(gameObject.transform.rotation, gameObject.transform.rotation * lookDelta, 360.0f);

        Plane plane = new Plane(currentRot * Vector3.forward, Vector3.zero);
        Vector3 testVec = cameraGameObject.transform.rotation * Vector3.forward;

        if (!plane.GetSide(testVec))
        {
            /*testVec = Vector3.ProjectOnPlane(testVec, plane.normal).normalized;
            Quaternion.*/
        }
        else
        {

        }
    }


    public Quaternion GetLookDirection()
    {
        return gameObject.transform.rotation;
    }
}
