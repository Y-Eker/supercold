using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform firePos;
    [SerializeField] Transform throwableTransform;
    [SerializeField] Transform gunContainerTransform;
    [SerializeField] Rigidbody throwableRb;
    [SerializeField] MeshCollider throwableMeshCollider;
    [SerializeField] Camera mainCam;
    [Header("Throw")]
    [SerializeField] float throwForceForwards, throwForceUpwards;
    
    [Header("Equip")]
    public bool isEquipped;
    public static bool slotFull;
    [SerializeField] Vector3 weaponOffset;
    [SerializeField] Vector3 weaponRotation;
    [SerializeField] float equipDistance;

    private GunScript throwableGunScript;
    private float distance;
    private bool isGun;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        isGun = gameObject.tag == "Gun";
        if (isGun) throwableGunScript = GetComponent<GunScript>();
        if (transform.parent != null)
        {
            if (transform.parent.tag == "GunContainer")
            {
                isEquipped = true;
                slotFull = true;
            }
        }
        if (!isEquipped)
        {
            if (isGun) throwableGunScript.enabled = false;
            throwableRb.isKinematic = false;
            throwableMeshCollider.enabled = true;
        }
        else
        {
            if (isGun) throwableGunScript.enabled = true;
            throwableRb.isKinematic = true;
            throwableMeshCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance = (throwableTransform.position - playerTransform.position).magnitude;
        if (!slotFull && distance <= equipDistance && Input.GetKey(KeyCode.Mouse0))
        {
            Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit rayInfo;
            if (Physics.Raycast(ray, out rayInfo))
            {
                if (rayInfo.collider == throwableMeshCollider)
                {
                    PickUp();

                }
            }
        }

        if (slotFull && isEquipped && (Input.GetKey(KeyCode.Mouse1) || (!isGun && Input.GetKey(KeyCode.Mouse0))))
        {
            Throw();
        }
    }

    void PickUp()
    {
        TimeManager.Instance.SpeedUpInstant(0.8f, 0.2f);
        if (isGun) throwableGunScript.enabled = true;
        throwableRb.isKinematic = true;
        throwableMeshCollider.enabled = false;
        isEquipped = true;
        slotFull = true;
        transform.SetParent(gunContainerTransform);
        transform.localPosition = weaponOffset;
        transform.localRotation = Quaternion.Euler(weaponRotation);
        transform.localScale = Vector3.one;
    }

    void Throw()
    {
        TimeManager.Instance.SpeedUpInstant(0.8f, 0.2f);
        transform.SetParent(null);
        if (isGun) throwableGunScript.enabled = false;
        throwableRb.isKinematic = false;
        throwableMeshCollider.enabled = true;
        isEquipped = false;
        slotFull = false;
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit rayInfo;
        Vector3 direction = new Vector3(0, 0, 0);
        if (Physics.Raycast(ray, out rayInfo)) 
        {
            direction = (rayInfo.point - firePos.transform.position).normalized;
        }
        else
        {
            direction = ray.GetPoint(50).normalized;
        }
        throwableRb.AddForce(direction * throwForceForwards, ForceMode.Impulse);
        throwableRb.AddForce(firePos.up * throwForceUpwards, ForceMode.Impulse);
        throwableRb.AddTorque(new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), ForceMode.Impulse);
    }
}
