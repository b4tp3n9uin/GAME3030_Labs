using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    [Header("Weapon Spawn"), SerializeField]
    GameObject Weapon;

    public PlayerControls playerController;
    Animator playerAnim;

    public Sprite crossHair;
    WeaponComponent equippedWeapon;

    [SerializeField]
    GameObject SocketLocation;
    [SerializeField]
    Transform GripSocketIK_Loacation;
    
    public bool FiringPressed = false;

    public readonly int isFiringHash = Animator.StringToHash("IsFiring");
    public readonly int isReloadingHash = Animator.StringToHash("IsReloading");

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerControls>();
        playerAnim = GetComponent<Animator>();

        GameObject SpawnedWeapon = Instantiate(Weapon, SocketLocation.transform.position, SocketLocation.transform.rotation, SocketLocation.transform);

        equippedWeapon = SpawnedWeapon.GetComponent<WeaponComponent>();
        equippedWeapon.Initialize(this);
        GripSocketIK_Loacation = equippedWeapon.gripLocation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!playerController.isReloading)
        {
            playerAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            playerAnim.SetIKPosition(AvatarIKGoal.LeftHand, GripSocketIK_Loacation.transform.position);
        }
        
    }

    public void OnFire(InputValue i_value)
    {
        FiringPressed = i_value.isPressed;
        
        if (FiringPressed)
        {
            StartFiring();
        }
        else
        {
            StopFiring();
        }
    }

    void StartFiring()
    {
        if (equippedWeapon.weaponStats.bulletsInClip <= 0)
            return;
        else if (equippedWeapon.weaponStats.bulletsInClip > 0)
        {
            // Set up firing animations.
            playerAnim.SetBool(isFiringHash, true);
            playerController.isFiring = true;
            equippedWeapon.StartFiringWeapon();
        }

    }

    void StopFiring()
    {
        playerAnim.SetBool(isFiringHash, false);
        playerController.isFiring = false;
        equippedWeapon.StopFiringWeapon();
    }

    public void OnReload(InputValue i_value)
    {
        playerController.isReloading = i_value.isPressed;

        // Set up reload animations.
        playerAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
        playerAnim.SetBool(isReloadingHash, playerController.isReloading);
    }

    public void StartReloading()
    {
        Debug.Log("Reload");
    }

}
