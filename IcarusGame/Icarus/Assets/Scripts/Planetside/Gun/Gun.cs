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
    [SerializeField]
    private Transform MuzzleTransform = null;
    [SerializeField]
    private Animator ArmAnimator = null;
    private BulletTrails BulletTrail = null;
    private Inventory PlayerInventory = null;

    private bool IsToolMode = false;
    private bool IsToolFiring = false;
    // Start is called before the first frame update
    private void Awake()
    {
        BulletTrail = GetComponent<BulletTrails>();
        PlayerInventory = GetComponentInParent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 200, 150), "IsToolFiring: " + IsToolFiring);
    }

    private void FixedUpdate()
    {
        if (IsToolMode && IsToolFiring)
        {
            RaycastHit hit;
            if (HitSomething(out hit))
            {
                GameObject target = hit.collider.gameObject;
                MiningResource miningResource = target.GetComponent<MiningResource>();
                if (miningResource != null)
                {
                    IsToolFiring = true;
                    return;
                }
            }
        }

        IsToolFiring = false;
    }

    void ProcessInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (IsToolMode)
            {
                IsToolFiring = true;
                ArmAnimator.SetBool("IsToolOn", IsToolFiring);
                RaycastHit hit;
                if (HitSomething(out hit))
                {
                    GameObject target = hit.collider.gameObject;
                    MiningResource miningResource = target.GetComponent<MiningResource>();
                    if (miningResource != null)
                    {
                        Inventory.RESOURCE resource = miningResource.resourceType;
                        StartCoroutine("Harvest", resource);
                    }
                }
            }
            else
            {
                if (CanShoot)
                {
                    // shoot the gun
                    Shoot();
                    StartCoroutine("Cooldown");
                }
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (IsToolMode)
            {
                IsToolFiring = false;
                ArmAnimator.SetBool("IsToolOn", IsToolFiring);
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            SwitchTools();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        Vector3 endpoint = Vector3.zero;
        if (HitSomething(out hit))
        {
            if (DecalManager != null && hit.collider.gameObject.isStatic)
            {
                SpawnDebris(hit);
                DecalManager.OnShotHit(hit);
            }
            endpoint = MuzzleTransform.position + (hit.point - MuzzleTransform.position).normalized * Mathf.Min(hit.distance, MaxDistance);
        }
        if (endpoint == Vector3.zero)
        {
            endpoint = MuzzleTransform.position + GunCamera.transform.forward * MaxDistance;
        }
        BulletTrail.OnShot(endpoint);
        ArmAnimator.SetTrigger("LethalFire");
    }

    private void SpawnDebris(RaycastHit hit)
    {
        if (Debris != null)
        {
            ParticleSystem newDebris = Instantiate<ParticleSystem>(Debris, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(newDebris.gameObject, DebrisLifetime);
        }
    }

    private void SwitchTools()
    {
        IsToolMode = !IsToolMode;
        ArmAnimator.SetBool("IsToolMode", IsToolMode);
    }

    private bool HitSomething(out RaycastHit hit)
    {
        return Physics.Raycast(GunCamera.transform.position, GunCamera.transform.forward, out hit, Mathf.Infinity);
    }

    private IEnumerator Cooldown()
    {
        CanShoot = false;
        yield return new WaitForSeconds(ShootingInterval);
        CanShoot = true;
    }
    
    private IEnumerator Harvest(Inventory.RESOURCE resource)
    {
        float timer = 0.0f;
        while (IsToolMode && IsToolFiring)
        {
            timer += Time.deltaTime;
            if (timer >= 1.0f)     // harvest every second
            { 
                PlayerInventory.UpdateResource(resource, 1);
                timer -= 1.0f;
                print("HARVESTING: " + resource.ToString());
            }
            yield return null;
        }
    }
}
