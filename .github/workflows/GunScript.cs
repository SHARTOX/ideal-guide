using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunShoot : MonoBehaviour
{
    //public float BulletSpeed = 300f;
    //public Rigidbody bullet;
    //public Transform BulletPosition;
    public Transform camera;

    bool shootingPeriode = false;
    public float coolDownPeriode = 10f;
    public bool SemiGun = true;
    public float firerate = 1f;
    float nextFireRate = 0f;

    public GameObject gun;
    public Animator anim;

    public bool isCrouching = false;
    bool isAiming = false;
    bool isSprinting = false;
    public float range = 1000f;
    public int damage = 50;

    public GameObject BulletPositio;
    public GameObject impactEffect;
    public GameObject bloodEffect;
    public Vector3 spreadV;
    public ParticleSystem muzzleFlash;

    public int clipSize;
    public int currentAmmo;
    public int ammo;
    public int thisAmmo;
    public float reloadTime;
    int bulletsToReload;
    public bool isCapableToShoot = true;
    public bool didSound = false;
    public float spread;
    public float crouchSpread;
    public float AimSpread;

    public AudioSource Emptysnd;
    public AudioSource Gunsnd;
    public AudioSource reloading;

    public Text ammoDisp;

    public int ammoDifference;
    public AudioSource AmmoPick;

    public GameObject playerLook;

    //This Update will call in every frame
    void Update()
    {
        ifStatsFunc();
    }

    void ifStatsFunc()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if(Input.GetButtonDown("Fire1") && shootingPeriode == false && SemiGun == true && isSprinting == false && currentAmmo > 0 && isCapableToShoot == true)
        {
            Gunsnd.Play();
            Shoot();
            currentAmmo = currentAmmo - 1;
            didSound = true;
            Invoke(nameof(fadeSound), 0.2f);
            muzzleFlash.Play();

        }

        if(Input.GetButton("Fire1") && Time.time > nextFireRate && isSprinting == false && currentAmmo > 0 && isCapableToShoot == true && SemiGun == false)
        {
            Gunsnd.Play();
            nextFireRate = Time.time + firerate;
            ShootAuto();
            currentAmmo = currentAmmo - 1;
            didSound = true;
            Invoke(nameof(fadeSound), 0.2f);
            muzzleFlash.Play();
        }

        if(Input.GetButtonDown("Fire2") && isAiming == false)
        {
            AimDownSight();
        }

        if(Input.GetButtonUp("Fire2") && isAiming == true)
        {
            AimUpSight();
        }

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }

        if(Input.GetKeyDown(KeyCode.R) && currentAmmo < clipSize && isCapableToShoot != false && ammo != 0)
        {
            Reload();
            anim.SetBool("isReloading", true);
            Invoke(nameof(StopReloading), reloadTime);
            isCapableToShoot = false;
            reloading.Play();

        }

        if(Input.GetButtonDown("Fire1") && currentAmmo == 0)
        {
            Emptysnd.Play();
        }

        ammoDisp.text = currentAmmo + "/" + ammo;


    }

    void Shoot()
    {
//Some Preferences:
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        float xc = Random.Range(-crouchSpread, crouchSpread);
        float yc = Random.Range(-crouchSpread, crouchSpread);

        float xa = Random.Range(-AimSpread, AimSpread);
        float ya = Random.Range(-AimSpread, AimSpread);

        Vector3 direction = BulletPositio.transform.forward + new Vector3(x, y, 0f);
        Vector3 directionc = BulletPositio.transform.forward + new Vector3(xc, yc, 0f);
        Vector3 directionA = BulletPositio.transform.forward + new Vector3(xa, ya, 0f);

        if(Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

//Aiming Without Crouching Shooting:

        if(isAiming == true && isCrouching == false)
        {
            RaycastHit hit;
            if(Physics.Raycast(BulletPositio.transform.position, directionA, out hit, range))
            {
                Target enemy = hit.transform.GetComponent<Target>();

                if(enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if(hit.transform.tag == "floor")
                {
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }

        if(isAiming == false && isCrouching == false)
        {
            RaycastHit hit;
            if(Physics.Raycast(BulletPositio.transform.position, direction, out hit, range))
            {
                Target enemy = hit.transform.GetComponent<Target>();

                if(enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if(hit.transform.tag == "floor")
                {
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
//Crouching Without Aiming Shooting:
        if(isCrouching == false && isAiming == false)
        {
            RaycastHit hit;
            if(Physics.Raycast(BulletPositio.transform.position, direction, out hit, range))
            {
                Target enemy = hit.transform.GetComponent<Target>();

                if(enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if(hit.transform.tag == "floor")
                {
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }

        if(isCrouching == true && isAiming == false)
        {
            RaycastHit hit;
            if(Physics.Raycast(BulletPositio.transform.position, directionc, out hit, range))
            {
                Target enemy = hit.transform.GetComponent<Target>();

                if(enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if(hit.transform.tag == "floor")
                {
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
        
        shootingPeriode = true;
        Invoke(nameof(coolDown), coolDownPeriode);
    }


    void ShootAuto()
    {
//Some Preferences:
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        float xc = Random.Range(-crouchSpread, crouchSpread);
        float yc = Random.Range(-crouchSpread, crouchSpread);

        float xa = Random.Range(-AimSpread, AimSpread);
        float ya = Random.Range(-AimSpread, AimSpread);

        Vector3 direction = BulletPositio.transform.forward + new Vector3(x, y, 0f);
        Vector3 directionc = BulletPositio.transform.forward + new Vector3(xc, yc, 0f);
        Vector3 directionA = BulletPositio.transform.forward + new Vector3(xa, ya, 0f);

        if(Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

//Aiming Without Crouching Shooting:

        if(isAiming == true && isCrouching == false)
        {
            RaycastHit hit;
            if(Physics.Raycast(BulletPositio.transform.position, directionA, out hit, range))
            {
                Target enemy = hit.transform.GetComponent<Target>();

                if(enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if(hit.transform.tag == "floor")
                {
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }

        if(isAiming == false && isCrouching == false)
        {
            RaycastHit hit;
            if(Physics.Raycast(BulletPositio.transform.position, direction, out hit, range))
            {
                Target enemy = hit.transform.GetComponent<Target>();

                if(enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if(hit.transform.tag == "floor")
                {
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }

//Crouching Without Aiming Shooting:

        if(isCrouching == false && isAiming == false)
        {
            RaycastHit hit;
            if(Physics.Raycast(BulletPositio.transform.position, direction, out hit, range))
            {
                Target enemy = hit.transform.GetComponent<Target>();

                if(enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if(hit.transform.tag == "floor")
                {
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }

        if(isCrouching == true && isAiming == false)
        {
            RaycastHit hit;
            if(Physics.Raycast(BulletPositio.transform.position, directionc, out hit, range))
            {
                Target enemy = hit.transform.GetComponent<Target>();

                if(enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if(hit.transform.tag == "floor")
                {
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }

//Crouching And Aiming Shooting:
        if(isCrouching == true && isAiming == true)
        {
            RaycastHit hit;
            if(Physics.Raycast(BulletPositio.transform.position, directionA, out hit, range))
            {
                Target enemy = hit.transform.GetComponent<Target>();

                if(enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if(hit.transform.tag == "floor")
                {
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
    }

    void coolDown()
    {
        shootingPeriode = false;
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    void AimDownSight()
    {
        isAiming = true;
        anim.SetBool("Ads", true);
    }

    void AimUpSight()
    {
        isAiming = false;
        anim.SetBool("Ads", false);
    }

    void Reload()
    {
        int bulletsToReload = clipSize - currentAmmo;
        
        if(ammo > bulletsToReload)
        {
            StartCoroutine(ReloadPeriodeAmmoGreater());
        }

        if(ammo <= bulletsToReload)
        {
            StartCoroutine(ReloadPeriodeAmmoLess());
        }
    }

    void StopReloading()
    {
        anim.SetBool("isReloading", false);
    }

    void StopShooting()
    {
        anim.SetBool("isShooting", false);
    }

    void fadeSound()
    {
        didSound = false;
    }

    IEnumerator ReloadPeriodeAmmoGreater()
    {
        yield return new WaitForSeconds (reloadTime);

            int bulletsToReload = clipSize - currentAmmo;

            ammo -= bulletsToReload;

            currentAmmo = currentAmmo + bulletsToReload;

            isCapableToShoot = true;

    }

    IEnumerator ReloadPeriodeAmmoLess()
    {
        yield return new WaitForSeconds (reloadTime);

            currentAmmo += ammo;
            ammo = 0;
            isCapableToShoot = true;

    }

}
