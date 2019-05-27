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
    [SerializeField]
    private DecalQueue DecalManager = null;
    [SerializeField]
    private Camera GunCamera = null;
    [SerializeField]
    private float MaxDistance = 1000.0f;
    private BulletTrails BulletTrail;
    // Start is called before the first frame update
    private void Awake()
    {
        BulletTrail = GetComponent<BulletTrails>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    void Shoot()
    {
        RaycastHit hit;
        Vector3 endpoint = Vector3.zero;
        if (Physics.Raycast(GunCamera.transform.position, GunCamera.transform.forward, out hit, Mathf.Infinity))
        {
            if (DecalManager != null && hit.collider.gameObject.isStatic)
            {
                SpawnDebris(hit);
                DecalManager.OnShotHit(hit);
            }
            endpoint = transform.position + (hit.point - transform.position).normalized * Mathf.Min(hit.distance, MaxDistance);
        }
        if (endpoint == Vector3.zero)
        {
            endpoint = transform.position + transform.forward * MaxDistance;
        }
        BulletTrail.OnShot(endpoint);
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
