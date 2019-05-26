using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipComponent : MonoBehaviour
{
  public float fwdAccel;
  public float strafeAccel;
  public float fwdDampen;
  public float strafeDampen;

  void Update()
  {
    Camera pCam = FindObjectsOfType<Camera>()[0];
    if (!pCam)
      return;

    Rigidbody pRigid = GetComponent<Rigidbody>();
    if (!pRigid)
      return;

    //enforce 2d movement
    transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);

    //orientation
    float x = Input.GetAxis("Mouse X");
    float y = Input.GetAxis("Mouse Y");

    Ray mouseWorldRay = pCam.ScreenPointToRay(Input.mousePosition);
    Plane camPlane = new Plane(pCam.transform.forward, 0);
    float rayDist;
    camPlane.Raycast(mouseWorldRay, out rayDist);

    Vector3 lookAt = mouseWorldRay.GetPoint(rayDist);
    //lookAt = lookAt - transform.position; //localize to transform

    //Quaternion rot = Quaternion.AngleAxis(Vector3.Dot(lookAt, Vector3.forward), Vector3.right);
    Debug.DrawLine(transform.position, lookAt,Color.red);

    //transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 360.0f);


    transform.LookAt(lookAt, pCam.transform.up);
    //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Mathf.Abs(transform.rotation.eulerAngles.y), transform.rotation.eulerAngles.z);
    //transform.right = -pCam.transform.forward;  //enforce ship orientation doesn't flip when passing mouse through the middle of the screen

    //velocity
    float xIn = Input.GetAxis("HorizontalSpaceshipMovement");
    float yIn = Input.GetAxis("VerticalSpaceshipMovement");
    yIn = Mathf.Clamp01(yIn);

    //dampen velocity
    pRigid.velocity = new Vector3(pRigid.velocity.x * (1 - strafeDampen), pRigid.velocity.y * (1 - fwdDampen), 0.0f);

    //add input accel
    Vector3 newVel = new Vector3(xIn * strafeAccel, yIn * fwdAccel, 0.0f);
    Vector3 worldVel = transform.up * newVel.x + transform.forward * newVel.y;
    pRigid.velocity += worldVel;
  }
}
