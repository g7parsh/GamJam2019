using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookController : MonoBehaviour
{
    public GameObject cameraGameObject;

    public float lookSensitivity = 1.0f;

    private void Awake()
    {
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
        Quaternion currentRot = cameraGameObject.transform.localRotation;

        Quaternion deltaRot = Quaternion.AngleAxis(-1.0f * delta * lookSensitivity, Vector3.right);

        Quaternion candidateRot = currentRot * deltaRot;

        if (Quaternion.Dot(candidateRot, Quaternion.LookRotation(Vector3.forward, Vector3.up)) < 0.0f)
        {
            Vector3 adjustedVec = Vector3.ProjectOnPlane(candidateRot * Vector3.forward, Vector3.forward).normalized;

            candidateRot = Quaternion.LookRotation(adjustedVec, Vector3.up);
        }

        cameraGameObject.transform.localRotation = candidateRot;
    }


    public Quaternion GetLookDirection()
    {
        return gameObject.transform.rotation;
    }
}
