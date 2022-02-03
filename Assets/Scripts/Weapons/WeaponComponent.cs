using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    NONE, PISTOL, RIFLE
};

public enum FireType
{
    SEMI_AUTO, FULL_AUTO, THREE_BURST, PUMP
}

[System.Serializable]
public struct WeaponStats
{
    public WeaponType weaponType;
    public FireType firePattern;
    public string weaponName;
    public float damage;
    public int clipSize;
    public int bulletsInClip;
    public float fireStartDelay;
    public float fireRate;
    public float fireDistance;
    public bool repeating;
    public LayerMask WeaponHitLayers;
}

public class WeaponComponent : MonoBehaviour
{
    public Transform gripLocation;
    public WeaponStats weaponStats;
    protected WeaponHolder weaponHolder;
    public bool isFiring;
    public bool isReloading;
    public Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(WeaponHolder _weapon)
    {
        weaponHolder = _weapon;
    }

    public virtual void StartFiringWeapon()
    {
        isFiring = true;
        if (weaponStats.repeating)
        {
            //Fire Weapon.
            InvokeRepeating(nameof(FireWeapon), weaponStats.fireStartDelay, weaponStats.fireRate);
        }
        else
        {
            FireWeapon();
        }
    }

    public virtual void StopFiringWeapon()
    {
        isFiring = false;
        CancelInvoke(nameof(FireWeapon));
    }

    protected virtual void FireWeapon()
    {
        if (weaponStats.bulletsInClip <= 0)
            StopFiringWeapon();
        else
        {
            print("fire weapon.");
            weaponStats.bulletsInClip--;
            print(weaponStats.bulletsInClip);

        }

    }
}
