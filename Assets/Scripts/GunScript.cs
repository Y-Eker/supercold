using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public int bulletsLeft;

    [Header("References")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] Transform firePos;
    [SerializeField] Camera mainCam;

    [Header("Gun Stats")]
    public int bulletsPerTap;
    [SerializeField] float reloadTime, spread, fireRate, timeBetweenShots, bulletForce;
    [SerializeField] int magazineSize;
    [SerializeField] bool allowButtonHold;

    private Vector3 center = new Vector3(0.5f, 0.5f, 0);
    private Vector3 targetPoint;
    private Vector3 directionWithoutSpread;
    private Vector3 directionWithSpread;
    private int bulletsShot;
    private bool readyToShoot, shootPressed, reloading, allowInvoke;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        allowInvoke = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (allowButtonHold)
        {
            shootPressed = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shootPressed = Input.GetKeyDown(KeyCode.Mouse0);
        }
        if (shootPressed)
        {
            TimeManager.Instance.SpeedUpInstant(0.8f, 0.1f);
        }
        if (readyToShoot && shootPressed && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
        if ((!reloading && bulletsLeft < magazineSize && Input.GetKeyDown(KeyCode.R)) || (readyToShoot && shootPressed && !reloading && bulletsLeft <= 0)) 
        {
            Reload();
        }
    }

    void Shoot()
    {
        readyToShoot = false;
        muzzleFlash.Play();

        Ray ray = mainCam.ViewportPointToRay(center);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }
        // calculates directions
        directionWithoutSpread = targetPoint - firePos.position;
        directionWithSpread = directionWithoutSpread + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0f);
        directionWithSpread.Normalize();
        // Instantiates a bullet and sets its forward direction to the desired direction
        GameObject currentBullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        currentBullet.transform.forward = directionWithSpread;
        // Gets Rigidbody of the Bullet and applies a force to it
        Rigidbody currentBulletRb = currentBullet.GetComponent<Rigidbody>();
        currentBulletRb.AddForce(currentBullet.transform.forward * bulletForce, ForceMode.Impulse);
        // 
        bulletsLeft--;
        bulletsShot++;

        // Adds a delay to shooting
        if (allowInvoke)
        {
            Invoke("ResetShot", 1 / fireRate);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    void ReloadFinished()
    {
        reloading = false;
        bulletsLeft = magazineSize;
    }
}
