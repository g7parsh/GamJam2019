using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private float ShootingInterval = 0.1f;
    private bool CanShoot = true;
    [SerializeField]
    private ParticleSystem Debris = null;
    [SerializeField]
    private float DebrisLifetime = 1.0f;

    private DecalQueue DecalManager;
    // Start is called before the first frame update
    private void Awake()
    {
        DecalManager = GetComponentInChildren<DecalQueue>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (DecalManager != null && hit.collider.gameObject.isStatic)
            {
                SpawnDebris(hit);
                DecalManager.OnShotHit(hit);
            }
        }
    }

    void ProcessInput()
    {
        if (Input.GetButtonDown("Fire1") && CanShoot)
        {
            // shoot the gun
            Shoot();
            StartCoroutine("Cooldown");
        }
    }

    private void SpawnDebris(RaycastHit hit)
    {
        if (Debris != null)
        {
            ParticleSystem newDebris = Instantiate<ParticleSystem>(Debris, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(newDebris.gameObject, DebrisLifetime);
        }
    }

    private IEnumerator Cooldown()
    {
        CanShoot = false;
        yield return new WaitForSeconds(ShootingInterval);
        CanShoot = true;
    }
}
