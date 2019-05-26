using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipCam : MonoBehaviour
{
  public Transform camTarget;
  public float followDist;

  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
    Rigidbody pRigid = camTarget.GetComponent<Rigidbody>();
    if (pRigid)
    {
      transform.position = new Vector3(camTarget.position.x + pRigid.velocity.x/3, camTarget.position.y + pRigid.velocity.y/3, -followDist);
    }
  }
}
